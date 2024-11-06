using Api.Responses;
using Core.Entities;
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
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        /// <summary>
        /// Obtener opciones de Menú
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchAll", Name = "SearchAllMenu")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchAllMenu(int TipoUsuario)
        {
            try
            {
                var menus = await _menuService.GetMenus(TipoUsuario);
                //var response = new ApiResponse<List<Menu>>(menus, 200);
                //return Ok(response);
                return Ok();
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }
    }
}
