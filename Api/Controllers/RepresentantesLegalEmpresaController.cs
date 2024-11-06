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
    public class RepresentantesLegalEmpresaController : ControllerBase
    {
        private readonly IRepresentantesLegalEmpresaService _service;
        public RepresentantesLegalEmpresaController(IRepresentantesLegalEmpresaService representantesLegalEmpresaService)
        {
            _service = representantesLegalEmpresaService;
        }

        /// <summary>
        /// Metodo para obtener todos los representantes legales
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpGet]
        public async Task<IActionResult>Get()
        {
            try
            {
                var response = await _service.Get();

                return Ok(new ApiResponse<List<RepresentantesLegalEmpresaDto>>(response, 200));
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la consulta del registro. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Metodo para obtener un representante legal por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var response = await _service.Get(id);

                return Ok(new ApiResponse<RepresentantesLegalEmpresaDto>(response, 200));
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la consulta del registro. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para obtener un representante legal por id de empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpGet("Empresa/{idEmpresa:int}")]
        public async Task<IActionResult> GetByEmpresa(int idEmpresa)
        {
            try
            {
                var response = await _service.GetByEmpresa(idEmpresa);

                return Ok(new ApiResponse<List<RepresentantesLegalEmpresaDto>>(response, 200));
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la consulta del registro. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Metodo para crear un representante legal
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RepresentantesLegalEmpresaDto dto)
        {
            try
            {
                var response = await _service.Add(dto);
                return Ok(new ApiResponse<RepresentantesLegalEmpresaDto>(response, 200));
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la creación del registro. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Metodo para editar un representante legal
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] RepresentantesLegalEmpresaDto dto)
        {
            try
            {
                var response = await _service.Edit(dto);
                return Ok(new ApiResponse<RepresentantesLegalEmpresaDto>(response, 200));
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la edición del registro. Detalle: {e.Message}");
            }
        }
        
        /// <summary>
        /// Metodo para eliminar un representante legal
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.Delete(id);
                return Ok(new ResponseAction() { estado= true , Id = id, mensaje = "Eliminación exitosa." });
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la edición del registro. Detalle: {e.Message}");
            }
        }
    }
}
