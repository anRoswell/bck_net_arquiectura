namespace Api.Controllers
{
    using Api.Responses;
    using Core.DTOs;
    using Core.DTOs.RegistrarGestionOrdenMovilDto;
    using Core.Exceptions;
    using Core.Interfaces;
    using Core.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class VersonDBController : ControllerBase
    {
        private readonly IVersonDBService _versonDBService;
        private readonly ILogger<VersonDBController> _logger;

        public VersonDBController(IVersonDBService versonDBService, ILogger<VersonDBController> logger)
        {
            _versonDBService = versonDBService;
            _logger = logger;
        }

        [HttpGet("ObtenerVersonDB", Name = "ObtenerVersonDB")]
        [Consumes("application/json")]
        public async Task<IActionResult> ObtenerVersonDB()
        {
            try
            {
                var data = await _versonDBService.ObtenerVersonDBAsync();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ObtenerVersonDBDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Metodo de prueba
        /// </summary>
        /// <param name="registrarGestionOrdenMovilRequestDto"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpPost("PruebasJsonElasticSearch", Name = "PruebasJsonElasticSearch")]
        [Consumes("application/json")]
        public async Task<IActionResult> PruebasJsonElasticSearch([FromBody] RegistrarGestionOrdenMovilRequestDto registrarGestionOrdenMovilRequestDto)
        {
            try
            {
                LogElastic(registrarGestionOrdenMovilRequestDto);

                return Ok();
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
