using Api.Responses;
using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    //[Authorize(Policy = "ShouldBeAnAdmin")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class PlantillaController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IUsuarioService _usuarioService;
        private readonly IParametrosInicialesService _paramsInicialesService;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;
        private readonly IPlantillaService _plantillaService;
        //private readonly IWebHostEnvironment _environment;

        public PlantillaController(
            IConfiguration configuration, 
            IUsuarioService usuarioService, 
            IParametrosInicialesService paramsInicialesService,
            IMapper mapper, 
            IPasswordService passwordService,
            IPlantillaService plantillaService
            //IWebHostEnvironment environment
        )
        {
            _configuration = configuration;
            _usuarioService = usuarioService;
            _mapper = mapper;
            _passwordService = passwordService;
            _paramsInicialesService = paramsInicialesService;
            _plantillaService = plantillaService;
            //_environment = environment;
        }

        /// <summary>
        /// Método para consultar un Json estructurado retonado desde procedimiento almacenado.
        /// </summary>
        /// <returns></returns>
        [HttpGet("PruebaJson", Name = "PruebaConsultarJson")]
        [Consumes("application/json")]
        public async Task<IActionResult> PruebaConsultarJson(int idReq)
        {
            try
            {
                var list = await _plantillaService.ConsultarJson(idReq);
                if (list is null)
                    list = new List<RequerimientoComparativoDto>();
                var response = new ApiResponse<List<RequerimientoComparativoDto>>(list, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para consultar varias tablas retornadas al tiempo desde procedimiento almacenado.
        /// </summary>
        /// <returns></returns>
        [HttpGet("PruebaMultiplesTablas", Name = "ConsultarMultiplesTablas")]
        [Consumes("application/json")]
        public async Task<IActionResult> ConsultarMultiplesTablas()
        {
            try
            {
                var list = await _plantillaService.ConsultarMultiplesTablas();
                var response = new ApiResponse<PruebaDataTable>(list, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }
    }
}