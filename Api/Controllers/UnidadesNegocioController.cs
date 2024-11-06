using Api.Responses;
using Core.DTOs;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelResponse;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadesNegocioController : ControllerBase
    {
        private readonly IUnidadesNegocioService _service;
        public UnidadesNegocioController(IUnidadesNegocioService service)
        {
            _service = service;
        }

        [HttpPost("CrearUnidadNegocio",Name = "Crear")]
        [Consumes("application/json")]
        public async Task<IActionResult> Crear([FromBody] UnidadesNegocioDto unidadesNegocio) 
        {
            try
            {
                var responseService = await _service.Crear(unidadesNegocio);
                var response = new ApiResponse<List<ResponseAction>>(responseService, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la creacion de unidades de negocio. Detalle: {e.Message}");
            }
        }

        [HttpPut("EditarUnidadNegocio", Name = "Editar")]
        [Consumes("application/json")]
        public async Task<IActionResult> Editar([FromBody] UnidadesNegocioDto unidadesNegocio)
        {
            try
            {
                var responseService = await _service.Editar(unidadesNegocio);
                var response = new ApiResponse<List<ResponseAction>>(responseService, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la edición de unidades de negocio. Detalle: {e.Message}");
            }
        }

        [HttpGet("BuscarUnidadNegocioPorId", Name = "BuscarPorId")]
        [Consumes("application/json")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            try
            {
                var responseService = await _service.BuscarPorId(id);
                var response = new ApiResponse<UnidadesNegocioDto>(responseService, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la consulta de unidades de negocio. Detalle: {e.Message}");
            }
        }

        [HttpGet("BuscarUnidadesNegocio", Name = "BuscarTodos")]
        [Consumes("application/json")]
        public async Task<IActionResult> BuscarTodos()
        {
            try
            {
                var responseService = await _service.BuscarTodos();
                var response = new ApiResponse<List<UnidadesNegocioDto>>(responseService, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la consulta de unidades de negocio. Detalle: {e.Message}");
            }
        }

        [HttpDelete("EliminarUnidadNegocio", Name = "Eliminar")]
        [Consumes("application/json")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var responseService = await _service.Eliminar(id);
                var response = new ApiResponse<List<ResponseAction>>(responseService, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la eliminacion de unidades de negocio. Detalle: {e.Message}");
            }
        }
    }
}
