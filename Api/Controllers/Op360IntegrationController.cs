using Api.Responses;
using Api.ViewsProcess;
using Core.DTOs;
using Core.DTOs.CargaInicialGosDto;
using Core.DTOs.Integration;
using Core.DTOs.Integration.Authentication;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Op360IntegrationController : ControllerBase
    {
        private readonly IOp360IntegrationService _op360IntegrationService;
        private readonly IConfiguration _configuration;

        public Op360IntegrationController(
            IOp360IntegrationService op360IntegrationService,
            IConfiguration configuration
            )
        {
            _op360IntegrationService = op360IntegrationService;
            _configuration = configuration;
        }

        //controller de prueba
        [HttpPost("GetOrdenes", Name = "GetOrdenes")]
        [Consumes("application/json")]
        //[Authorize(Policy = "ShouldBeOsfPolicy")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrdenes([FromBody] Body _integration)
        {
            try
            {
                var data = await _op360IntegrationService.GuardarOrdenIntegrationBodyPrueba(_integration);
                var statusCode = data.codigo == "CO_000" ? 200 : 400;
                var response = new IntegrationResponseDto(data.codigo, data.mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Genera un token de maximo 300 caracteres.
        /// </summary>  
        /// <param name="authenticationRequestDto">Dto con usuario y contraseña.</param>
        /// <returns>Token.</returns>
        [HttpPost("Authentication", Name = "Authentication")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> Authentication([FromBody] AuthenticationRequestDto authenticationRequestDto)
        {
            try
            {
                var data = await _op360IntegrationService.AuthenticationAsync(authenticationRequestDto);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto<AuthenticationResponseDto>>(data, statusCode, data.Mensaje);
                if (statusCode == 400)
                {
                    HttpContext.Response.Headers.Add("Authorization", "");
                    return BadRequest(response);
                }
                TokenProcess tokenProcess = new(_configuration);
                var token = tokenProcess.GenerateTokenOsf(data.Datos);
                HttpContext.Response.Headers.Add("Authorization", token);
                var newData = new ResponseDto<TokenResponseDto>()
                {
                    Codigo = data.Codigo,
                    Mensaje = data.Mensaje,
                    TotalRecords = data.TotalRecords,
                    Datos = new TokenResponseDto() { token = token}
                };
                var newResponse = new ApiResponse<ResponseDto<TokenResponseDto>>(newData, statusCode, newData.Mensaje);
                return Ok(newResponse);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
    }
}
