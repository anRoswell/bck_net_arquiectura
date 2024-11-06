using Api.Responses;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    //[Authorize(Policy = "ShouldBeAnAdmin")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresasService _empresasService;

        public EmpresaController
        (
           IEmpresasService  empresaService
        )
        {
            _empresasService = empresaService;            
        }

        /// <summary>
        /// Consultar empresas
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchEmpresas", Name = "SearchEmpresas")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchEmpresas()
        {
            var  listEmpresas = await _empresasService.GetEmpresas();
            var  response = new ApiResponse<List<Empresa>>(listEmpresas, 200);
            return Ok(response);
        }

        /// <summary>
        /// Crear Empresa
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        [HttpPost("Create", Name = "CreateEmpresa")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateEmpresa([FromBody] Empresa empresa)
        {
            try
            {
                var responseAction = await _empresasService.PostCrear(empresa);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error al intentar insertar registro. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Eliminar Empresa
        /// </summary>
        /// <param name="empresa"></param>
        /// <returns></returns>
        [HttpDelete("Delete", Name = "DeleteEmpresa")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteEmpresa(Empresa empresa)
        {
            try
            {
                var responseAction = await _empresasService.DeleteEmpresa(empresa);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la eliminación del registro. Detalle: {e.Message}");
            }
        }

    }
}
