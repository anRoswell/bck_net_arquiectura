namespace Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using Core.DTOs;
    using Core.DTOs.CargaMasiva;
    using Core.DTOs.FilesDto;
    using Core.DTOs.Gos.CargarOrdenesAreaCentral;
    using Core.Enumerations;
    using Core.Extensions;
    using Core.Interfaces;
    using Core.QueryFilters;
    using Infrastructure.Interfaces;
    using Microsoft.AspNetCore.Hosting;
    using OfficeOpenXml;

    public class CargaMasivaGosService : ICargaMasivaConfiguracion
    {
        #region Parameters
        private readonly IFilesProcess _filesProcess;
        private readonly IDynamicBulkService _dynamicBulkService;
        private readonly IOp360Service _op360Service;
        public CargaInicial CargaInicial => CargaInicial.Gos;
        private readonly IWebHostEnvironment _environment;
        #endregion

        public CargaMasivaGosService(
            IFilesProcess filesProcess, 
            IDynamicBulkService dynamicBulkService, 
            IOp360Service op360Service,
            IWebHostEnvironment environment
            )
        {
            _filesProcess = filesProcess;
            _dynamicBulkService = dynamicBulkService;
            _op360Service = op360Service;
            _environment = environment;
        }

        public async Task<ResponseDto<MedirTiempo>> Procesar(object parameter, int myUser)
        {
            CargarOrdenesAreaCentralRequestDto parameter2 = (CargarOrdenesAreaCentralRequestDto)parameter;
            string fileBase64 = parameter2.FileBase64;
            MedirTiempo t = new();
            try
            {
                Stopwatch stopwatch = new();
                long tiempoTotal = 0;
                stopwatch.Start();
                byte[] excelFileBytes = fileBase64.Base64ToBytes();
                if (excelFileBytes == null)
                {
                    return new ResponseDto<MedirTiempo>()
                    {
                        Codigo = 1,
                        Mensaje = $"Se presento un error en el archivo base 64",
                        Datos = t
                    };
                }
                
                Enum_RutasArchivos Id_Ruta = _environment.EnvironmentName.ToString() == ApiEnvironments.DevLocal ? Enum_RutasArchivos.GosFileLocal : Enum_RutasArchivos.GosFileProd;
                Op360FileBase64 parameters = new()
                {
                    FileBase64 = fileBase64,
                    Extension = ".xlsx",
                    Id_Ruta = Id_Ruta,
                    Actividad = Enum_gnl_actividades.G_GOS.ToString(),
                    Tipos_Soporte = TipoSoporteEnum.GOCM.ToString(),
                    id_usuario = myUser
                };

                FileResponseOracle filesReq = await _filesProcess.SaveFileOracleBase64_Gos(parameters);
                var id_soporte = filesReq.Id_Soporte;
                t.id_soporte = id_soporte;
                stopwatch.Stop();
                tiempoTotal += stopwatch.ElapsedMilliseconds;
                t.GuardarArchivo = $"Tiempo en guardar archivo: {stopwatch.ElapsedMilliseconds} Milisegundos.";
                stopwatch.Reset();
                stopwatch.Start();
                var columnDefinitions = new Dictionary<string, Type>
                {
                    { "ORDEN", typeof(int) },
                    { "CUENTA", typeof(int) },
                    { "NOMBRE_CLIENTE", typeof(string) },
                    { "DIRECCION", typeof(string) },
                    { "MUNICIPIO", typeof(string) },
                    { "BARRIO", typeof(string) },
                    { "NOMBRE_GESTOR", typeof(string) },
                    { "CEDULA_GESTOR", typeof(int) },
                    { "FECHA_PROGRAMACION", typeof(string) },
                    { "ID_SOPORTE", typeof(int) },
                    { "USUARIO_REGISTRA", typeof(string) },
                };
                int registrosExcel = await ProcessData(excelFileBytes, id_soporte, myUser, columnDefinitions);
                stopwatch.Stop();
                tiempoTotal += stopwatch.ElapsedMilliseconds;
                t.CargueDataTemporal = $"Tiempo en cargar la data a la tabla temporal: {stopwatch.ElapsedMilliseconds} Milisegundos.";
                stopwatch.Reset();
                stopwatch.Start();
                QueryOp360CargueMasivoValidacion tmp = new()
                {
                    id_soporte = filesReq.Id_Soporte,
                    usuario_registra = myUser.ToString(),
                    nombre_archivo = filesReq.NombreInterno
                };
                var listTest2 = await _op360Service.Guardar_Orden_Masivo_Final_Gos(tmp);
                if (listTest2.Codigo != 200)
                {
                    return new ResponseDto<MedirTiempo>()
                    {
                        Codigo = listTest2.Codigo,
                        Mensaje = listTest2.Mensaje ?? "ocurrio un error al realizar el procesamiento final de los datos.",
                        Datos = t
                    };
                }
                stopwatch.Stop();
                tiempoTotal += stopwatch.ElapsedMilliseconds;
                t.Procesamiento = $"Tiempo en procesar la información: {stopwatch.ElapsedMilliseconds} Milisegundos.";
                t.TiempoTotal = $"Tiempo total proceso backend: {tiempoTotal} Milisegundos.";
                stopwatch.Reset();

                return new ResponseDto<MedirTiempo>()
                {
                    Codigo = 0,
                    Mensaje = "Se guardaron las ordenes correctamente",
                    Datos = t,
                    TotalRecords = registrosExcel
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<MedirTiempo>()
                {
                    Codigo = 1,
                    Mensaje = ex.Message,
                    Datos = t
                };
            }
        }

        #region Private Methods
        private async Task<int> ProcessData(byte[] excelFileBytes, int id_soporte, int myUser, Dictionary<string, Type> columnDefinitions)
        {
            int registrosExcel;
            using (var excelPackage = new ExcelPackage(new MemoryStream(excelFileBytes)))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var worksheet = excelPackage.Workbook.Worksheets[0];
                registrosExcel = worksheet.Dimension.Rows;
                if (registrosExcel <= 0) throw new Exception("No se encontraron registros para cargar");
                int partes = 50000;
                int cantidad = registrosExcel;
                int cnt = cantidad / partes;
                cnt = cantidad > partes ? cnt + 1 : (cantidad < partes ? cnt + 1 : cnt);
                int ini = 2;
                int fin = cantidad < partes ? cantidad : partes;
                for (int ix = 1; ix <= cnt; ix++)
                {
                    var data = new List<Dictionary<string, object>>();
                    for (int row = ini; row <= fin; row++)
                    {
                        var rowData = new Dictionary<string, object>
                        {
                            { "ORDEN", row },
                            { "CUENTA", worksheet.Cells[row, 1].Value?.ToString() },
                            { "NOMBRE_CLIENTE", worksheet.Cells[row, 2].Value?.ToString() },
                            { "DIRECCION", worksheet.Cells[row, 3].Value?.ToString() },
                            { "MUNICIPIO", worksheet.Cells[row, 4].Value?.ToString() },
                            { "BARRIO", worksheet.Cells[row, 5].Value?.ToString() },
                            { "NOMBRE_GESTOR", worksheet.Cells[row, 6].Value?.ToString() },
                            { "CEDULA_GESTOR", worksheet.Cells[row, 7].Value?.ToString() },
                            { "FECHA_PROGRAMACION", worksheet.Cells[row, 8].Value?.ToString() },
                            { "ID_SOPORTE", id_soporte },
                            { "USUARIO_REGISTRA", myUser },
                        };
                        if (!string.IsNullOrEmpty(worksheet.Cells[row, 1].Value?.ToString())) data.Add(rowData);
                    }
                    await _dynamicBulkService.BulkCopyAsync("gos_ordenes_cargue_temporal", columnDefinitions, data);
                    ini = fin + ((cantidad > fin) ? 1 : 0);
                    fin = fin + partes;
                    fin = (fin > cantidad) ? cantidad : fin;
                    if (ini == fin) break;
                }
            }
            return registrosExcel;
        }


        #endregion
    }
}