namespace Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Api.Responses;
    using Core.DTOs;
    using Core.DTOs.AsignarDesasignarOrdenesAGestoresDto;
    using Core.DTOs.ObtenerOrdenesTrabajoOficinaCentralDto;
    using Core.DTOs.CargaInicialGosDto;
    using Core.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Core.DTOs.Gos.GetOrdenById;
    using Core.DTOs.Gos.UpdateOrden;
    using Core.DTOs.Gos.CrearComentario;
    using Core.DTOs.Gos.CargarOrdenesAreaCentral;
    using Core.Enumerations;
    using Infrastructure.Interfaces;
    using Core.QueryFilters;
    using Core.DTOs.Gos.CambiarEstadoOrden;
    using Core.DTOs.Gos.ObtenerArchivosInstancia;
    using Core.DTOs.Gos.ObtenerArchivosInstanciaDetalle;
    using Core.DTOs.Gos.ObtenerOrdenesTrabajoOficinaCentralDto;
    using Core.DTOs.Gos.Web.ConsultaOrdenesFechaDto;
    using Core.DTOs.FilesDto;
    using Core.Exceptions;
    using System.Linq;
    using Core.DTOs.Gos.Web.GetSoporteById;
    using Core.DTOs.Gos.Web.ConsultarOrdenesGestion;
    using Core.DTOs.Gos.Web.Dashboard;
    using Newtonsoft.Json;

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class Op360GosWebController : ControllerBase
    {
        private readonly IOp360GosWebService _op360GosWebService;
        private readonly ICargaMasivaFactory _cargaMasivaFactory;
        private readonly IOp360ReportingService _op360ReportingService;

        public Op360GosWebController(IOp360GosWebService op360GosWebService, ICargaMasivaFactory cargaMasivaFactory, IOp360ReportingService op360ReportingService)
		{
            _op360GosWebService = op360GosWebService;
            _cargaMasivaFactory = cargaMasivaFactory;
            _op360ReportingService = op360ReportingService;
        }

        /// <summary>
        /// Carga los parametros iniciales para web y mobile.
        /// </summary>
        /// <returns>Lista de selectores.</returns>
        [AllowAnonymous]
        [HttpGet("GetParametrosIniciales", Name = "GetParametrosIniciales")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetParametrosIniciales()
        {
            try
            {
                var data = await _op360GosWebService.GetParametrosInicialesAsync();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto<ParametrosInicialesResponseDto>>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Carga las ordenes por id.
        /// </summary>
        /// <param name="getOrdenByIdRequest">parametro.</param>
        /// <returns>Listado de ordenes</returns>
        [Authorize(Policy = "ShouldBeAnGosAreaCentral")]
        [HttpGet("GetOrdenById/{id_orden}", Name = "GetOrdenById")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetOrdenById([FromRoute] GetOrdenByIdRequest getOrdenByIdRequest)
        {
            try
            {
                var data = await _op360GosWebService.GetOrdenByIdAsync(getOrdenByIdRequest);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto<GetOrdenByIdResponseDto>>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Actualiza la orden.
        /// </summary>
        /// <param name="updateOrdenRequestDto">Parametros.</param>
        /// <returns></returns>
        [HttpPut("UpdateOrden", Name = "UpdateOrden")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnGosAreaCentral")]
        public async Task<IActionResult> UpdateOrden([FromBody] UpdateOrdenRequestDto updateOrdenRequestDto)
        {
            try
            {
                int.TryParse(HttpContext.Items["id_usuario"]?.ToString() ?? "0", out int myUser);
                var data = await _op360GosWebService.UpdateOrdenAsync(updateOrdenRequestDto, myUser);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Crea el comentario de la orden.
        /// </summary>
        /// <param name="crearComentarioRequestDto">Parametros.</param>
        /// <returns></returns>
        [HttpPost("CrearComentarioAsync", Name = "CrearComentarioAsync")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnGosAreaCentral")]
        public async Task<IActionResult> CrearComentario([FromBody] CrearComentarioRequestDto crearComentarioRequestDto)
        {
            try
            {
                int.TryParse(HttpContext.Items["id_usuario"]?.ToString() ?? "0", out int myUser);
                var data = await _op360GosWebService.CrearComentarioAsync(crearComentarioRequestDto, myUser);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Obtiene las ordenes del trabajo oficina central.
        /// </summary>
        /// <param name="obtenerOrdenesTrabajoOficinaCentralRequestDto">Los Parametros.</param>
        /// <returns>Ordenes.</returns>
        [Authorize(Policy = "ShouldBeAnGosAreaCentral")]
        [HttpPost("ObtenerOrdenesTrabajoOficinaCentral", Name = "ObtenerOrdenesTrabajoOficinaCentral")]
        [Consumes("application/json")]
        public async Task<IActionResult> ObtenerOrdenesTrabajoOficinaCentral([FromBody] ObtenerOrdenesTrabajoOficinaCentralRequestDto obtenerOrdenesTrabajoOficinaCentralRequestDto)
        {
            try
            {
                var data = await _op360GosWebService.ObtenerOrdenesTrabajoOficinaCentralAsync(obtenerOrdenesTrabajoOficinaCentralRequestDto);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ObtenerOrdenesTrabajoOficinaCentralResponseDto>(data.Datos, statusCode, data.Mensaje);
                if (statusCode == 200)
                {
                    var request = JsonConvert.DeserializeObject<QueryOp360ServerSide>(obtenerOrdenesTrabajoOficinaCentralRequestDto.ServerSide);
                    response.TotalRecords = data?.paginacion.total_registros ?? 0;
                    response.Meta = new Metadata2()
                    {
                        RegistrosTotales = data?.TotalRecords ?? 0,
                        NoPagina = request.first ?? 0,
                        RegistrosPorPagina = request.rows ?? 0
                    };
                }

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Asignar o desasignar una orden segun el flag.
        /// </summary>
        /// <param name="asignarDesasignarOrdenesAGestoresRequestDto">Parametros.</param>
        /// <returns></returns>
        [HttpPut("AsignarDesasignarOrdenesAGestores", Name = "AsignarDesasignarOrdenesAGestores")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnGosAreaCentral")]
        public async Task<IActionResult> AsignarDesasignarOrdenesAGestores([FromBody] AsignarDesasignarOrdenesAGestoresRequestDto asignarDesasignarOrdenesAGestoresRequestDto)
        {
            try
            {
                var data = await _op360GosWebService.AsignarDesasignarOrdenesAGestoresAsync(asignarDesasignarOrdenesAGestoresRequestDto);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Carga las ordenes desde el excel.
        /// </summary>
        /// <param name="cargarOrdenesAreaCentralRequest">Parametros.</param>
        /// <returns></returns>
        [HttpPost("CargarOrdenesAreaCentral", Name = "CargarOrdenesAreaCentral")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnGosAreaCentral")]
        public async Task<IActionResult> CargarOrdenesAreaCentral([FromBody] CargarOrdenesAreaCentralRequestDto cargarOrdenesAreaCentralRequest)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(cargarOrdenesAreaCentralRequest.FileBase64))
                {
                    int.TryParse(HttpContext.Items["id_usuario"]?.ToString() ?? "0", out int myUser);
                    var cargaMasiva = _cargaMasivaFactory.Crear(CargaInicial.Gos);                    
                    var data = await cargaMasiva.Procesar(cargarOrdenesAreaCentralRequest, myUser);
                    var statusCode = data.Codigo == 0 ? 200 : 400;
                    var response = new ApiResponse<ResponseDto<Core.DTOs.CargaMasiva.MedirTiempo>>(data, statusCode, data.Mensaje);
                    var res = new ApiResponse<ResponseDto<Core.DTOs.CargaMasiva.MedirTiempo>>(data, 200, data.Mensaje);

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
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Obtiene los archivos procesados en carga masiva.
        /// </summary>
        /// <param name="obtenerArchivosInstanciaRequestDto">Los Parametros.</param>
        /// <returns>Archivos.</returns>
        [Authorize(Policy = "ShouldBeAnGosAreaCentral")]
        [HttpGet("ObtenerArchivosInstancia/{pagina}/{registros}", Name = "ObtenerArchivosInstancia")]
        [Consumes("application/json")]
        public async Task<IActionResult> ObtenerArchivosInstancia([FromRoute] ObtenerArchivosInstanciaRequestDto obtenerArchivosInstanciaRequestDto)
        {
            try
            {
                var data = await _op360GosWebService.ObtenerArchivosInstanciaAsync(obtenerArchivosInstanciaRequestDto);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ObtenerArchivosInstanciaResponseDto>(data.Datos, statusCode, data.Mensaje);
                if (statusCode == 200)
                {
                    response.TotalRecords = data?.paginacion.total_registros ?? 0;
                    response.Meta = new Metadata2()
                    {
                        RegistrosTotales = data?.paginacion.total_registros ?? 0,
                        NoPagina = obtenerArchivosInstanciaRequestDto.pagina ?? 0,
                        RegistrosPorPagina = obtenerArchivosInstanciaRequestDto.registros ?? 0
                    };
                }

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Obtiene los detalles del archivo procesado en carga masiva.
        /// </summary>
        /// <param name="obtenerArchivosInstanciaDetalleRequestDto">Los Parametros.</param>
        /// <returns>detalles del archivo.</returns>
        [Authorize(Policy = "ShouldBeAnGosAreaCentral")]
        [HttpGet("ObtenerArchivosInstanciaDetalle/{id_archivo_instancia}/{pagina}/{registros}", Name = "ObtenerArchivosInstanciaDetalle")]
        [Consumes("application/json")]
        public async Task<IActionResult> ObtenerArchivosInstanciaDetalle([FromRoute] ObtenerArchivosInstanciaDetalleRequestDto obtenerArchivosInstanciaDetalleRequestDto)
        {
            try
            {
                var data = await _op360GosWebService.ObtenerArchivosInstanciaDetalleAsync(obtenerArchivosInstanciaDetalleRequestDto);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ObtenerArchivosInstanciaDetalleResponseDto>(data.Datos, statusCode, data.Mensaje);
                if (statusCode == 200)
                {
                    response.TotalRecords = data?.paginacion.total_registros ?? 0;
                    response.Meta = new Metadata2()
                    {
                        RegistrosTotales = data?.paginacion.total_registros ?? 0,
                        NoPagina = obtenerArchivosInstanciaDetalleRequestDto.pagina ?? 0,
                        RegistrosPorPagina = obtenerArchivosInstanciaDetalleRequestDto.registros ?? 0
                    };
                }

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Cambia el estado de la orden.
        /// </summary>
        /// <param name="cambiarEstadoOrdenRequestDto">Parametros.</param>
        /// <returns></returns>
        [HttpPut("CambiarEstadoOrden", Name = "CambiarEstadoOrden")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnGosAreaCentral")]
        public async Task<IActionResult> CambiarEstadoOrden([FromBody] CambiarEstadoOrdenRequestDto cambiarEstadoOrdenRequestDto)
        {
            try
            {
                var data = await _op360GosWebService.CambiarEstadoOrdenAsync(cambiarEstadoOrdenRequestDto);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Obtiene el reporte de ordenes en excel.
        /// </summary>
        /// <param name="consultaOrdenesFechaRequestDto">Los Parametros.</param>
        /// <returns>Reporte.</returns>
        [Authorize(Policy = "ShouldBeAnGosAreaCentral")]
        [HttpGet("ConsultarOrdenesAreaCentralGosExcel/{fecha_inicial}/{fecha_final}/{formato}", Name = "ConsultarOrdenesAreaCentralGosExcel")]
        [Consumes("application/json")]
        public async Task<IActionResult> ConsultarOrdenesAreaCentralGosExcel([FromRoute] ConsultaOrdenesFechaRequestDto consultaOrdenesFechaRequestDto, string formato)
        {
            try
            {
                var streamresponse = await _op360ReportingService.ConsultarOrdenesAreaCentralGosExcel(consultaOrdenesFechaRequestDto, formato);
                if (formato == "base64" && streamresponse.Codigo == 200)
                {
                    return Ok(new ApiResponse<FileResponseOracleBase64>(streamresponse.Datos.Item1, streamresponse.Codigo, streamresponse.Mensaje, streamresponse.TotalRecords));
                }
                else if (formato == "bytes" && streamresponse.Codigo == 200)
                {
                    return File(streamresponse.Datos.Item2, streamresponse.Datos.Item1.TypeMime, streamresponse.Datos.Item1.NombreArchivo);
                }
                else
                {
                    return BadRequest(new ApiResponse<FileResponseOracleBase64>(new FileResponseOracleBase64() { }, 400, !new[] { "base64", "bytes" }.Contains(formato) ? "formato inexistente" : streamresponse.Mensaje));
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la descarga del excel. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Obtiene el base64 del soporte consultado.
        /// </summary>
        /// <param name="getSoporteByIdRequestDto">parametro.</param>
        /// <returns>Base64</returns>
       // [Authorize(Policy = "ShouldBeAnGosAreaCentral")]
        [HttpGet("GetSoporteById/{id_soporte}/{id_Ruta}", Name = "GetSoporteById")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetSoporteById([FromRoute] GetSoporteByIdRequestDto getSoporteByIdRequestDto)
        {
            try
            {
                var data = await _op360GosWebService.GetSoporteByIdAsync(getSoporteByIdRequestDto);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto<GetSoporteByIdResponseDto>>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Obtiene el reporte de ordenes gestion en excel.
        /// </summary>
        /// <param name="consultaOrdenesGestionDto">Los Parametros.</param>
        /// <returns>Reporte.</returns>
        [Authorize(Policy = "ShouldBeAnGosAreaCentral")]
        [HttpPost("ConsultaOrdenesGestionGosExcel", Name = "ConsultaOrdenesGestionGosExcel")]
        [Consumes("application/json")]
        public async Task<IActionResult> ConsultaOrdenesGestionGosExcel([FromBody] ConsultaOrdenesGestionDto consultaOrdenesGestionDto)
        {
            try
            {
                var streamresponse = await _op360ReportingService.ConsultaOrdenesGestionGosExcel(consultaOrdenesGestionDto);
                if (consultaOrdenesGestionDto.Format == "base64" && streamresponse.Codigo == 200)
                {
                    return Ok(new ApiResponse<FileResponseOracleBase64>(streamresponse.Datos.Item1, streamresponse.Codigo, streamresponse.Mensaje, streamresponse.TotalRecords));
                }
                else if (consultaOrdenesGestionDto.Format == "bytes" && streamresponse.Codigo == 200)
                {
                    return File(streamresponse.Datos.Item2, streamresponse.Datos.Item1.TypeMime, streamresponse.Datos.Item1.NombreArchivo);
                }
                else
                {
                    return BadRequest(new ApiResponse<FileResponseOracleBase64>(new FileResponseOracleBase64() { }, 400, !new[] { "base64", "bytes" }.Contains(consultaOrdenesGestionDto.Format) ? "formato inexistente" : streamresponse.Mensaje));
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la descarga del excel. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Carga datos del dashboard gos.
        /// </summary>
        /// <returns>Reporte.</returns>
        // [Authorize(Policy = "ShouldBeAnGosAreaCentral")]
        [HttpGet("GetDashboard", Name = "GetDashboard")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                var data = await _op360GosWebService.GetDashboardAsync();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto<ReporteDto>>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }
    }
}