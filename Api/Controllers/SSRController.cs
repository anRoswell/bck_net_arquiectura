using Api.Responses;
using AutoMapper;
using AutoMapper.Configuration;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize] // DashboardUser
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SSRController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public SSRController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Consultar proveedor por Nit
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [HttpPut("SSR", Name = "SSRDataTable")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> SSRDataTable(PagingRequest paging)
        {
            try
            {
                var pagingResponse = new PagingResponse()
                {
                    Draw = paging.Draw
                };

                var usuario = await _usuarioService.GetListarUsuarios();
                var recordsTotal = usuario.Count;

                pagingResponse.RecordsTotal = recordsTotal;
                pagingResponse.RecordsFiltered = recordsTotal;
                pagingResponse.Usuarios = usuario.ToArray();

                //PagingRequest proveedorDto = _mapper.Map<SSRDto>(entidad);
                //var response = new ApiResponse<ProveedorDto>(proveedorDto, 200);
                //return Ok(response);
                return Ok(pagingResponse);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }
    }
}
