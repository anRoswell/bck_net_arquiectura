namespace Core.Services
{
    using Core.DTOs;
    using Core.DTOs.FilesDto;
    using Core.DTOs.Gos.Web.ConsultaOrdenesFechaDto;
    using Core.DTOs.Gos.Web.ConsultarOrdenesGestion;
    using Core.DTOs.ObtenerReporteTraza;
    using Core.Entities;
    using Core.Enumerations;
    using Core.Exceptions;
    using Core.Interfaces;
    using Core.Options;
    using Core.QueryFilters;
    using Core.Tools;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class Op360ReportingService : IOp360ReportingService
    {
        private readonly IUnitOfWork _unitOfWork;        
        private readonly IFilesProcess _filesProcess;
        private readonly PathOptions _pathOptions;        
        private readonly IOp360Service _op360Service;
        private readonly HttpClient _httpClient;
        private readonly ParametrosOptions _parametrosOptions;
        private readonly IOp360GosWebService _op360GosWebService;

        public Op360ReportingService(
            IUnitOfWork unitOfWork,            
            IFilesProcess filesProcess,
            IOptions<PathOptions> pathOptions,
            IOp360Service op360Service,
            HttpClient httpClient,
            IOptions<ParametrosOptions> parametrosOptions,
            IOp360GosWebService op360GosWebService
        )
        {
            _unitOfWork = unitOfWork;            
            _filesProcess = filesProcess;
            _pathOptions = pathOptions.Value;
            _op360Service = op360Service;
            _parametrosOptions = parametrosOptions.Value;
            _op360GosWebService = op360GosWebService;

            // Configurar el cliente HTTP para ignorar la validación del certificado SSL
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            _httpClient = new HttpClient(handler);
        }
        
        public async Task<byte[]> ActaOsPdf(QueryActaOsPdf parameters)
        {
            try
            {
                var data = await _op360Service.GetData_Ordenes(parameters);

                // Definir la ruta del archivo HTML
                string htmlFile = @"D:\WorkSpaceGIT2023\SyspotecOracle4\PortalOPE360NetProject\Ope360BackendNetProject\Documentacion\Html Template Daniel\TablaActaOsDemo.html";

                // Leer el contenido del archivo HTML
                string htmlContent = System.IO.File.ReadAllText(htmlFile);

                foreach (var item in data.datos)
                {
                    htmlContent = htmlContent.Replace("%" + item.key + "%", item.valor);
                }

                Tuple<string, byte[]> tmp = Funciones.PdfSharpConvertToBytes2(htmlContent, "Acta_Orden");
                return tmp.Item2;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<FileResponsePdfDto> ObtenerJasperReportPdf(QueryJasperReportsParams parameters)
        {
            try
            {
                FileResponsePdfDto response = new FileResponsePdfDto();

                // URL del archivo PDF                
                Enum_RutasArchivos ras = Enum_RutasArchivos.ServerJasperReportsUrlBase;
                gnl_rutas_archivo_servidor rootFileServer = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)ras);
                Dictionary<string, string> QryPrmsDic0 = new Dictionary<string, string>()
                {
                    { "_repName", parameters._repName.Trim() },
                    { "_repFormat", parameters._repFormat.Trim() }
                };
                Dictionary<string, string> QryPrmsDic1 = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(parameters._repParams);
                QryPrmsDic0 = QryPrmsDic0.Concat(QryPrmsDic1).ToDictionary(x => x.Key, x => x.Value.Trim());
                var _queryParams = $"?{string.Join("&", QryPrmsDic0.Select(x => string.Concat(x.Key, "=", x.Value)).ToArray())}";

                string UrlCompleta = new Uri(
                                    new Uri(rootFileServer.RUTA_WEB),
                                    _queryParams).ToString().Trim();                

                // Realizar la solicitud HTTP GET
                response.Response = await _httpClient.GetAsync(UrlCompleta);

                // Verificar si la solicitud fue exitosa
                if (response.Response.IsSuccessStatusCode)
                {
                    // Leer el contenido de la respuesta
                    response.File = await response.Response.Content.ReadAsStreamAsync();
                    response.StatusCode = response.Response.StatusCode;
                    response.Mensaje = $"Archivo generado correctamente.";
                    response.UrlReporteJasper = UrlCompleta;
                }
                else
                {
                    response.StatusCode = response.Response.StatusCode;
                    response.Mensaje = $"Error al descargar el archivo PDF, {response.Response.RequestMessage}";
                    response.UrlReporteJasper = UrlCompleta;
                }
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        public async Task<ResponseDto<(FileResponseOracleBase64, byte[])>> Consultar_Ordenes_Area_Central_Excel(QueryOp360Ordenes parameters, string format)
        {
            try
            {
                //QueryOp360ObtenerGnlPlantilla parameters1 = new QueryOp360ObtenerGnlPlantilla()
                //{
                //    codigo = "LOG01"
                //};
                //var param1 = JsonConvert.SerializeObject(parameters1);
                //ResponseDto<FileResponseReportesJasper> tmplogo = await _unitOfWork.StoreProcedure<ResponseDto<FileResponseReportesJasper>>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_ordenes.prc_descargar_archivo_gnl_plantilla", param1);
                //tmplogo.Datos.archivobyte = Convert.FromBase64String(tmplogo.Datos.archivobase64);

                // Agregamos Logo institucional
                QueryOp360ConfigExcel ConfigExcel = new QueryOp360ConfigExcel() { 
                    rootFileServer = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)(_parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? Enum_RutasArchivos.ExcelLogoLocal : Enum_RutasArchivos.ExcelLogo)),
                    TypeMime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    TituloReporteExcel = "Listado de Ordenes Area Central",
                    NombreArchivo = string.Concat(Core.Tools.Funciones.GetCodigoUnico("Op360Scr"), ".xlsx")
                    //ArchivoByte = tmplogo.Datos.archivobyte
                };

                var listTest = await _op360Service.Consultar_Ordenes_Area_Central(parameters, true);                
                return await _unitOfWork.ExcelGenerador<ResponseDto<FileResponseOracleBase64>>().ExecuteExcelGenerador(listTest.Datos.ordenes.AsQueryable(), format, ConfigExcel);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<ResponseDto<(FileResponseOracleBase64, byte[])>> Consultar_Ordenes_Contratistas_Excel(QueryOp360OrdenesContratistas parameters, string format)
        {
            try
            {
                // Agregamos Logo institucional
                QueryOp360ConfigExcel ConfigExcel = new QueryOp360ConfigExcel()
                {
                    rootFileServer = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)(_parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? Enum_RutasArchivos.ExcelLogoLocal : Enum_RutasArchivos.ExcelLogo)),
                    TypeMime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    TituloReporteExcel = "Listado de Ordenes Contratistas",
                    NombreArchivo = string.Concat(Core.Tools.Funciones.GetCodigoUnico("Op360Scr"), ".xlsx")
                };
                var listTest = await _op360Service.Consultar_Ordenes_Contratistas(parameters, true);
                return await _unitOfWork.ExcelGenerador<ResponseDto<FileResponseOracleBase64>>().ExecuteExcelGenerador(listTest.Datos.ordenes.AsQueryable(), format, ConfigExcel);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<ResponseDto<(FileResponseOracleBase64, byte[])>> Consultar_Archivos_Instancia_Excel(QueryOp360ArchivosInstancia parameters, string format)
        {
            try
            {
                // Agregamos Logo institucional
                QueryOp360ConfigExcel ConfigExcel = new QueryOp360ConfigExcel()
                {
                    rootFileServer = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)(_parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? Enum_RutasArchivos.ExcelLogoLocal : Enum_RutasArchivos.ExcelLogo)),
                    TypeMime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    TituloReporteExcel = "Listado Archivos Instancia",
                    NombreArchivo = string.Concat(Core.Tools.Funciones.GetCodigoUnico("Op360Scr"), ".xlsx")
                };
                var listTest = await _op360Service.Consultar_Archivos_Instancia(parameters, true);
                return await _unitOfWork.ExcelGenerador<ResponseDto<FileResponseOracleBase64>>().ExecuteExcelGenerador(listTest.Datos.archivos_instancia.AsQueryable(), format, ConfigExcel);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<ResponseDto<(FileResponseOracleBase64, byte[])>> Consultar_Archivos_Instancia_Detalle_Excel(QueryOp360ArchivosInstanciaDetalle parameters, string format)
        {
            try
            {
                // Agregamos Logo institucional
                QueryOp360ConfigExcel ConfigExcel = new QueryOp360ConfigExcel()
                {
                    rootFileServer = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)(_parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? Enum_RutasArchivos.ExcelLogoLocal : Enum_RutasArchivos.ExcelLogo)),
                    TypeMime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    TituloReporteExcel = "Listado Archivos Instancia",
                    NombreArchivo = string.Concat(Core.Tools.Funciones.GetCodigoUnico("Op360Scr"), ".xlsx")
                };
                var listTest = await _op360Service.Consultar_Archivos_Instancia_Detalle(parameters, true);
                return await _unitOfWork.ExcelGenerador<ResponseDto<FileResponseOracleBase64>>().ExecuteExcelGenerador(listTest.Datos.archivos_instancia_detalle.AsQueryable(), format, ConfigExcel);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
        
        public async Task<ResponseDto<(FileResponseOracleBase64, byte[])>> ConsultarReporteEjecutadosScrExcel(QueryOp360ReporteEjecutados parameters, string format)
        {
            try
            {
                QueryOp360ConfigExcel ConfigExcel = new QueryOp360ConfigExcel()
                {
                    rootFileServer = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)(_parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? Enum_RutasArchivos.ExcelLogoLocal : Enum_RutasArchivos.ExcelLogo)),
                    TypeMime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    TituloReporteExcel = "Reporte de Ejecutados",
                    NombreArchivo = string.Concat(Core.Tools.Funciones.GetCodigoUnico("Op360Scr"), ".xlsx")
                    //ArchivoByte = tmplogo.Datos.archivobyte
                };

                var listTest = await _op360Service.Consultar_Reporte_Ejecutados(parameters, true);
                return await _unitOfWork.ExcelGenerador<ResponseDto<FileResponseOracleBase64>>().ExecuteExcelGenerador(listTest.Datos.ordenes.AsQueryable(), format, ConfigExcel);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<ResponseDto<(FileResponseOracleBase64, byte[])>> PruebaServerSideExcel(QueryOp360ServerSideTest parameters, string format)
        {
            try
            {
                QueryOp360ConfigExcel ConfigExcel = new QueryOp360ConfigExcel()
                {
                    rootFileServer = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)(_parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? Enum_RutasArchivos.ExcelLogoLocal : Enum_RutasArchivos.ExcelLogo)),
                    TypeMime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    TituloReporteExcel = "Tiulo Reporte",
                    NombreArchivo = string.Concat(Core.Tools.Funciones.GetCodigoUnico("ServerSide"), ".xlsx")
                    //ArchivoByte = tmplogo.Datos.archivobyte
                };

                var listTest = await _op360Service.ServerSideTest(parameters, true);
                return await _unitOfWork.ExcelGenerador<ResponseDto<FileResponseOracleBase64>>().ExecuteExcelGenerador(listTest.Datos.clientes.AsQueryable(), format, ConfigExcel);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<ResponseDto<(FileResponseOracleBase64, byte[])>> ConsultarReporteEjecutadosContratistasScrExcel(QueryOp360ReporteEjecutadosContratistas parameters, string format)
        {
            try
            {
                QueryOp360ConfigExcel ConfigExcel = new QueryOp360ConfigExcel()
                {
                    rootFileServer = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)(_parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? Enum_RutasArchivos.ExcelLogoLocal : Enum_RutasArchivos.ExcelLogo)),
                    TypeMime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    TituloReporteExcel = "Reporte de Ejecutados",
                    NombreArchivo = string.Concat(Core.Tools.Funciones.GetCodigoUnico("Op360Scr"), ".xlsx")
                    //ArchivoByte = tmplogo.Datos.archivobyte
                };

                var listTest = await _op360Service.Consultar_Reporte_Ejecutados_Contratistas(parameters, true);
                return await _unitOfWork.ExcelGenerador<ResponseDto<FileResponseOracleBase64>>().ExecuteExcelGenerador(listTest.Datos.ordenes.AsQueryable(), format, ConfigExcel);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<ResponseDto<(FileResponseOracleBase64, byte[])>> Consultar_Log_LegalizacionScr_Excel(QueryOp360LogLegalizacion parameters, string format)
        {
            try
            {
                // Agregamos Logo institucional
                QueryOp360ConfigExcel ConfigExcel = new QueryOp360ConfigExcel()
                {
                    rootFileServer = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)(_parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? Enum_RutasArchivos.ExcelLogoLocal : Enum_RutasArchivos.ExcelLogo)),
                    TypeMime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    TituloReporteExcel = "Log Legalizacion",
                    NombreArchivo = string.Concat(Core.Tools.Funciones.GetCodigoUnico("Op360LogLegalizacion"), ".xlsx")
                };
                var listTest = await _op360Service.ObtenerReporteLogLegalizacion(parameters,true);
                return await _unitOfWork.ExcelGenerador<ResponseDto<FileResponseOracleBase64>>().ExecuteExcelGenerador(listTest.Datos.reportes_legalizacion.AsQueryable(), format, ConfigExcel);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<ResponseDto<(FileResponseOracleBase64, byte[])>> Consultar_Log_LegalizacionScr_Excel_Contratista(QueryOp360LogLegalizacionContratistas parameters, string format)
        {
            try
            {
                // Agregamos Logo institucional
                QueryOp360ConfigExcel ConfigExcel = new QueryOp360ConfigExcel()
                {
                    rootFileServer = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)(_parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? Enum_RutasArchivos.ExcelLogoLocal : Enum_RutasArchivos.ExcelLogo)),
                    TypeMime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    TituloReporteExcel = "Log Legalizacion",
                    NombreArchivo = string.Concat(Core.Tools.Funciones.GetCodigoUnico("Op360LogLegalizacion"), ".xlsx")
                };
                var listTest = await _op360Service.ObtenerReporteLogLegalizacioContratista(parameters, true);
                return await _unitOfWork.ExcelGenerador<ResponseDto<FileResponseOracleBase64>>().ExecuteExcelGenerador(listTest.Datos.reportes_legalizacion.AsQueryable(), format, ConfigExcel);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<ResponseDto<(FileResponseOracleBase64, byte[])>> Consultar_Reporte_TrazaScr_AreaCentral_Excel(OrdenesRequestDto parameters, string format)
        {
            try
            {
                // Agregamos Logo institucional
                QueryOp360ConfigExcel ConfigExcel = new QueryOp360ConfigExcel()
                {
                    rootFileServer = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)(_parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? Enum_RutasArchivos.ExcelLogoLocal : Enum_RutasArchivos.ExcelLogo)),
                    TypeMime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    TituloReporteExcel = "Reporte Traza AreaCentral",
                    NombreArchivo = string.Concat(Core.Tools.Funciones.GetCodigoUnico("Op360SCRReporteTrazaAreaCentral"), ".xlsx")
                };
                var listTest = await _op360Service.GetReporteTrazaAreaCentralAsync(parameters, true);
                return await _unitOfWork.ExcelGenerador<ResponseDto<FileResponseOracleBase64>>().ExecuteExcelGenerador(listTest.Datos.reporte_traza.AsQueryable(), format, ConfigExcel);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<ResponseDto<(FileResponseOracleBase64, byte[])>> Consultar_Reporte_TrazaScr_Contratistas_Excel(OrdenesContratistaRequestDto parameters, string format)
        {
            try
            {
                QueryOp360ConfigExcel ConfigExcel = new QueryOp360ConfigExcel()
                {
                    rootFileServer = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)(_parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? Enum_RutasArchivos.ExcelLogoLocal : Enum_RutasArchivos.ExcelLogo)),
                    TypeMime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    TituloReporteExcel = "Reporte Traza Contratista",
                    NombreArchivo = string.Concat(Core.Tools.Funciones.GetCodigoUnico("Op360SCRReporteTrazaContratista"), ".xlsx")
                };

                var listTest = await _op360Service.GetReporteTrazaContratistaAsync(parameters, true);
                if (listTest.Datos == null)
                {
                    return new ResponseDto<(FileResponseOracleBase64, byte[])>
                    {
                        Codigo = 1,
                        Mensaje = "No se encontraron contratistas asociados para generar el PDF"
                    };
                }
                else
                {
                    return await _unitOfWork.ExcelGenerador<ResponseDto<FileResponseOracleBase64>>().ExecuteExcelGenerador(listTest.Datos.reporte_traza.AsQueryable(), format, ConfigExcel);
                }
                
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }


        #region Gos
        public async Task<ResponseDto<(FileResponseOracleBase64, byte[])>> ConsultarOrdenesAreaCentralGosExcel(ConsultaOrdenesFechaRequestDto parameters, string format)
        {
            try
            {
                QueryOp360ConfigExcel ConfigExcel = new()
                {
                    rootFileServer = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)(_parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? Enum_RutasArchivos.ExcelLogoLocal : Enum_RutasArchivos.ExcelLogo)),
                    TypeMime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    TituloReporteExcel = "Listado de Ordenes Area Central",
                    NombreArchivo = string.Concat(Funciones.GetCodigoUnico("Op360Gos"), ".xlsx")
                };
                var listTest = await _op360GosWebService.ConsultarOrdenesAreaCentralGosExcel(parameters);
                return await _unitOfWork.ExcelGenerador<ResponseDto<FileResponseOracleBase64>>().ExecuteExcelGenerador(listTest.Datos.orden.AsQueryable(), format, ConfigExcel);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<ResponseDto<(FileResponseOracleBase64, byte[])>> ConsultaOrdenesGestionGosExcel(ConsultaOrdenesGestionDto parameters)
        {
            try
            {
                ConsultaOrdenesGestionSpDto consultaOrdenesGestionSpDto = new()
                {
                    filtros = parameters.filtros
                };
                QueryOp360ConfigExcel ConfigExcel = new()
                {
                    rootFileServer = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)(_parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? Enum_RutasArchivos.ExcelLogoLocal : Enum_RutasArchivos.ExcelLogo)),
                    TypeMime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    TituloReporteExcel = "Listado de Ordenes Gestion",
                    NombreArchivo = string.Concat(Funciones.GetCodigoUnico("Op360GosOrdenesGestion"), ".xlsx")
                };
                var listTest = await _op360GosWebService.ConsultaOrdenesGestionGosExcel(consultaOrdenesGestionSpDto);
                if (!listTest.Datos.ordenes.Any())
                {
                    return new ResponseDto<(FileResponseOracleBase64, byte[])>
                    {
                        Codigo = 1,
                        Mensaje = "No se encontraron registros para el reporte."
                    };
                }
                else
                {
                    return await _unitOfWork.ExcelGenerador<ResponseDto<FileResponseOracleBase64>>().ExecuteExcelGenerador(listTest.Datos.ordenes.AsQueryable(), parameters.Format, ConfigExcel);
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        #endregion
    }
}