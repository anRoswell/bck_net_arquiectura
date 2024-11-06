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
    public class UsuarioxMenuController : ControllerBase
    {
        private readonly IPermisosUsuarioxMenuService _permisosUsuarioxMenuService;

        public UsuarioxMenuController(IPermisosUsuarioxMenuService permisosUsuarioxMenuService)
        {
            _permisosUsuarioxMenuService = permisosUsuarioxMenuService;
        }

        /// <summary>
        /// Consultar Permisos de usuario de un menu específico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Search", Name = "SearchUsuarioXMenu")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchUsuarioXMenu(int id)
        {
            try
            {
                var resp = await _permisosUsuarioxMenuService.GetPorId(id);
                var entidad = resp.Count > 0 ? resp[0] : null;

                if (entidad is null)
                {
                    return Ok(ErrorResponse.GetError(false, "Usuario por Menu Inválido", 400));
                }

                var response = new ApiResponse<PermisosUsuarioxMenu>(entidad, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Consultar Permisos de usuario de todos los menus
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchAll", Name = "SearchAllUsuarioXMenu")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchAllUsuarioXMenu()
        {
            try
            {
                var entidades = await _permisosUsuarioxMenuService.GetListado();
                var response = new ApiResponse<List<PermisosUsuarioxMenu>>(entidades, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Asignar permiso de usuario a un menu especifico
        /// </summary>
        /// <param name="usuarioxMenu"></param>
        /// <returns></returns>
        [HttpPost("Create", Name = "CreateUsuarioXMenu")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateUsuarioXMenu([FromBody] PermisosUsuarioxMenu usuarioxMenu)
        {
            try
            {
                var responseAction = await _permisosUsuarioxMenuService.PostCrear(usuarioxMenu);
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
        /// Actualizar permiso de usuario de un menu especifico
        /// </summary>
        /// <param name="usuarioxMenu"></param>
        /// <returns></returns>
        [HttpPut("Update", Name = "UpdateUsuarioXMenu")]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateUsuarioXMenu([FromBody] PermisosUsuarioxMenu usuarioxMenu)
        {
            try
            {
                var responseAction = await _permisosUsuarioxMenuService.PutActualizar(usuarioxMenu);
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
        /// Quitar permiso de usuario a un menu especifico
        /// </summary>
        /// <param name="usuarioxMenu"></param>
        /// <returns></returns>
        [HttpDelete("Delete", Name = "DeleteUsuarioXMenu")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteUsuarioXMenu([FromBody] PermisosUsuarioxMenu usuarioxMenu)
        {
            try
            {
                var responseAction = await _permisosUsuarioxMenuService.DeleteRegistro(usuarioxMenu);
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
