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
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ShouldBeAnAdminOrProv")]
    public class OrdenesMaestrasController : ControllerBase
    {
        private readonly IOrdenesMaestrasService _ordenesMaestrasService;

        public OrdenesMaestrasController(IOrdenesMaestrasService ordenesMaestrasService)
        {
            _ordenesMaestrasService = ordenesMaestrasService;
        }

        /// <summary>
        /// Buscar Ordenes maestro
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchAll", Name = "SearchOrdenes")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchOrdenes()
        {
            List<OrdenesMaestras> listConsulta = await _ordenesMaestrasService.GetListado();
            var response = new ApiResponse<List<OrdenesMaestras>>(listConsulta, 200);
            return Ok(response);
        }

        [HttpPost("OrdenReq", Name = "PostOrdenReq")]
        [Consumes("application/json")]        
        public async Task<IActionResult> PostOrdenReq([FromBody] OrdenReq ordenReq)
        {
            try
            {
                ordenReq.CodUser = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
              

                var responseAction = await _ordenesMaestrasService.PostOrdenReq(ordenReq);
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
        /// Buscar Ordenes por proveedor
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchByProveedor", Name = "SearchByProveedor")]
        [Consumes("application/json")]        
        public async Task<IActionResult> SearchByProveedor([FromQuery] QuerySearchOrdenes parametros)
        {
            parametros.CodProveedor = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
            List<OrdenesMaestras> listConsulta = await _ordenesMaestrasService.SearchByProveedor(parametros);
            var response = new ApiResponse<List<OrdenesMaestras>>(listConsulta, 200);
            return Ok(response);
        }
    }
}
