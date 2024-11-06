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
    using Core.Options;
    using Core.QueryFilters;
    using Core.Tools;
    using Infrastructure.Interfaces;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Options;
    using OfficeOpenXml;

    public class CargaMasivaScrReasigOrden2Service : ICargaMasivaConfiguracion
    {
        #region Parameters
        private readonly IFilesProcess _filesProcess;
        private readonly IDynamicBulkService _dynamicBulkService;
        private readonly IOp360Service _op360Service;
        public CargaInicial CargaInicial => CargaInicial.ScrReasigOrden2;
        private readonly IWebHostEnvironment _environment;
        private readonly ParametrosCarguesOptions _parametrosCarguesOptions;
        #endregion

        public CargaMasivaScrReasigOrden2Service(
            IFilesProcess filesProcess, 
            IDynamicBulkService dynamicBulkService, 
            IOp360Service op360Service,
            IWebHostEnvironment environment,
            IOptions<ParametrosCarguesOptions> parametrosCarguesOptions
            )
		{
            _filesProcess = filesProcess;
            _dynamicBulkService = dynamicBulkService;
            _op360Service = op360Service;
            _environment = environment;
            _parametrosCarguesOptions = parametrosCarguesOptions.Value;
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
                Enum_RutasArchivos Id_Ruta = _environment.EnvironmentName.ToString() == ApiEnvironments.DevLocal ? Enum_RutasArchivos.ExcelScrMasivOrdBitesLocal : Enum_RutasArchivos.ExcelScrMasivOrdBites;
                QueryOp360FileBase64 parameters = new()
                {
                    FileBase64 = fileBase64,
                    Extension = ".xlsx",
                    Id_Ruta = Id_Ruta,
                    Actividad = Enum_gnl_actividades.G_SCR.ToString(),
                    Tipos_Soporte = TipoSoporteEnum.SRAO2.ToString(),
                    id_usuario = myUser
                };
                //-- Se validan las columnas del archivo antes de guardarlo en disco.
                int validacion = Funciones.fnValidarExcel(excelFileBytes, _parametrosCarguesOptions.MasivoReasig2OrdenClmns);

                ResponseDto<FileResponseOracle> filesReq = await _filesProcess.SaveFileOracleBase64_Scr(parameters);
                var id_soporte = filesReq.Datos.Id_Soporte;
                t.id_soporte = id_soporte;
                stopwatch.Stop();
                tiempoTotal += stopwatch.ElapsedMilliseconds;
                t.GuardarArchivo = $"Tiempo en guardar archivo: {stopwatch.ElapsedMilliseconds} Milisegundos.";
                stopwatch.Reset();
                stopwatch.Start();
                var columnDefinitions = new Dictionary<string, Type>
                {
                    { "ORDEN", typeof(int) },
                    { "NUMERO_ORDEN", typeof(string) },
                    { "CODIGO_TIPO_SUSPENCION", typeof(string) },
                    { "CODIGO_CONTRATISTA", typeof(string) },
                    { "USUARIO_REGISTRA", typeof(string) },
                    { "ID_SOPORTE", typeof(int) }
                };
                int registrosExcel = await ProcessData(excelFileBytes, id_soporte, myUser, columnDefinitions);
                stopwatch.Stop();
                tiempoTotal += stopwatch.ElapsedMilliseconds;
                t.CargueDataTemporal = $"Tiempo en cargar la data a la tabla temporal: {stopwatch.ElapsedMilliseconds} Milisegundos.";
                stopwatch.Reset();
                stopwatch.Start();
                QueryOp360CargueMasivoValidacion tmp = new()
                {
                    id_soporte = filesReq.Datos.Id_Soporte,
                    usuario_registra = myUser.ToString(),
                    nombre_archivo = filesReq.Datos.NombreInterno
                };
                var listTest2 = await _op360Service.Procesar_Masivo_Reasignar_Ordenes2_SCR(tmp);                
                stopwatch.Stop();
                tiempoTotal += stopwatch.ElapsedMilliseconds;
                t.Procesamiento = $"Tiempo en procesar la información: {stopwatch.ElapsedMilliseconds} Milisegundos.";
                t.TiempoTotal = $"Tiempo total proceso backend: {tiempoTotal} Milisegundos.";
                stopwatch.Reset();
                if (listTest2.Codigo != 200 && listTest2.Codigo != 0)
                {
                    return new ResponseDto<MedirTiempo>()
                    {
                        Codigo = listTest2.Codigo,
                        Mensaje = listTest2.Mensaje ?? "ocurrio un error al realizar el procesamiento final de los datos.",
                        Datos = t
                    };
                }
                return new ResponseDto<MedirTiempo>()
                {
                    Codigo = 0,
                    Mensaje = $"Proceso terminado, {listTest2.Mensaje}",
                    Datos = t,
                    TotalRecords = registrosExcel
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<MedirTiempo>()
                {
                    Codigo = 1,
                    Mensaje = $"{ex.Message}, {ex.InnerException?.Message ?? ""}",
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
                registrosExcel = worksheet.Dimension.Rows - 1;
                if (registrosExcel <= 0) throw new Exception("No se encontraron registros para cargar");
                int partes = 50000;
                int cantidad = worksheet.Dimension.Rows;
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
                            { "NUMERO_ORDEN", worksheet.Cells[row, 1].Value?.ToString() },
                            //{ "CODIGO_TIPO_SUSPENCION", worksheet.Cells[row, 2].Value?.ToString() },                            
                            { "CODIGO_CONTRATISTA", worksheet.Cells[row, 2].Value?.ToString() },
                            { "USUARIO_REGISTRA", myUser },
                            { "ID_SOPORTE", id_soporte }
                        };
                        if (!string.IsNullOrEmpty(worksheet.Cells[row, 1].Value?.ToString())) data.Add(rowData);
                    }
                    await _dynamicBulkService.BulkCopyAsync("ORD_ORDENES_CARGUE_TEMPORAL_REASIGNACION", columnDefinitions, data);
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