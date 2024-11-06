using Api.Responses;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize(Policy = "ShouldBeAnAdmin")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class MenuXPerfilController : ControllerBase
    {
        private readonly IPermisosMenuXPerfilService _permisosMenuXPerfilService;

        public MenuXPerfilController(IPermisosMenuXPerfilService permisosMenuXPerfilService)
        {
            _permisosMenuXPerfilService = permisosMenuXPerfilService;
        }

        /// <summary>
        /// Consultar Permisos de menu de un perfil específico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Search", Name = "SearchMenuXperfil")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchMenuXperfil(int id)
        {
            try
            {
                var resp = await _permisosMenuXPerfilService.GetPorId(id);
                var entidad = resp.Count > 0 ? resp[0] : null;

                if (entidad is null)
                {
                    return Ok(ErrorResponse.GetError(false, "Menu por Usuario Inválido", 400));
                }

                var response = new ApiResponse<PermisosMenuXperfil>(entidad, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Consultar Permisos de menu de todos los perfiles
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchAll", Name = "SearchAllMenuXperfil")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchAllMenuXperfil()
        {
            try
            {
                var entidades = await _permisosMenuXPerfilService.GetListado();
                var response = new ApiResponse<List<PermisosMenuXperfil>>(entidades, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Asignar permiso de menu a un perfil especifico
        /// </summary>
        /// <param name="permisosMenu"></param>
        /// <returns></returns>
        [HttpPost("Create", Name = "CreateMenuXperfil")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateMenuXperfil([FromBody] PermisosMenuXperfil permisosMenu)
        {
            try
            {
                var responseAction = await _permisosMenuXPerfilService.PostCrear(permisosMenu);
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
        /// Actualizar permiso de menu de un perfil especifico
        /// </summary>
        /// <param name="permisosMenu"></param>
        /// <returns></returns>
        [HttpPut("Update", Name = "UpdateMenuXperfil")]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateMenuXperfil([FromBody] PermisosMenuXperfil permisosMenu)
        {
            try
            {
                permisosMenu.CodUserUpdate = HttpContext.Items["UserID"]?.ToString();
                var responseAction = await _permisosMenuXPerfilService.PutActualizar(permisosMenu);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la actualización del registro. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Quitar permiso de menu a un perfil especifico
        /// </summary>
        /// <param name="permisosMenu"></param>
        /// <returns></returns>
        [HttpDelete("Delete", Name = "DeleteMenuXperfil")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteMenuXperfil([FromBody] PermisosMenuXperfil permisosMenu)
        {
            try
            {
                permisosMenu.CodUserUpdate = HttpContext.Items["UserID"]?.ToString();
                var responseAction = await _permisosMenuXPerfilService.DeleteRegistro(permisosMenu);
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
