using Api.Responses;
using Core.DTOs;
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
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TipoMinutaController : ControllerBase
    {
        private readonly ITipoMinutaService _tipoMinutaService;

        public TipoMinutaController(ITipoMinutaService tipoMinutaService)
            => _tipoMinutaService = tipoMinutaService;

        [HttpGet(Name = "ObtenerTiposMinuta")]
        public async Task<IActionResult> ObtenerTiposMinuta()
        {
            try
            {
                List<TipoMinutaDto> tiposMinuta = await _tipoMinutaService.ObtenerTipoMinuta();
                var response = new ApiResponse<List<TipoMinutaDto>>(tiposMinuta, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la consulta de tipos de minuta. Detalle: {e.Message}");
            }
        }

        [HttpDelete("{id:int}", Name = "EliminarTipoMinuta")]
        public async Task<IActionResult> EliminarTipoMinuta(int id)
        {
            try
            {
                ResponseAction responseAction = await _tipoMinutaService.EliminarTipoMinuta(id);
                var response = new ApiResponse<List<ResponseAction>>(new List<ResponseAction> { responseAction }, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la eliminación del registro. Detalle: {e.Message}");
            }
        }

        [HttpPost(Name = "CrearTipoMinuta")]
        public async Task<IActionResult> CrearTipoMinuta([FromForm] TipoMinutaCreateCommand command)
        {
            try
            {
                TipoMinutaDto responseAction = await _tipoMinutaService.GuardarTipoDeMinuta(command);
                var response = new ApiResponse<TipoMinutaDto>(responseAction, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la creación del registro. Detalle: {e.Message}");
            }
        }
    }
}