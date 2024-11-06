namespace Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Api.Responses;
    using Core.DTOs;
    using Core.DTOs.Gos.Mobile.ComprometerGestion;
    using Core.DTOs.Gos.Mobile.CrearGestion;
    using Core.DTOs.Gos.Mobile.ObtenerGestionesByGestorDto;
    using Core.DTOs.ObtenerGestionesByGestorDto;
    using Core.DTOs.ProcesarGestionDto;
    using Core.Exceptions;
    using Core.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class Op360GosMobileController : ControllerBase
    {
        public readonly IOp360GosMobileService _op360GosMobileService;

        public Op360GosMobileController(IOp360GosMobileService op360GosMobileService)
		{
            _op360GosMobileService = op360GosMobileService;
        }

        /// <summary>
        /// Obtiene las gestiones por gestor asignado.
        /// </summary>
        /// <param name="obtenerGestionesByGestorRequestDto">Parametro de entrada.</param>
        /// <returns>Lista de gestiones</returns>
        [HttpGet("ObtenerGestionesByGestor/{id_contratista_persona_gestor}", Name = "ObtenerGestionesByGestor")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerGestionesByGestor([FromRoute] ObtenerGestionesByGestorRequestDto obtenerGestionesByGestorRequestDto)
        {
            try
            {
                var data = await _op360GosMobileService.ObtenerGestionesByGestorAsync(obtenerGestionesByGestorRequestDto);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<IList<GestionesByGestorDto>>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Procesa la gestion
        /// </summary>
        /// <param name="procesarGestionRequestDto">Parametros de entrada.</param>
        /// <returns>Respuesta base</returns>
        [HttpPost("ProcesarGestion", Name = "ProcesarGestion")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> ProcesarGestion([FromBody] ProcesarGestionRequestDto procesarGestionRequestDto)
        {
            try
            {
                var data = await _op360GosMobileService.ProcesarGestionAsync(procesarGestionRequestDto);
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
        /// Crea la gestion
        /// </summary>
        /// <param name="crearGestionRequestDto">Parametros de entrada.</param>
        /// <returns>Respuesta base</returns>
        [HttpPost("CrearGestion", Name = "CrearGestion")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> CrearGestion([FromBody] CrearGestionRequestDto crearGestionRequestDto)
        {
            try
            {
                var data = await _op360GosMobileService.CrearGestionAsync(crearGestionRequestDto);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto<CrearGestionResponseDto>>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }

        /// <summary>
        /// Compromete la gestion
        /// </summary>
        /// <param name="comprometerGestionRequestDto">Parametros de entrada.</param>
        /// <returns>Respuesta base</returns>
        [HttpPost("ComprometerGestion", Name = "ComprometerGestion")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> ComprometerGestion([FromBody] ComprometerGestionRequestDto comprometerGestionRequestDto)
        {
            try
            {
                var data = await _op360GosMobileService.ComprometerGestionAsync(comprometerGestionRequestDto);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }
    }
}