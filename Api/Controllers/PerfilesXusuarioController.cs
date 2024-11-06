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
    [Authorize(Policy = "ShouldBeAnAdminOrProv")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilesXusuarioController : ControllerBase
    {
        private readonly IPerfilesXusuarioService _perfilesXusuarioService;

        public PerfilesXusuarioController(IPerfilesXusuarioService perfilesXusuarioService)
        {
            _perfilesXusuarioService = perfilesXusuarioService;
        }

        /// <summary>
        /// Consultar Perfiles del Usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Search", Name = "SearchPerfilXusuario")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchPerfilXusuario(int id)
        {
            try
            {
                var resp = await _perfilesXusuarioService.GetPorId(id);
                var perfil = resp.Count > 0 ? resp[0] : null;

                if (perfil is null)
                {
                    return Ok(ErrorResponse.GetError(false, "Perfil por Usuario Inválido", 400));
                }

                var response = new ApiResponse<PerfilesXusuario>(perfil, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Consultar perfiles de un usuario especifico
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpGet("SearchByIdUsuario", Name = "SearchPerfilXusuarioByIdUsuario")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchPerfilXusuarioByIdUsuario(int idUsuario)
        {
            try
            {
                var resp = await _perfilesXusuarioService.GetPorIdUsuario(idUsuario);
                var response = new ApiResponse<List<PerfilesXusuario>>(resp, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Consultar Perfiles de Usuarios
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchAll", Name = "SearchAllPerfilXusuario")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchAllPerfilXusuario()
        {
            try
            {
                var perfiles = await _perfilesXusuarioService.GetListado();
                var response = new ApiResponse<List<PerfilesXusuarioView>>(perfiles, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Crear un perfil de usuario especifico
        /// </summary>
        /// <param name="perfilUsuario"></param>
        /// <returns></returns>
        [HttpPost("Create", Name = "CreatePerfilXusuario")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreatePerfilXusuario([FromBody] PerfilesXusuario perfilUsuario)
        {
            try
            {
                var responseAction = await _perfilesXusuarioService.PostCrear(perfilUsuario);
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
        /// Actualizar un perfil de usuario especifico
        /// </summary>
        /// <param name="perfilUsuario"></param>
        /// <returns></returns>
        [HttpPut("Update", Name = "UpdatePerfilXusuario")]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdatePerfilXusuario([FromBody] PerfilesXusuario perfilUsuario)
        {
            try
            {
                perfilUsuario.CodUserUpdate = HttpContext.Items["UserID"]?.ToString();
                var responseAction = await _perfilesXusuarioService.PutActualizar(perfilUsuario);
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
        /// Eliminar un perfil de usuario especifico
        /// </summary>
        /// <param name="perfilUsuario"></param>
        /// <returns></returns>
        [HttpDelete("Delete", Name = "DeletePerfilXusuario")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeletePerfilXusuario(PerfilesXusuario perfilUsuario)
        {
            try
            {
                perfilUsuario.CodUserUpdate = HttpContext.Items["UserID"]?.ToString();
                var responseAction = await _perfilesXusuarioService.DeleteRegistro(perfilUsuario);
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
