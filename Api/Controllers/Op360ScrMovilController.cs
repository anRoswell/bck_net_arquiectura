namespace Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Api.Responses;
    using Core.Dtos.RechazarOrdenDto;
    using Core.DTOs;
    using Core.DTOs.CargaInicialMovilDto;
    using Core.DTOs.ComprometerOrdenDto;
    using Core.DTOs.CuestionarioInstanciasMovilDto;
    using Core.DTOs.EncuestaInicialMovilDto;
    using Core.DTOs.OrdenesAsignadasTecnicoMovilDto;
    using Core.DTOs.RegistrarGestionOrdenMovilDto;
    using Core.Exceptions;
    using Core.Interfaces;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Serilog;

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class Op360ScrMovilController : ControllerBase
    {
        private readonly IAire360MCargaInicialService _aire360MCargaInicialService;
        private readonly ILogger<Op360ScrMovilController> _logger;

        public Op360ScrMovilController(IAire360MCargaInicialService aire360MCargaInicialService, ILogger<Op360ScrMovilController> logger)
        {
            _aire360MCargaInicialService = aire360MCargaInicialService;
            _logger = logger;
        }

        /// <summary>
        ///Obtiene los registros de carga inicial.
        /// </summary>
        /// <returns>Json de respuesta con los registros.</returns>
        [HttpGet("GetCargaInicial", Name = "GetCargaInicial")]
        [Consumes("application/json")]    
        public async Task<IActionResult> GetCargaInicial()
        {
            try
            {
                var data = await _aire360MCargaInicialService.GetCargaInicialAsync();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto<DataCargaInicialMovilDto>>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Obtiene la encuesta inicial.
        /// </summary>
        /// <param name="encuestaInicialRequestMovilDto">Parametro de entrada.</param>
        /// <returns>Json de respuesta con los registros.</returns>
        [HttpGet("GetEncuestaInicial/{id_contratista_persona}", Name = "GetEncuestaInicial/{id_contratista_persona}")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetEncuestaInicial([FromRoute] EncuestaInicialRequestMovilDto encuestaInicialRequestMovilDto)
        {
            try
            {
                var data = await _aire360MCargaInicialService.GetEncuestaInicialAsync(encuestaInicialRequestMovilDto);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto<EncuestaInicialMovilDto>>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Obtiene las ordenes asignadas por tecnico.
        /// </summary>
        /// <param name="ordenesAsignadasTecnicoMovilRequestDto">Parametro de entrada.</param>
        /// <returns>Json de respuesta con los registros.</returns>
        [HttpGet("GetOrdenesAsignadasTecnico/{id_contratista_persona}", Name = "GetOrdenesAsignadasTecnico/{id_contratista_persona}")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetOrdenesAsignadasTecnico([FromRoute] OrdenesAsignadasTecnicoMovilRequestDto ordenesAsignadasTecnicoMovilRequestDto)
        {
            try
            {
                var data = await _aire360MCargaInicialService.GetOrdenesAsignadasTecnicoAsync(ordenesAsignadasTecnicoMovilRequestDto);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto<IList<OrdenesAsignadasTecnicoMovilDto>>>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Crea el cuestionario.
        /// </summary>
        /// <param name="cuestionarioInstanciasRequestMovilDto">Parametro de entrada.</param>
        /// <returns>Id del cuestionario creado.</returns>
        [HttpPost("CreateCuestionarioInstancia", Name = "CreateCuestionarioInstancia")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateCuestionarioInstancia([FromBody] CuestionarioInstanciasRequestMovilDto cuestionarioInstanciasRequestMovilDto)
        {
            try
            {
                var data = await _aire360MCargaInicialService.CreateCuestionarioInstanciaAsync(cuestionarioInstanciasRequestMovilDto);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto<CuestionarioInstanciasResponseMovilDto>>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Registrar gestion de orden.
        /// </summary>
        /// <param name="registrarGestionOrdenMovilRequestDto">Parametro de entrada.</param>
        /// <returns>Id de la gestion de orden creada.</returns>
        [HttpPost("RegisterGestionOrden", Name = "RegisterGestionOrden")]
        [Consumes("application/json")]
        public async Task<IActionResult> RegisterGestionOrden([FromBody] RegistrarGestionOrdenMovilRequestDto registrarGestionOrdenMovilRequestDto)
        {
            try
            {
                LogElastic(registrarGestionOrdenMovilRequestDto);

                var data = await _aire360MCargaInicialService.RegisterGestionOrdenAsync(registrarGestionOrdenMovilRequestDto);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Rechazar la orden.
        /// </summary>
        /// <param name="rechazarOrdenRequestDto">Parametro de entrada.</param>
        /// <returns>Response.</returns>
        [HttpPut("RechazarOrden", Name = "RechazarOrden")]
        [Consumes("application/json")]
        public async Task<IActionResult> RechazarOrden([FromBody] RechazarOrdenRequestDto rechazarOrdenRequestDto)
        {
            try
            {
                var data = await _aire360MCargaInicialService.RechazarOrdenAsync(rechazarOrdenRequestDto);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Compromete la orden
        /// </summary>
        /// <param name="comprometerOrdenRequestDto">Parametros de entrada.</param>
        /// <returns>Reponse.</returns>
        [HttpPut("ComprometerOrden", Name = "ComprometerOrden")]
        [Consumes("application/json")]
        public async Task<IActionResult> ComprometerOrden([FromBody]ComprometerOrdenRequestDto comprometerOrdenRequestDto)
        {
            try
            {
                var data = await _aire360MCargaInicialService.ComprometerOrdenAsync(comprometerOrdenRequestDto);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        private void LogElastic(object data)
        {
            string log = JsonConvert.SerializeObject(data);
            _logger.LogInformation(log);
        }
    }
}