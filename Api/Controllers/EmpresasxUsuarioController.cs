using Api.Responses;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelResponse;
using Core.QueryFilters;
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
    public class EmpresasxUsuarioController : ControllerBase
    {
        private readonly IPermisosEmpresasxUsuarioService _permisosEmpresasxUsuarioService;

        public EmpresasxUsuarioController(IPermisosEmpresasxUsuarioService permisosEmpresasxUsuarioService)
        {
            _permisosEmpresasxUsuarioService = permisosEmpresasxUsuarioService;
        }

        /// <summary>
        /// Consultar Permisos de empresas de usuario específico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Search", Name = "SearchEmpresaXusuario")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchEmpresaXusuario(int id)
        {
            try
            {
                var resp = await _permisosEmpresasxUsuarioService.GetPorId(id);
                var entidad = resp.Count > 0 ? resp[0] : null;

                if (entidad is null)
                {
                    return Ok(ErrorResponse.GetError(false, "Empresa por Usuario Inválido", 400));
                }

                var response = new ApiResponse<PermisosEmpresasxUsuario>(entidad, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Consultar Permisos de empresas de todos los usuarios
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchAll", Name = "SearchAllEmpresaXusuario")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchAllEmpresaXusuario()
        {
            try
            {
                var entidades = await _permisosEmpresasxUsuarioService.GetListado();
                var response = new ApiResponse<List<PermisosEmpresasxUsuario>>(entidades, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Asignar permiso de empresa a un usuario especifico
        /// </summary>
        /// <param name="empresasxUsuario"></param>
        /// <returns></returns>
        [HttpPost("Create", Name = "CreateEmpresaXusuario")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateEmpresaXusuario([FromBody] QueryCreatePermisoEmpresa empresasxUsuario)
        {
            try
            {
                var responseAction = await _permisosEmpresasxUsuarioService.PostCrear(empresasxUsuario);
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
        /// Actualizar permiso de empresa de un usuario especifico
        /// </summary>
        /// <param name="empresasxUsuario"></param>
        /// <returns></returns>
        [HttpPut("Update", Name = "UpdateEmpresaXusuario")]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateEmpresaXusuario([FromBody] QueryCreatePermisoEmpresa empresasxUsuario)
        {
            try
            {
                var responseAction = await _permisosEmpresasxUsuarioService.PutActualizar(empresasxUsuario);
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
        /// Quitar permiso de empresa a un usuario especifico
        /// </summary>
        /// <param name="empresasxUsuario"></param>
        /// <returns></returns>
        [HttpDelete("Delete", Name = "DeleteEmpresaXusuario")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteEmpresaXusuario([FromBody] PermisosEmpresasxUsuario empresasxUsuario)
        {
            try
            {
                empresasxUsuario.CodUserUpdate = HttpContext.Items["UserID"]?.ToString();
                var responseAction = await _permisosEmpresasxUsuarioService.DeleteRegistro(empresasxUsuario);
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