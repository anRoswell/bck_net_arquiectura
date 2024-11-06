namespace Api.Controllers
{
    using Api.Responses;
    using AutoMapper;
    using Core.Dtos.FiltrosDto;
    using Core.DTOs;
    using Core.DTOs.FilesDto;
    using Core.DTOs.FiltrosDto;
    using Core.DTOs.Gos.CargarOrdenesAreaCentral;
    using Core.DTOs.ObtenerReporteTraza;
    using Core.Entities;
    using Core.Enumerations;
    using Core.Exceptions;
    using Core.Interfaces;
    using Core.QueryFilters;
    using Infrastructure.Interfaces;
    using Infrastructure.Validators;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class Op360ScrWebController : ControllerBase
    {
        private readonly IOp360Service _op360Service;
        private readonly IOp360ReportingService _op360ReportingService;
        private readonly IMapper _mapper;
        private readonly IFilesProcess _filesProcess;
        private readonly IConfiguration _configuration;
        private readonly ICargaMasivaFactory _cargaMasivaFactory;
        private readonly IWebHostEnvironment _environment;

        public Op360ScrWebController(
            IOp360Service op360Service,
            IOp360ReportingService op360reportingService,
            IMapper mapper,
            IFilesProcess filesProcess,
            IConfiguration configuration,
            ICargaMasivaFactory cargaMasivaFactory,
            IWebHostEnvironment environment
            )
        {
            _op360Service = op360Service;
            _op360ReportingService = op360reportingService;
            _mapper = mapper;
            _filesProcess = filesProcess;
            _configuration = configuration;
            _cargaMasivaFactory = cargaMasivaFactory;
            _environment = environment;
        }

        #region Ordenes
        /// <summary>
        /// Retorna las ordenes de trabajo para el perfil de oficina central        
        /// Carlos Vargas
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>     
        //[Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorial")]
        [Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorialReadOnly2")]
        [HttpGet("ObtenerOrdenesTrabajoOficinaCentral", Name = "GetAllOrdenesTrabajoOficinaCentral")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetAllOrdenesTrabajoOficinaCentral([FromQuery] QueryOp360Ordenes parameters)
        {
            try
            {
                var listTest = await _op360Service.Consultar_Ordenes_Area_Central(parameters);
                Aire_Scr_OrdenAreaCentral_ResponseDto aire_Scr_Orden_ResponseDto = new()
                {
                    ordenes = listTest.Datos?.ordenes ?? null,
                    grafica_asignacion = listTest.Datos?.grafica_asignacion ?? null,
                    ArrayIdOrdenesFiltradas = listTest.Datos?.ArrayIdOrdenesFiltradas ?? null
                };
                var response = new ApiResponse<Aire_Scr_OrdenAreaCentral_ResponseDto>(aire_Scr_Orden_ResponseDto, listTest.Codigo == 0 ? 200 : 400, listTest.Mensaje)
                {
                    TotalRecords = listTest?.Datos?.RegistrosTotales ?? 0,
                    Meta = new Metadata2()
                    {
                        RegistrosTotales = listTest?.Datos?.RegistrosTotales ?? 0,
                        NoPagina = parameters.ServerSideJson?.first ?? 0,
                        RegistrosPorPagina = parameters.ServerSideJson?.rows ?? 0
                    }
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        [AllowAnonymous]
        [HttpGet("PruebaServerSide", Name = "PruebaServerSide")]
        [Consumes("application/json")]
        public async Task<IActionResult> PruebaServerSide([FromQuery] QueryOp360ServerSideTest parameters)
        {
            try
            {
                var listTest = await _op360Service.ServerSideTest(parameters);                
                var response = new ApiResponse<Op360ServerSideTestDto>(listTest.Datos, listTest.Codigo == 0 ? 200 : 400, listTest.Mensaje)
                {
                    TotalRecords = listTest?.Datos?.RegistrosTotales ?? 0,
                    Meta = new Metadata2()
                    {
                        RegistrosTotales = listTest?.Datos?.RegistrosTotales ?? 0,
                        NoPagina = parameters.ServerSideJson?.first ?? 0,
                        RegistrosPorPagina = parameters.ServerSideJson?.rows ?? 0
                    }
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        [AllowAnonymous]
        [HttpGet("PruebaServerSideExcel/{format}", Name = "PruebaServerSideExcel/{format}")]
        [Consumes("application/json")]
        public async Task<IActionResult> PruebaServerSideExcel([FromQuery] QueryOp360ServerSideTest parameters, string format)
        {
            try
            {
                var streamresponse = await _op360ReportingService.PruebaServerSideExcel(parameters, format);
                if (format == "base64" && streamresponse.Codigo == 200)
                {
                    return Ok(new ApiResponse<FileResponseOracleBase64>(streamresponse.Datos.Item1, streamresponse.Codigo, streamresponse.Mensaje, streamresponse.TotalRecords));
                }
                else if (format == "bytes" && streamresponse.Codigo == 200)
                {
                    return File(streamresponse.Datos.Item2, streamresponse.Datos.Item1.TypeMime, streamresponse.Datos.Item1.NombreArchivo);
                }
                else
                {
                    return BadRequest(new ApiResponse<FileResponseOracleBase64>(new FileResponseOracleBase64() { }, 400, !new[] { "base64", "bytes" }.Contains(format) ? "formato inexistente" : streamresponse.Mensaje));
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la descarga del excel. Detalle: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Retorna las ordenes de trabajo y las exporta a excel        
        /// Carlos Vargas
        /// </summary>        
        /// <returns></returns>        
        //[Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorial")]
        [Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorialReadOnly2")]
        [HttpGet("ExportarOrdenesTrabajoOficinaCentralExcel/{format}", Name = "GetAllOrdenesTrabajoOficinaCentralExcel/{format}")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetAllOrdenesTrabajoOficinaCentralExcel([FromQuery] QueryOp360Ordenes parameters, string format)
        {
            try
            {
                var streamresponse = await _op360ReportingService.Consultar_Ordenes_Area_Central_Excel(parameters, format);
                if (format == "base64" && streamresponse.Codigo == 200)
                {
                    return Ok(new ApiResponse<FileResponseOracleBase64>(streamresponse.Datos.Item1, streamresponse.Codigo, streamresponse.Mensaje, streamresponse.TotalRecords));
                }
                else if (format == "bytes" && streamresponse.Codigo == 200)
                {
                    return File(streamresponse.Datos.Item2, streamresponse.Datos.Item1.TypeMime, streamresponse.Datos.Item1.NombreArchivo);
                }
                else
                {
                    return BadRequest(new ApiResponse<FileResponseOracleBase64>(new FileResponseOracleBase64() { }, 400, !new[] { "base64", "bytes" }.Contains(format) ? "formato inexistente" : streamresponse.Mensaje));
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la descarga del excel. Detalle: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        //[Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorial")]
        [Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorialReadOnly2")]
        [HttpGet("ObtenerReporteEjecutados", Name = "GetReporteEjecutadosScr")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetReporteEjecutadosScr([FromQuery] QueryOp360ReporteEjecutados parameters)
        {
            try
            {
                var listTest = await _op360Service.Consultar_Reporte_Ejecutados(parameters);
                var response = new ApiResponse<Aire_Scr_ReporteEjectuados_Response>(listTest.Datos, listTest.Codigo == 0 ? 200 : 400, listTest.Mensaje)
                {
                    TotalRecords = listTest?.Datos?.RegistrosTotales ?? 0,
                    Meta = new Metadata2()
                    {
                        RegistrosTotales = listTest?.Datos?.RegistrosTotales ?? 0,
                        NoPagina = parameters.ServerSideJson?.first ?? 0,
                        RegistrosPorPagina = parameters.ServerSideJson?.rows ?? 0
                    }
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        //[Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorial")]
        [Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorialReadOnly2")]
        [HttpGet("ObtenerReporteEjecutadosExcel/{format}", Name = "GetReporteEjecutadosScrExcel/{format}")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetReporteEjecutadosScrExcel([FromQuery] QueryOp360ReporteEjecutados parameters, string format)
        {
            try
            {
                var streamresponse = await _op360ReportingService.ConsultarReporteEjecutadosScrExcel(parameters, format);
                if (format == "base64" && streamresponse.Codigo == 200)
                {
                    return Ok(new ApiResponse<FileResponseOracleBase64>(streamresponse.Datos.Item1, streamresponse.Codigo, streamresponse.Mensaje, streamresponse.TotalRecords));
                }
                else if (format == "bytes" && streamresponse.Codigo == 200)
                {
                    return File(streamresponse.Datos.Item2, streamresponse.Datos.Item1.TypeMime, streamresponse.Datos.Item1.NombreArchivo);
                }
                else
                {
                    return BadRequest(new ApiResponse<FileResponseOracleBase64>(new FileResponseOracleBase64() { }, 400, !new[] { "base64", "bytes" }.Contains(format) ? "formato inexistente" : streamresponse.Mensaje));
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la descarga del excel. Detalle: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Retorna los parametros iniciales para el modulo de ordenes en area central        
        /// Carlos Vargas
        /// </summary>
        /// <returns></returns>        
        //[Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorial")]
        [Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorialReadOnly2")]
        [HttpGet("GetParametrosInicialesAreaCentral", Name = "ParametrosInicialesAreaCentral")]
        [Consumes("application/json")]
        public async Task<IActionResult> ParametrosInicialesAreaCentral()
        {
            try
            {
                var listTest = await _op360Service.GetAireScrOrdParametrosIniciales();
                var response = new ApiResponseUrlPlantilla<Op360ParametrosInicialesAreaCentralDto>(listTest.Datos, 200, listTest.Mensaje);
                response.Url_plantilla_Generacion_Ordenes = listTest.Datos.Url_plantilla_Generacion_Ordenes;
                response.Url_plantilla_Reasignacion_Contratistas = listTest.Datos.Url_plantilla_Reasignacion_Contratista;
                response.Url_plantilla_Legalizacion_Orden = listTest.Datos.Url_plantilla_Legalizacion_Orden;
                response.Url_plantilla_Asignacion_Tecnico = listTest.Datos.Url_plantilla_Asignacion_Tecnico;
                response.Url_plantilla_DesAsignacion_Tecnico = listTest.Datos.Url_plantilla_DesAsignacion_Tecnico;
                response.Url_plantilla_Reasignacion_Contratistas2 = listTest.Datos.Url_plantilla_Reasignacion_Contratista2;
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Retorna las ordenes de trabajo        
        /// Carlos Vargas
        /// </summary>        
        /// <returns></returns>        
        //[Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorial")]
        [Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorialReadOnly2")]
        [HttpGet("ObtenerOrdenesTrabajoAgrupadasOficinaCentral", Name = "GetAllOrdenesTrabajoAgrupadas")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetAllOrdenesTrabajoAgrupadas([FromQuery] QueryOp360OrdenesAgrupada parameters)
        {
            try
            {
                parameters.id_contratista = parameters.id_contratista ?? -2;
                var listTest = await _op360Service.Consultar_Ordenes_Agrupadas_Area_Central(parameters);

                var response = new ApiResponse<Aire_Scr_Orden_Agrupada_Response>(listTest.Datos, 200, listTest.Mensaje);
                response.TotalRecords = listTest?.Datos?.ordenes_agrupadas?.Length ?? 0;
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistasTerritorial")]
        [HttpGet("ObtenerReporteEjecutadosContratistas", Name = "GetReporteEjecutadosContratistasScr")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetReporteEjecutadosContratistasScr([FromQuery] QueryOp360ReporteEjecutadosContratistas parameters)
        {
            try
            {
                int.TryParse(HttpContext.Items["id_persona"]?.ToString() ?? "0", out int v_id_persona);
                int.TryParse(HttpContext.Items["id_usuario"]?.ToString() ?? "0", out int vid_usuario);
                parameters.id_persona = v_id_persona.ToString();
                parameters.id_usuario = vid_usuario.ToString();
                if (string.IsNullOrWhiteSpace(parameters.id_persona))
                {
                    return BadRequest(ErrorResponse.Op360ErrorTemplate(400, "El usuairo no tiene un id_persona", 400));
                }
                else
                {
                    var listTest = await _op360Service.Consultar_Reporte_Ejecutados_Contratistas(parameters);
                    var response = new ApiResponse<Aire_Scr_ReporteEjectuados_Response>(listTest.Datos, listTest.Codigo == 0 ? 200 : 400, listTest.Mensaje)
                    {
                        TotalRecords = listTest?.Datos?.RegistrosTotales ?? 0,
                        Meta = new Metadata2()
                        {
                            RegistrosTotales = listTest?.Datos?.RegistrosTotales ?? 0,
                            NoPagina = parameters.ServerSideJson?.first ?? 0,
                            RegistrosPorPagina = parameters.ServerSideJson?.rows ?? 0
                        }
                    };
                    return Ok(response);
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistasTerritorial")]
        [HttpGet("ObtenerReporteEjecutadosContratistasExcel/{format}", Name = "GetReporteEjecutadosContratistasScrExcel/{format}")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetReporteEjecutadosContratistasScrExcel([FromQuery] QueryOp360ReporteEjecutadosContratistas parameters, string format)
        {
            try
            {
                int.TryParse(HttpContext.Items["id_persona"]?.ToString() ?? "0", out int v_id_persona);
                int.TryParse(HttpContext.Items["id_usuario"]?.ToString() ?? "0", out int vid_usuario);
                parameters.id_persona = v_id_persona.ToString();
                parameters.id_usuario = vid_usuario.ToString();
                var streamresponse = await _op360ReportingService.ConsultarReporteEjecutadosContratistasScrExcel(parameters, format);
                if (format == "base64" && streamresponse.Codigo == 200)
                {
                    return Ok(new ApiResponse<FileResponseOracleBase64>(streamresponse.Datos.Item1, streamresponse.Codigo, streamresponse.Mensaje, streamresponse.TotalRecords));
                }
                else if (format == "bytes" && streamresponse.Codigo == 200)
                {
                    return File(streamresponse.Datos.Item2, streamresponse.Datos.Item1.TypeMime, streamresponse.Datos.Item1.NombreArchivo);
                }
                else
                {
                    return BadRequest(new ApiResponse<FileResponseOracleBase64>(new FileResponseOracleBase64() { }, 400, !new[] { "base64", "bytes" }.Contains(format) ? "formato inexistente" : streamresponse.Mensaje));
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la descarga del excel. Detalle: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        [HttpGet("ObtenerResumenGlobalOrdenes", Name = "GetResumeprc_consultar_ordenes_asignadas_tecnicon_Global_Ordenes")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> GetResumen_Global_Ordenes()
        {
            try
            {
                var listTest = await _op360Service.GetResumen_Global_Ordenes();
                var response = new ApiResponse<Op360ResumenGlobalOrdenesDto>(listTest.Datos, 200, listTest.Mensaje);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }        

        /// <summary>
        /// Retorna las ordenes de trabajo para el perfil de contratistas
        /// Carlos Vargas
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "ShouldBeAnAdminContratistasTerritorial")]
        [HttpGet("ObtenerOrdenesTrabajoContratistas", Name = "GetAllOrdenesTrabajoContratistas")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetAllOrdenesTrabajoContratistas([FromQuery] QueryOp360OrdenesContratistas parameters)
        {
            try
            {
                int.TryParse(HttpContext.Items["id_persona"]?.ToString() ?? "0", out int v_id_persona);
                int.TryParse(HttpContext.Items["id_usuario"]?.ToString() ?? "0", out int vid_usuario);
                parameters.id_persona = v_id_persona.ToString();
                parameters.id_usuario = vid_usuario.ToString();
                var validator2 = new QueryOp360OrdenesContratistasValidator();
                var validationResult2 = validator2.Validate(parameters);
                if (!validationResult2.IsValid)
                {
                    return BadRequest(ErrorResponse.Op360ErrorTemplate(400, validationResult2.ToString("|"), 400));
                }
                else
                {
                    var listTest = await _op360Service.Consultar_Ordenes_Contratistas(parameters);

                    Aire_Scr_Orden_ResponseDto aire_Scr_Orden_ResponseDto = new()
                    {
                        ordenes = listTest.Datos?.ordenes ?? new Aire_Scr_Orden[] { },
                        grafica_asignacion = listTest.Datos?.grafica_asignacion ?? new Grafica_Asignacion[] { },
                        ArrayIdOrdenesFiltradas = listTest.Datos?.ArrayIdOrdenesFiltradas ?? new int[] { }
                    };
                    var response = new ApiResponse<Aire_Scr_Orden_ResponseDto>(aire_Scr_Orden_ResponseDto, listTest.Codigo == 0 ? 200 : 400, listTest.Mensaje)
                    {
                        TotalRecords = listTest?.Datos?.RegistrosTotales ?? 0,
                        Meta = new Metadata2()
                        {
                            RegistrosTotales = listTest?.Datos?.RegistrosTotales ?? 0,
                            NoPagina = parameters.ServerSideJson?.first ?? 0,
                            RegistrosPorPagina = parameters.ServerSideJson?.rows ?? 0
                        }
                    };

                    return Ok(response);
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Retorna las ordenes de trabajo para el perfil de contratistas
        /// Carlos Vargas
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "ShouldBeAnAdminContratistasTerritorial")]
        [HttpGet("ObtenerOrdenesTrabajoContratistasExcel/{format}", Name = "GetAllOrdenesTrabajoContratistasExcel/{format}")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetAllOrdenesTrabajoContratistasExcel([FromQuery] QueryOp360OrdenesContratistas parameters, string format)
        {
            try
            {
                int.TryParse(HttpContext.Items["id_persona"]?.ToString() ?? "0", out int v_id_persona);
                int.TryParse(HttpContext.Items["id_usuario"]?.ToString() ?? "0", out int vid_usuario);
                parameters.id_persona = v_id_persona.ToString();
                parameters.id_usuario = vid_usuario.ToString();
                var validator2 = new QueryOp360OrdenesContratistasValidator();
                var validationResult2 = validator2.Validate(parameters);
                if (!validationResult2.IsValid)
                {
                    return BadRequest(ErrorResponse.Op360ErrorTemplate(400, validationResult2.ToString("|"), 400));
                }
                else
                {
                    var streamresponse = await _op360ReportingService.Consultar_Ordenes_Contratistas_Excel(parameters, format);
                    if (format == "base64" && streamresponse.Codigo == 200)
                    {
                        return Ok(new ApiResponse<FileResponseOracleBase64>(streamresponse.Datos.Item1, streamresponse.Codigo, streamresponse.Mensaje, streamresponse.TotalRecords));
                    }
                    else if (format == "bytes" && streamresponse.Codigo == 200)
                    {
                        return File(streamresponse.Datos.Item2, streamresponse.Datos.Item1.TypeMime, streamresponse.Datos.Item1.NombreArchivo);
                    }
                    else
                    {
                        return BadRequest(new ApiResponse<FileResponseOracleBase64>(new FileResponseOracleBase64() { }, 400, !new[] { "base64", "bytes" }.Contains(format) ? "formato inexistente" : streamresponse.Mensaje));
                    }
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Retorna el listado de ordenes de trabajo para el dashboard de area central
        /// Carlos Vargas
        /// </summary>        
        /// <returns></returns>        
        [Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorial")]
        [HttpGet("ObtenerOrdenesDashBoardAreaCentral", Name = "GetOrdenesDashBoardAreaCentral")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetOrdenesDashBoardAreaCentral([FromQuery] QueryOp360OrdenesDashBoard parameters)
        {
            try
            {
                parameters.pageSize = parameters.pageSize ?? 100000;
                parameters.pageNumber = parameters.pageNumber ?? 0;
                parameters.sortColumn = parameters.sortColumn ?? "id_orden";
                parameters.sortDirection = parameters.sortDirection ?? "desc";

                var listTest = await _op360Service.Consultar_Ordenes_DashBoard_Area_Central(parameters);
                var response = new ApiResponse<Aire_Scr_Orden_DashBoard_AreaCentral_Response>(listTest.Datos, 200, listTest.Mensaje)
                {
                    TotalRecords = listTest?.Datos?.RegistrosTotales ?? 0,
                    Meta = new Metadata2()
                    {
                        RegistrosTotales = listTest?.Datos?.RegistrosTotales ?? 0,
                        NoPagina = parameters.pageNumber ?? 0,
                        RegistrosPorPagina = parameters.pageSize ?? 0
                    }
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Retorna el listado de ordenes de trabajo para el dashboard de contratistas
        /// Carlos Vargas
        /// </summary>        
        /// <returns></returns>        
        [Authorize(Policy = "ShouldBeAnAdminContratistasTerritorial")]
        [HttpGet("ObtenerOrdenesDashBoardContratistas", Name = "GetOrdenesDashBoardContratistas")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetOrdenesDashBoardContratistas([FromQuery] QueryOp360OrdenesDashBoardContratista parameters)
        {
            try
            {
                int.TryParse(HttpContext.Items["id_contratista"]?.ToString() ?? "0", out int vidContratista);
                parameters.id_contratista = vidContratista;
                var validator2 = new QueryOp360OrdenesDashBoardContratistaValidator();
                var validationResult2 = validator2.Validate(parameters);
                if (!validationResult2.IsValid)
                {
                    return BadRequest(ErrorResponse.Op360ErrorTemplate(400, validationResult2.ToString("|"), 400));
                }
                else
                {
                    parameters.pageSize = parameters.pageSize ?? 1000000;
                    parameters.pageNumber = parameters.pageNumber ?? 0;
                    parameters.sortColumn = parameters.sortColumn ?? "id_orden";
                    parameters.sortDirection = parameters.sortDirection ?? "desc";

                    var listTest = await _op360Service.Consultar_Ordenes_DashBoard_Contratistas(parameters);
                    var response = new ApiResponse<Aire_Scr_Orden_DashBoard_AreaCentral_Response>(listTest.Datos, 200, listTest.Mensaje)
                    {
                        TotalRecords = listTest?.Datos?.RegistrosTotales ?? 0,
                        Meta = new Metadata2()
                        {
                            RegistrosTotales = listTest?.Datos?.RegistrosTotales ?? 0,
                            NoPagina = parameters.pageNumber ?? 0,
                            RegistrosPorPagina = parameters.pageSize ?? 0
                        }
                    };

                    return Ok(response);
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// consultar orden de trabajo por Id
        /// Carlos Vargas
        /// </summary>        
        /// <returns></returns>        
        [Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorial")]
        [HttpGet("ConsultarOrdenesTrabajoxId", Name = "GetOrdenesTrabajoxId")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetOrdenesTrabajoxId([FromQuery] QueryOp360Orden parameters)
        {
            try
            {
                var listTest = await _op360Service.Consultar_Orden_Por_Id(parameters);
                var response = new ApiResponse<Aire_Scr_OrdenById_Response>(listTest.Datos, 200, listTest.Mensaje);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Retorna la lista de los exceles cargados para cargue masivo de ordenes
        /// Carlos Vargas
        /// </summary>        
        /// <returns></returns>        
        [Authorize(Policy = "ShouldBeAnAdminAreaCentralContratistasTerritorial")]
        [HttpGet("ObtenerArchivosInstancia", Name = "GetConsultarArchivosInstancia")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetConsultarArchivosInstancia([FromQuery] QueryOp360ArchivosInstancia parameters)
        {
            try
            {
                var listTest = await _op360Service.Consultar_Archivos_Instancia(parameters);
                var response = new ApiResponse<Archivos_Instancia_Response>(listTest.Datos, listTest.Codigo == 0 ? 200 : 400, listTest.Mensaje);                
                response.TotalRecords = listTest?.Datos?.RegistrosTotales ?? 0;
                response.Meta = new Metadata2()
                {
                    RegistrosTotales = listTest?.Datos?.RegistrosTotales ?? 0,
                    NoPagina = parameters.ServerSideJson?.first ?? 0,
                    RegistrosPorPagina = parameters.ServerSideJson?.rows ?? 0
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminAreaCentralContratistasTerritorial")]
        [HttpGet("ObtenerArchivosInstanciaExcel/{format}", Name = "GetConsultarArchivosInstanciaExcel/{format}")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetConsultarArchivosInstanciaExcel([FromQuery] QueryOp360ArchivosInstancia parameters, string format)
        {
            try
            {
                var streamresponse = await _op360ReportingService.Consultar_Archivos_Instancia_Excel(parameters, format);
                if (format == "base64" && streamresponse.Codigo == 200)
                {
                    return Ok(new ApiResponse<FileResponseOracleBase64>(streamresponse.Datos.Item1, streamresponse.Codigo, streamresponse.Mensaje, streamresponse.TotalRecords));
                }
                else if (format == "bytes" && streamresponse.Codigo == 200)
                {
                    return File(streamresponse.Datos.Item2, streamresponse.Datos.Item1.TypeMime, streamresponse.Datos.Item1.NombreArchivo);
                }
                else
                {
                    return BadRequest(new ApiResponse<FileResponseOracleBase64>(new FileResponseOracleBase64() { }, 400, !new[] { "base64", "bytes" }.Contains(format) ? "formato inexistente" : streamresponse.Mensaje));
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Retorna la lista de los exceles cargados para cargue masivo de ordenes
        /// Carlos Vargas
        /// </summary>        
        /// <returns></returns>        
        [Authorize(Policy = "ShouldBeAnAdminAreaCentralContratistasTerritorial")]
        [HttpGet("ObtenerArchivosInstanciaDetalle", Name = "GetConsultarArchivosInstanciaDetalle")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetConsultarArchivosInstanciaDetalle([FromQuery] QueryOp360ArchivosInstanciaDetalle parameters)
        {
            try
            {
                var listTest = await _op360Service.Consultar_Archivos_Instancia_Detalle(parameters);

                var response = new ApiResponse<Archivos_Instancia_Detalle_Response>(listTest.Datos, listTest.Codigo == 0 ? 200 : 400, listTest.Mensaje);
                response.TotalRecords = listTest?.Datos?.RegistrosTotales ?? 0;
                response.Meta = new Metadata2()
                {
                    RegistrosTotales = listTest?.Datos?.RegistrosTotales ?? 0,
                    NoPagina = parameters.ServerSideJson?.first ?? 0,
                    RegistrosPorPagina = parameters.ServerSideJson?.rows ?? 0
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminAreaCentralContratistasTerritorial")]
        [HttpGet("ObtenerArchivosInstanciaDetalleExcel/{format}", Name = "GetConsultarArchivosInstanciaDetalleExcel/{format}")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetConsultarArchivosInstanciaDetalleExcel([FromQuery] QueryOp360ArchivosInstanciaDetalle parameters, string format)
        {
            try
            {
                var streamresponse = await _op360ReportingService.Consultar_Archivos_Instancia_Detalle_Excel(parameters, format);
                if (format == "base64" && streamresponse.Codigo == 200)
                {
                    return Ok(new ApiResponse<FileResponseOracleBase64>(streamresponse.Datos.Item1, streamresponse.Codigo, streamresponse.Mensaje, streamresponse.TotalRecords));
                }
                else if (format == "bytes" && streamresponse.Codigo == 200)
                {
                    return File(streamresponse.Datos.Item2, streamresponse.Datos.Item1.TypeMime, streamresponse.Datos.Item1.NombreArchivo);
                }
                else
                {
                    return BadRequest(new ApiResponse<FileResponseOracleBase64>(new FileResponseOracleBase64() { }, 400, !new[] { "base64", "bytes" }.Contains(format) ? "formato inexistente" : streamresponse.Mensaje));
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }
        
        /// <summary>
        /// Asignar o Deasignar Aliados o Contratistas        
        /// Carlos Vargas        
        /// </summary>        
        /// <returns></returns>        
        [Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorial")]
        [HttpPost("OrdenesGestionarContratistas", Name = "GestionarOrdenesContratistas")]
        [Consumes("application/json")]
        public async Task<IActionResult> GestionarOrdenesContratistas([FromBody] QueryOp360GestionarContratistas parameters)
        {
            try
            {
                // Validar si el asignar contratista es true y el id_contratista es 0 retornar error.
                if (parameters.Asignar_Contratista && string.IsNullOrWhiteSpace(parameters.Identificacion))
                {
                    return Ok(ErrorResponse.Op360ErrorTemplate(50006, "Debe seleccionar un contratista", 400));
                }

                // Si el asignar contratista es verdadero y llega un id contratista, se entiende que se va asignar un contratista.
                if (parameters.Asignar_Contratista && !string.IsNullOrWhiteSpace(parameters.Identificacion))
                {
                    var listTest = await _op360Service.Gestionar_Ordenes_Contratista_Asignar(parameters);
                    var response = new ApiResponse<ResponseDto>(listTest, 200, listTest.Mensaje);
                    if (listTest.Codigo == 200)
                    {
                        return Ok(response);
                    }
                    else
                    {
                        return BadRequest(ErrorResponse.Op360ErrorTemplate(listTest.Codigo, listTest.Mensaje, listTest.Codigo));
                    }
                }
                else if (!parameters.Asignar_Contratista)
                {
                    var listTest = await _op360Service.Gestionar_Ordenes_Contratista_DesAsignar(parameters);
                    var response = new ApiResponse<ResponseDto>(listTest, 200, listTest.Mensaje);
                    return Ok(response);
                }
                else
                {
                    return BadRequest(ErrorResponse.Op360ErrorTemplate(50007, "No se realizo ninguna acción", 400));

                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Asignar o Deasignar Brigadas o Técnicos        
        /// Carlos Vargas        
        /// </summary>        
        /// <returns></returns>        
        [Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorial")]
        [HttpPost("OrdenesGestionarBrigadas", Name = "GestionarOrdenesBrigadas")]
        [Consumes("application/json")]
        public async Task<IActionResult> GestionarOrdenesBrigadas([FromBody] QueryOp360GestionarBrigadas parameters)
        {
            try
            {
                //Se toma la identificacion de la sesión ya que el usuario es un contratista que debe estar logueado.
                //int.TryParse(HttpContext.Items["id_contratista"]?.ToString() ?? "0", out int vidContratista);
                int.TryParse(HttpContext.Items["id_persona"]?.ToString() ?? "0", out int vid_persona);
                long.TryParse(HttpContext.Items["identificacion"]?.ToString() ?? "0", out long videntificacion);
                
                // Si el asignar contratista es verdadero y llega un id contratista, se entiende que se va asignar un contratista.
                if (parameters.Asignar_Brigada)
                {
                    var listTest = await _op360Service.Gestionar_Ordenes_Brigada_Asignar(parameters);
                    var response = new ApiResponse<ResponseDto>(listTest, 200, listTest.Mensaje);
                    if (listTest.Codigo == 200)
                    {
                        return Ok(response);
                    }
                    else
                    {
                        return BadRequest(ErrorResponse.Op360ErrorTemplate(50033, listTest.Mensaje, listTest.Codigo));
                    }
                }
                else if (!parameters.Asignar_Brigada)
                {
                    parameters.Identificacion_Brigada = videntificacion.ToString();
                    var listTest = await _op360Service.Gestionar_Ordenes_Brigada_DesAsignar(parameters);
                    var response = new ApiResponse<ResponseDto>(listTest, 200, listTest.Mensaje);
                    return Ok(response);
                }
                else
                {
                    return BadRequest(ErrorResponse.Op360ErrorTemplate(50033, "No se realizo ninguna acción", 400));
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Retorna los parametros iniciales para el modulo de ordenes Contratistas
        /// Carlos Vargas
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "ShouldBeAnAdminContratistasTerritorial")]
        [HttpGet("GetParametrosInicialesContratistas", Name = "ParametrosInicialesContratistas")]
        [Consumes("application/json")]
        public async Task<IActionResult> ParametrosInicialesContratistas()
        {
            try
            {
                //Se toma la identificacion de la sesión ya que el usuario es un contratista que debe estar logueado.
                int.TryParse(HttpContext.Items["id_persona"]?.ToString() ?? "0", out int vid_persona);
                int.TryParse(HttpContext.Items["id_usuario"]?.ToString() ?? "0", out int vid_usuario);
                if (vid_persona == 0)
                {
                    Op360ParametrosInicialesContratistaDto tmp = new();
                    var response2 = new ApiResponse<Op360ParametrosInicialesContratistaDto>(tmp, 400, "El usuario no tiene asignado un id de persona.");
                    return BadRequest(response2);
                }

                QueryOp360ObtenerBrigadas parameters = new QueryOp360ObtenerBrigadas()
                {
                    id_persona = vid_persona.ToString(),
                    id_usuario = vid_usuario.ToString()
                };
                var listTest = await _op360Service.GetParametrosInicialesContratistas(parameters);
                var response = new ApiResponseUrlPlantillaContratista<Op360ParametrosInicialesContratistaDto>(listTest.Datos, 200, listTest.Mensaje);
                response.TotalRecords = listTest?.Datos?.Brigadas?.Length ?? 0;
                response.Url_plantilla_Asignacion_Tecnico = listTest.Datos.Url_plantilla_Asignacion_Tecnico;
                response.Url_plantilla_DesAsignacion_Tecnico = listTest.Datos.Url_plantilla_DesAsignacion_Tecnico;
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Retorna la info del cliente por el nic o el nis        
        /// Carlos Vargas
        /// </summary>        
        /// <returns></returns>        
        [Authorize(Policy = "ShouldBeAnAdminAreaCentralReadOnly")]
        [HttpGet("GetClienteByNisNic", Name = "ClienteByNisNic")]
        [Consumes("application/json")]
        public async Task<IActionResult> ClienteByNisNic([FromQuery] QueryOp360NicNis parameters)
        {
            try
            {
                var listTest = await _op360Service.ConsultarClienteByNicNis(parameters);
                var response = new ApiResponse<Aire_ScrOrdConsultaClienteNicNis>(listTest.Datos, 200, listTest.Mensaje);

                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Retorna las ordenes de trabajo        
        /// Carlos Vargas
        /// </summary>        
        /// <returns></returns>        
        [Authorize(Policy = "ShouldBeAnAdminAreaCentral")]
        [HttpPost("CrearOrden", Name = "ShouldBeAnAdminAreaCentral")]
        [Consumes("application/json")]
        public async Task<IActionResult> GuardarOrden([FromBody] QueryOp360RegistrarOrden parameters)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(parameters.fileBase64))
                {
                    int.TryParse(HttpContext.Items["id_usuario"]?.ToString() ?? "0", out int myUser);
                    var cargaMasiva = _cargaMasivaFactory.Crear(CargaInicial.Scr);
                    var Id_Ruta = _environment.EnvironmentName.ToString() == ApiEnvironments.DevLocal ? Enum_RutasArchivos.ExcelScrMasivOrdBitesLocal : Enum_RutasArchivos.ExcelScrMasivOrdBites;
                    var data = await cargaMasiva.Procesar(parameters, myUser);
                    var statusCode = data.Codigo == 0 ? 200 : 400;
                    var response = new ApiResponse<ResponseDto<Core.DTOs.CargaMasiva.MedirTiempo>>(data, statusCode, data.Mensaje);
                    return statusCode == 200 ? Ok(response) : BadRequest(response);
                }
                else if (parameters.formdata != null && string.IsNullOrWhiteSpace(parameters.fileBase64))
                {
                    var listTest = await _op360Service.Guardar_Orden_Formdata(parameters.formdata);
                    FormdataResponse rst = new()
                    {
                        id_orden = listTest.Codigo
                    };
                    var response = new ApiResponse<FormdataResponse>(rst, 200, listTest.Mensaje);
                    return Ok(response);
                }
                else
                {
                    var response = new ApiResponse<FormdataResponse>(new FormdataResponse(), 400, "No se detectaron archivos o un registro para crear");
                    return BadRequest(response);
                }

            }
            catch (Exception e)
            {
                return BadRequest(ErrorResponse.Op360ErrorTemplate(0, $"Error No Controlado: {e.Message}, {e.InnerException?.Message ?? ""}", 400));
            }
        }

        /// <summary>
        /// Consultar la Trazabilidad de una Orden o de varias órdenes
        /// Carlos Vargas
        /// </summary>        
        /// <returns></returns>        
        [Authorize(Policy = "ShouldBeAnAdminAreaCentralContratistasTerritorial")]
        [HttpGet("ConsultarTrazabilidadOrdenes", Name = "Consultar_Trazabilidad_Ordenes")]
        [Consumes("application/json")]
        public async Task<IActionResult> Consultar_Trazabilidad_Ordenes([FromQuery] QueryOp360GetOrdenes parameters)
        {
            try
            {
                var listTest = await _op360Service.GetConsultar_Trazabilidad_Ordenes(parameters);
                var response = new ApiResponse<IList<Op360Ordenes_TrazabilidadDto>>(listTest.Datos, 200, listTest.Mensaje);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Consultar la georeferenciacion por area central
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorial")]
        [HttpGet("ConsultarGeorreferenciaAreaCentral", Name = "Consultar_Georreferencia_AreaCentral")]
        [Consumes("application/json")]
        public async Task<IActionResult> Consultar_Georreferencia_AreaCentral([FromQuery] QueryOp360GetFechaGeoreferenciacion parameters)
        {
            try
            {
                var listTest = await _op360Service.GetConsultar_Georreferencia_AreaCentral(parameters);
                var response = new ApiResponse<IList<Op360GeoreferenciacionFechaDto>>(listTest.Datos, 200, listTest.Mensaje);
                response.TotalRecords = listTest?.Datos?.Count ?? 0;
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Consultar la georeferenciacion por area central
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [Authorize(Policy = "ShouldBeAnAdminContratistasTerritorial")]
        [HttpGet("ConsultarGeorreferenciaContratistaPersona", Name = "Consultar_Georreferencia_ContratistaPersona")]
        [Consumes("application/json")]
        public async Task<IActionResult> Consultar_Georreferencia_ContratistaPersona([FromQuery] QueryOp360GetFechaGeoreferenciacionContratista parameters)
        {
            try
            {
                int.TryParse(HttpContext.Items["id_contratista"]?.ToString() ?? "0", out int vidContratista);
                parameters.id_contratista = vidContratista;
                var validator = new QueryOp360GetFechaGeoreferenciacionContratistaValidator();
                var validationResult = validator.Validate(parameters);
                if (!validationResult.IsValid)
                {
                    return BadRequest(ErrorResponse.Op360ErrorTemplate(400, validationResult.ToString("|"), 400));
                }
                else
                {
                    var listTest = await _op360Service.GetConsultar_Georreferencia_ContratistaPersona(parameters);
                    var response = new ApiResponse<IList<Op360GeoreferenciacionFechaDto>>(listTest.Datos, 200, listTest.Mensaje);
                    response.TotalRecords = listTest?.Datos?.Count ?? 0;

                    return Ok(response);
                }

            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Cerrar Ordenes Manualmente 
        /// Carlos Vargas        
        /// </summary>        
        /// <returns></returns>        
        [Authorize(Policy = "ShouldBeAnAdminAreaCentral")]
        [HttpPost("CerrarOrden", Name = "CerrarOrdenesManualmente")]
        [Consumes("application/json")]
        public async Task<IActionResult> CerrarOrdenesManualmente([FromBody] QueryOp360CerrarOrdenes parameters)
        {
            try
            {
                int.TryParse(HttpContext.Items["id_usuario"]?.ToString() ?? "0", out int vidUsuario);
                parameters.id_usuario_cierre = vidUsuario;
                if (parameters.id_usuario_cierre == 0)
                {
                    return BadRequest(ErrorResponse.Op360ErrorTemplate(400, "No se detecto un usuario logueado en el sistema", 400));
                }
                else
                {
                    // Validar si el asignar contratista es true y el id_contratista es 0 retornar error.
                    var listTest = await _op360Service.CerrarOrdenesManualmente(parameters);
                    var response = new ApiResponse<ResponseDto>(listTest, listTest.Codigo == 0 ? 200 : 400, listTest.Mensaje);
                    return Ok(response);
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Metodo: Carga masiva de ordenes cerradas      
        /// Desarrollado por: Cenhawer Rodriguez
        /// </summary>        
        /// <returns></returns>        
        [Authorize(Policy = "ShouldBeAnAdminAreaCentral")]
        [HttpPost("MasivoCerrarOrdenesSCR", Name = "MasivoCerrarOrdenesSCR")]
        [Consumes("application/json")]
        public async Task<IActionResult> MasivoCerrarOrdenesSCR([FromBody] CargarOrdenesAreaCentralRequestDto parameters)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(parameters.FileBase64))
                {
                    int.TryParse(HttpContext.Items["id_usuario"]?.ToString() ?? "0", out int myUser);
                    var cargaMasiva = _cargaMasivaFactory.Crear(CargaInicial.ScrCerrarOrden);                    
                    var data = await cargaMasiva.Procesar(parameters, myUser);
                    var statusCode = data.Codigo == 0 ? 200 : 400;
                    var response = new ApiResponse<ResponseDto<Core.DTOs.CargaMasiva.MedirTiempo>>(data, statusCode, data.Mensaje);
                    return statusCode == 200 ? Ok(response) : BadRequest(response);
                }
                else
                {
                    var response = new ApiResponse<FormdataResponse>(new FormdataResponse(), 400, "No se detectaron archivos o un registro para crear");
                    return BadRequest(response);
                }

            }
            catch (Exception e)
            {
                return BadRequest(ErrorResponse.Op360ErrorTemplate(0, $"Error No Controlado: {e.Message}, {e.InnerException?.Message ?? ""}", 400));
            }
        }
               
        [Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorial")]
        [HttpPost("MasivoReasignarOrdenesSCR", Name = "MasivoReasignarOrdenesSCR")]
        [Consumes("application/json")]
        public async Task<IActionResult> MasivoReasignarOrdenesSCR([FromBody] CargarOrdenesAreaCentralRequestDto parameters)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(parameters.FileBase64))
                {
                    int.TryParse(HttpContext.Items["id_usuario"]?.ToString() ?? "0", out int myUser);
                    var cargaMasiva = _cargaMasivaFactory.Crear(CargaInicial.ScrReasigOrden);
                    var Id_Ruta = _environment.EnvironmentName.ToString() == ApiEnvironments.DevLocal ? Enum_RutasArchivos.ExcelScrMasivOrdBitesLocal : Enum_RutasArchivos.ExcelScrMasivOrdBites;
                    var data = await cargaMasiva.Procesar(parameters, myUser);
                    var statusCode = data.Codigo == 0 ? 200 : 400;
                    var response = new ApiResponse<ResponseDto<Core.DTOs.CargaMasiva.MedirTiempo>>(data, statusCode, data.Mensaje);
                    return statusCode == 200 ? Ok(response) : BadRequest(response);
                }
                else
                {
                    var response = new ApiResponse<FormdataResponse>(new FormdataResponse(), 400, "No se detectaron archivos o un registro para crear");
                    return BadRequest(response);
                }

            }
            catch (Exception e)
            {
                return BadRequest(ErrorResponse.Op360ErrorTemplate(0, $"Error No Controlado: {e.Message}, {e.InnerException?.Message ?? ""}", 400));
            }
        }

        /// <summary>
        /// Exclusivo para el rol territorial. no se carga el tipo de suspencion.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorial")]
        [HttpPost("MasivoReasignarOrdenesSCR2", Name = "MasivoReasignarOrdenesSCR2")]
        [Consumes("application/json")]
        public async Task<IActionResult> MasivoReasignarOrdenesSCR2([FromBody] CargarOrdenesAreaCentralRequestDto parameters)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(parameters.FileBase64))
                {
                    int.TryParse(HttpContext.Items["id_usuario"]?.ToString() ?? "0", out int myUser);
                    var cargaMasiva = _cargaMasivaFactory.Crear(CargaInicial.ScrReasigOrden2);
                    var Id_Ruta = _environment.EnvironmentName.ToString() == ApiEnvironments.DevLocal ? Enum_RutasArchivos.ExcelScrMasivOrdBitesLocal : Enum_RutasArchivos.ExcelScrMasivOrdBites;
                    var data = await cargaMasiva.Procesar(parameters, myUser);
                    var statusCode = data.Codigo == 0 ? 200 : 400;
                    var response = new ApiResponse<ResponseDto<Core.DTOs.CargaMasiva.MedirTiempo>>(data, statusCode, data.Mensaje);
                    return statusCode == 200 ? Ok(response) : BadRequest(response);
                }
                else
                {
                    var response = new ApiResponse<FormdataResponse>(new FormdataResponse(), 400, "No se detectaron archivos o un registro para crear");
                    return BadRequest(response);
                }

            }
            catch (Exception e)
            {
                return BadRequest(ErrorResponse.Op360ErrorTemplate(0, $"Error No Controlado: {e.Message}, {e.InnerException?.Message ?? ""}", 400));
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorial")]
        [HttpPost("MasivoAsignarTecnicosSCR", Name = "MasivoAsignarTecnicosSCR")]
        [Consumes("application/json")]
        public async Task<IActionResult> MasivoAsignarTecnicosSCR([FromBody] CargarOrdenesAreaCentralRequestDto parameters)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(parameters.FileBase64))
                {
                    int.TryParse(HttpContext.Items["id_usuario"]?.ToString() ?? "0", out int myUser);
                    var cargaMasiva = _cargaMasivaFactory.Crear(CargaInicial.ScrAsigTecnico);
                    var Id_Ruta = _environment.EnvironmentName.ToString() == ApiEnvironments.DevLocal ? Enum_RutasArchivos.ExcelScrMasivOrdBitesLocal : Enum_RutasArchivos.ExcelScrMasivOrdBites;
                    var data = await cargaMasiva.Procesar(parameters, myUser);
                    var statusCode = data.Codigo == 0 ? 200 : 400;
                    var response = new ApiResponse<ResponseDto<Core.DTOs.CargaMasiva.MedirTiempo>>(data, statusCode, data.Mensaje);
                    return statusCode == 200 ? Ok(response) : BadRequest(response);
                }
                else
                {
                    var response = new ApiResponse<FormdataResponse>(new FormdataResponse(), 400, "No se detectaron archivos o un registro para crear");
                    return BadRequest(response);
                }

            }
            catch (Exception e)
            {
                return BadRequest(ErrorResponse.Op360ErrorTemplate(0, $"Error No Controlado: {e.Message}, {e.InnerException?.Message ?? ""}", 400));
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorial")]
        [HttpPost("MasivoDesAsignarTecnicosSCR", Name = "MasivoDesAsignarTecnicosSCR")]
        [Consumes("application/json")]
        public async Task<IActionResult> MasivoDesAsignarTecnicosSCR([FromBody] CargarOrdenesAreaCentralRequestDto parameters)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(parameters.FileBase64))
                {
                    int.TryParse(HttpContext.Items["id_usuario"]?.ToString() ?? "0", out int myUser);
                    var cargaMasiva = _cargaMasivaFactory.Crear(CargaInicial.ScrDesasigTecnico);
                    var Id_Ruta = _environment.EnvironmentName.ToString() == ApiEnvironments.DevLocal ? Enum_RutasArchivos.ExcelScrMasivOrdBitesLocal : Enum_RutasArchivos.ExcelScrMasivOrdBites;
                    var data = await cargaMasiva.Procesar(parameters, myUser);
                    var statusCode = data.Codigo == 0 ? 200 : 400;
                    var response = new ApiResponse<ResponseDto<Core.DTOs.CargaMasiva.MedirTiempo>>(data, statusCode, data.Mensaje);
                    return statusCode == 200 ? Ok(response) : BadRequest(response);
                }
                else
                {
                    var response = new ApiResponse<FormdataResponse>(new FormdataResponse(), 400, "No se detectaron archivos o un registro para crear");
                    return BadRequest(response);
                }

            }
            catch (Exception e)
            {
                return BadRequest(ErrorResponse.Op360ErrorTemplate(0, $"Error No Controlado: {e.Message}, {e.InnerException?.Message ?? ""}", 400));
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorial")]
        [HttpPost("DescomprometerOrdenes", Name = "DescomprometerOrdenes")]
        [Consumes("application/json")]
        public async Task<IActionResult> DescomprometerOrdenes([FromBody] QueryOp360DescomprometerOrdenes parameters)
        {
            try
            {
                // Validar si el asignar contratista es true y el id_contratista es 0 retornar error.
                var listTest = await _op360Service.DescomprometerOrdenesManualmente(parameters);
                var response = new ApiResponse<ResponseDto>(listTest, listTest.Codigo == 0 ? 200 : 400, listTest.Mensaje);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorialReadOnly2")]
        [HttpGet("ObtenerReporteLogLegalizacionAreaCentral", Name = "GetReporteLogLegalizacionScrAreaCentral")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetReporteLogLegalizacionScrAreaCentral([FromQuery] QueryOp360LogLegalizacion parameters)
        {
            try
            {
                var listTest = await _op360Service.ObtenerReporteLogLegalizacion(parameters);
                var response = new ApiResponse<ObtenerReporteLogLegalizacionDto>(listTest.Datos, listTest.Codigo == 0 ? 200 : 400, listTest.Mensaje);
                response.TotalRecords = listTest?.Datos?.RegistrosTotales ?? 0;
                response.Meta = new Metadata2()
                {
                    RegistrosTotales = listTest?.Datos?.RegistrosTotales ?? 0,
                    NoPagina = parameters.ServerSideJson?.first ?? 0,
                    RegistrosPorPagina = parameters.ServerSideJson?.rows ?? 0
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorialReadOnly2")]
        [HttpGet("ObtenerReporteLogLegalizacionAreaCentralExcel/{format}", Name = "GetReporteLogLegalizacionScrAreaCentralExcel/{format}")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetReporteLogLegalizacionScrAreaCentralExcel([FromQuery] QueryOp360LogLegalizacion parameters, string format)
        {
            try
            {
                var streamresponse = await _op360ReportingService.Consultar_Log_LegalizacionScr_Excel(parameters, format);
                if (format == "base64" && streamresponse.Codigo == 200)
                {
                    return Ok(new ApiResponse<FileResponseOracleBase64>(streamresponse.Datos.Item1, streamresponse.Codigo, streamresponse.Mensaje, streamresponse.TotalRecords));
                }
                else if (format == "bytes" && streamresponse.Codigo == 200)
                {
                    return File(streamresponse.Datos.Item2, streamresponse.Datos.Item1.TypeMime, streamresponse.Datos.Item1.NombreArchivo);
                }
                else
                {
                    return BadRequest(new ApiResponse<FileResponseOracleBase64>(new FileResponseOracleBase64() { }, 400, !new[] { "base64", "bytes" }.Contains(format) ? "formato inexistente" : streamresponse.Mensaje));
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminAreaCentralContratistas")]
        [HttpGet("ObtenerReporteLogLegalizacionContratista", Name = "GetReporteLogLegalizacionScrContratista")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetReporteLogLegalizacionScrContratista([FromQuery] QueryOp360LogLegalizacionContratistas parameters)
        {
            try
            {
                int.TryParse(HttpContext.Items["id_persona"]?.ToString() ?? "0", out int v_id_persona);
                int.TryParse(HttpContext.Items["id_usuario"]?.ToString() ?? "0", out int vid_usuario);
                parameters.id_persona = v_id_persona.ToString();
                parameters.id_usuario = vid_usuario.ToString();
                var listTest = await _op360Service.ObtenerReporteLogLegalizacioContratista(parameters);
                var response = new ApiResponse<ObtenerReporteLogLegalizacionDto>(listTest.Datos, listTest.Codigo == 0 ? 200 : 400, listTest.Mensaje);
                response.TotalRecords = listTest?.Datos?.RegistrosTotales ?? 0;
                response.Meta = new Metadata2()
                {
                    RegistrosTotales = listTest?.Datos?.RegistrosTotales ?? 0,
                    NoPagina = parameters.ServerSideJson?.first ?? 0,
                    RegistrosPorPagina = parameters.ServerSideJson?.rows ?? 0
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminAreaCentralContratistas")]
        [HttpGet("ObtenerReporteLogLegalizacionContratistaExcel/{format}", Name = "GetReporteLogLegalizacionScrContratistaExcel/{format}")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetReporteLogLegalizacionScrContratistaExcel([FromQuery] QueryOp360LogLegalizacionContratistas parameters, string format)
        {
            try
            {
                int.TryParse(HttpContext.Items["id_persona"]?.ToString() ?? "0", out int v_id_persona);
                int.TryParse(HttpContext.Items["id_usuario"]?.ToString() ?? "0", out int vid_usuario);
                parameters.id_persona = v_id_persona.ToString();
                parameters.id_usuario = vid_usuario.ToString();
                if (string.IsNullOrWhiteSpace(parameters.id_persona))
                {
                    return BadRequest(ErrorResponse.Op360ErrorTemplate(400, "El usuairo no tiene un id_persona", 400));
                }
                else
                {
                    var streamresponse = await _op360ReportingService.Consultar_Log_LegalizacionScr_Excel_Contratista(parameters, format);
                    if (format == "base64" && streamresponse.Codigo == 200)
                    {
                        return Ok(new ApiResponse<FileResponseOracleBase64>(streamresponse.Datos.Item1, streamresponse.Codigo, streamresponse.Mensaje, streamresponse.TotalRecords));
                    }
                    else if (format == "bytes" && streamresponse.Codigo == 200)
                    {
                        return File(streamresponse.Datos.Item2, streamresponse.Datos.Item1.TypeMime, streamresponse.Datos.Item1.NombreArchivo);
                    }
                    else
                    {
                        return BadRequest(new ApiResponse<FileResponseOracleBase64>(new FileResponseOracleBase64() { }, 400, !new[] { "base64", "bytes" }.Contains(format) ? "formato inexistente" : streamresponse.Mensaje));
                    }
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Obtiene los registros de la tabla ordenes historial.
        /// </summary>
        /// <param name="ordenesRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorialReadOnly2")]
        [HttpGet("ObtenerReporteTrazaAreaCentral", Name = "GetReporteTrazaScrAreaCentral")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetReporteTrazaScrAreaCentral([FromQuery] OrdenesRequestDto ordenesRequestDto)
        {
            try
            {
                var listTest = await _op360Service.GetReporteTrazaAreaCentralAsync(ordenesRequestDto);
                var response = new ApiResponse<ObtenerReporteTrazaDto>(listTest.Datos, listTest.Codigo == 0 ? ResponseHttp.Ok : ResponseHttp.Error, listTest.Mensaje);
                response.TotalRecords = listTest?.Datos?.RegistrosTotales ?? 0;
                response.Meta = new Metadata2()
                {
                    RegistrosTotales = listTest?.Datos?.RegistrosTotales ?? 0,
                    NoPagina = ordenesRequestDto.ServerSideJson?.first ?? 0,
                    RegistrosPorPagina = ordenesRequestDto.ServerSideJson?.rows ?? 0
                };
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorialReadOnly2")]
        [HttpGet("ObtenerReporteTrazaAreaCentralExcel/{format}", Name = "GetReporteTrazaScrAreaCentralExcel/{format}")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetReporteTrazaScrAreaCentralExcel([FromQuery] OrdenesRequestDto parameters, string format)
        {
            try
            {
                var streamresponse = await _op360ReportingService.Consultar_Reporte_TrazaScr_AreaCentral_Excel(parameters, format);
                if (format == "base64" && streamresponse.Codigo == (int)ResponseHttp.Ok)
                {
                    return Ok(new ApiResponse<FileResponseOracleBase64>(streamresponse.Datos.Item1, streamresponse.Codigo, streamresponse.Mensaje, streamresponse.TotalRecords));
                }
                else if (format == "bytes" && streamresponse.Codigo == (int)ResponseHttp.Ok)
                {
                    return File(streamresponse.Datos.Item2, streamresponse.Datos.Item1.TypeMime, streamresponse.Datos.Item1.NombreArchivo);
                }
                else
                {
                    return BadRequest(new ApiResponse<FileResponseOracleBase64>(new FileResponseOracleBase64() { }, 400, !new[] { "base64", "bytes" }.Contains(format) ? "formato inexistente" : streamresponse.Mensaje));
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistasTerritorial")]
        [HttpGet("ObtenerReporteTrazaScrContratistas", Name = "GetReporteTrazaScrContratistas")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetReporteTrazaScrContratistas([FromQuery] OrdenesContratistaRequestDto ordenesContratistaRequestDto)
        {
            try
            {
                int.TryParse(HttpContext.Items["id_persona"]?.ToString() ?? "0", out int v_id_persona);
                //int.TryParse(HttpContext.Items["id_usuario"]?.ToString() ?? "0", out int v_id_usuario);
                ordenesContratistaRequestDto.id_persona = v_id_persona.ToString();
                //ordenesContratistaRequestDto.id_usuario = v_id_usuario.ToString();

                if (string.IsNullOrWhiteSpace(ordenesContratistaRequestDto.id_persona))
                {
                    return BadRequest(ErrorResponse.Op360ErrorTemplate(400, "El usuairo no tiene un id_persona", 400));
                }
                else
                {
                    var listTest = await _op360Service.GetReporteTrazaContratistaAsync(ordenesContratistaRequestDto);
                    var response = new ApiResponse<ObtenerReporteTrazaDto>(listTest.Datos, listTest.Codigo == 0 ? ResponseHttp.Ok : ResponseHttp.Error, listTest.Mensaje);
                    response.TotalRecords = listTest?.Datos?.RegistrosTotales ?? 0;
                    response.Meta = new Metadata2()
                    {
                        RegistrosTotales = listTest?.Datos?.RegistrosTotales ?? 0,
                        NoPagina = ordenesContratistaRequestDto.ServerSideJson?.first ?? 0,
                        RegistrosPorPagina = ordenesContratistaRequestDto.ServerSideJson?.rows ?? 0
                    };
                    return Ok(response);
                }
                
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistasTerritorial")]
        [HttpGet("ObtenerReporteTrazaScrContratistasExcel/{format}", Name = "GetReporteTrazaScrContratistasExcel/{format}")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetReporteTrazaScrContratistasExcel([FromQuery] OrdenesContratistaRequestDto parameters, string format)
        {
            try
            {
                int.TryParse(HttpContext.Items["id_persona"]?.ToString() ?? "0", out int v_id_persona);
                //int.TryParse(HttpContext.Items["id_usuario"]?.ToString() ?? "0", out int vid_usuario);
                parameters.id_persona = v_id_persona.ToString();
                //parameters.id_usuario = vid_usuario.ToString();
                var streamresponse = await _op360ReportingService.Consultar_Reporte_TrazaScr_Contratistas_Excel(parameters, format);
                if (format == "base64" && streamresponse.Codigo == 200)
                {
                    return Ok(new ApiResponse<FileResponseOracleBase64>(streamresponse.Datos.Item1, streamresponse.Codigo, streamresponse.Mensaje, streamresponse.TotalRecords));
                }
                else if (format == "bytes" && streamresponse.Codigo == 200)
                {
                    return File(streamresponse.Datos.Item2, streamresponse.Datos.Item1.TypeMime, streamresponse.Datos.Item1.NombreArchivo);
                }
                else
                {
                    return BadRequest(new ApiResponse<FileResponseOracleBase64>(new FileResponseOracleBase64() { }, 400, !new[] { "base64", "bytes" }.Contains(format) ? "formato inexistente" : streamresponse.Mensaje));
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la descarga del excel. Detalle: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }
        #endregion

        #region Filtros        
        /// <summary>
        /// Retorna los parametros iniciales para el modulo de ordenes Contratistas
        /// Carlos Vargas
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetParametrosIniciales", Name = "ParametrosInicialesFiltros")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminAreaCentral")]
        public async Task<IActionResult> ParametrosInicialesFiltros()
        {
            try
            {
                var listTest = await _op360Service.GetParametrosInicialesFiltros();
                var statusCode = listTest.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ParametrosInicialesFiltrosDto>(listTest.Datos, statusCode, listTest.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Obtiene los filtros.
        /// </summary>
        /// <returns>Response.</returns>
        [HttpGet("ConsultarFiltros", Name = "ConsultarFiltros")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminAreaCentral")]
        public async Task<IActionResult> ConsultarFiltros()
        {
            try
            {
                var data = await _op360Service.ConsultarFiltros();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<IList<GetFiltroResponseDto>>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Crea el filtro.
        /// </summary>
        /// <param name="crearFiltroRequestDto">Parametros de entrada.</param>
        /// <returns>Response.</returns>
        [HttpPost("CrearFiltro", Name = "CrearFiltro")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminAreaCentral")]
        public async Task<IActionResult> CrearFiltro([FromBody] CreateFiltroRequestDto crearFiltroRequestDto)
        {
            try
            {
                var data = await _op360Service.CrearFiltro(crearFiltroRequestDto);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<CreateFiltroResponseDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Actualiza el filtro.
        /// </summary>
        /// <param name="updateFiltroRequestDto">Parametros de entrada.</param>
        /// <returns>Response.</returns>
        [HttpPut("ActualizarFiltro", Name = "ActualizarFiltro")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminAreaCentral")]
        public async Task<IActionResult> ActualizarFiltro([FromBody] UpdateFiltroRequestDto updateFiltroRequestDto)
        {
            try
            {
                var data = await _op360Service.ActualizarFiltro(updateFiltroRequestDto);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Elimina el filtro.
        /// </summary>
        /// <param name="deleteFiltroRequestDto">Parametros de entrada.</param>
        /// <returns>Response.</returns>
        [HttpDelete("EliminarFiltro/{id_filtro}", Name = "EliminarFiltro")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminAreaCentral")]
        public async Task<IActionResult> EliminarFiltro([FromRoute] DeleteFiltroRequestDto deleteFiltroRequestDto)
        {
            try
            {
                var data = await _op360Service.EliminarFiltro(deleteFiltroRequestDto);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ListarFiltros", Name = "ListarFiltros")]
        [Consumes("application/json")]
        public async Task<IActionResult> ListarFiltros()
        {
            try
            {
                var data = await _op360Service.Listar_Filtros();
                var statusCode = data != null ? 200 : 400;
                var response = new ApiResponse<Op360_ListarFiltrosResponseDto>(data, statusCode, "");

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("EditarFiltros", Name = "EditarFiltros")]
        [Consumes("application/json")]
        public async Task<IActionResult> EditarFiltros([FromBody] Op360_EditarFiltrosDto parameters)
        {
            try
            {
                var data = await _op360Service.Editar_Filtros(parameters);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }        
        #endregion
    }
}