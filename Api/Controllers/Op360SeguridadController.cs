namespace Api.Controllers
{
    using Api.Responses;
    using Api.ViewsProcess;
    using Core.Entities;
    using Core.Exceptions;
    using Core.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Threading.Tasks;

    [Authorize(Policy = "ShouldBeAnAdminAreaCentral")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class Op360SeguridadController : ControllerBase
    {
        private readonly IOp360Service _op360Service;
        private readonly IConfiguration _configuration;

        public Op360SeguridadController(
            IConfiguration configuration,
            IOp360Service op360Service
            )
        {
            _configuration = configuration;
            _op360Service = op360Service;
        }

        #region Seguridad
        /// <summary>
        /// Login con Apex
        /// clave: 5e6ewrtLOGINEXTERNO6weasdf _01
        /// Carlos Vargas
        /// </summary>
        /// <param name="Id_Usuario"></param>
        /// <param name="Token_Apex"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("LoginExterno", Name = "ExternalLogin")]
        [Consumes("application/json")]
        public async Task<IActionResult> ExternalLogin(string Id_Usuario, string Token_Apex)
        {
            try
            {
                Usuarios_Perfiles listConsulta = await _op360Service.ValidarLoginExterno(Id_Usuario, Token_Apex);
                var response = new ApiResponse<Usuarios_Perfiles>(listConsulta, listConsulta.Estado, listConsulta.Mensaje);
                if (response.Status == 200)
                {
                    TokenProcess tokenProcess = new TokenProcess(_configuration);
                    var token = tokenProcess.GenerateToken(listConsulta.Datos, HttpContext);
                    HttpContext.Response.Headers.Add("Authorization", token);
                    return Ok(response);
                }
                else
                {
                    HttpContext.Response.Headers.Add("Authorization", "");
                    return Unauthorized(response);
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
