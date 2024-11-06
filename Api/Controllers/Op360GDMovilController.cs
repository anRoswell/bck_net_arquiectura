using Api.Responses;
using Core.DTOs.CargaInicialMovilDto;
using Core.DTOs;
using Core.Interfaces;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Core.Entities;
using Core.Exceptions;
using Core.QueryFilters;
using NPOI.OpenXmlFormats;
using Core.Dtos.RechazarOrdenDto;
using System.Collections.Generic;
using Core.DTOs.GDMovilDto;
using Core.Enumerations;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]

    public class Op360GDMovilController : ControllerBase
    {
        private readonly IOp360GDMovilService _GdMovilService;

        public Op360GDMovilController(IOp360GDMovilService gdmovilService)
        {
            _GdMovilService = gdmovilService;
        }

        #region End-pointListos

        #region CargaInicial
        //[Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("CargaInicialGD", Name = "CargaInicialGD")]
        [Consumes("application/json")]
        public async Task<IActionResult> CargaInicialGD()
        {
            try
            {
                var data = await _GdMovilService.CargaInicialAsync();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_DataCargaIniciaMovilDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region GetOrdenes
        //[Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ListaOrdenesAsignadasTecnicoGD/{id_tecnico}", Name = "GetOrdenesAsignadasTecnicoGD/{id_tecnico}")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetOrdenesAsignadasTecnicoGD([FromRoute] Op360GD_OrdenesAsignadasTecnicoMovilRequestDto op360GD_OrdenesAsignadasTecnicoMovilRequest)
        {
            try
            {
                var data = await _GdMovilService.GetOrdenesAsignadasTecnicoAsync(op360GD_OrdenesAsignadasTecnicoMovilRequest);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_OrdenesAsignadasTecnicoMovilDto[]>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region GestionDeOrdenes
        [HttpPost("GestionOrdenGD", Name = "GestionOrdenGD")]
        [Consumes("application/json")]
        public async Task<IActionResult> GestionOrdenGD([FromBody] Op360GD_GestionOrdenDto op360GD_GestionOrden)
        {
            try
            {
                var data = await _GdMovilService.RegistarGestionOrdenAsync(op360GD_GestionOrden);
                var statusCode = data.Codigo == 0 ? ResponseHttp.Ok : ResponseHttp.Error;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == ResponseHttp.Ok ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"error: {e.Message}");
            }
        }
        #endregion

        #endregion

        #region CascaronesConModelo

        #endregion

        #region CacaronesSinModelo

        #region ConsultaDeOrdenes
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ConsultaDeOrdenes", Name = "ConsultaDeOrdenes")]
        [Consumes("application/json")]
        public async Task<IActionResult> ConsultaDeOrdenes()
        {
            try
            {
                var data = await _GdMovilService.ConsultaOrdenesAsync();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_ConsultaOrdenesDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ConsultarOrdenXId", Name = "ConsultarOrdenXId")]
        [Consumes("application/json")]
        public async Task<IActionResult> ConsultarOrdenXId([FromQuery] Op360GD_OrdenIdDto gestionDanioOrdenId)
        {
            try
            {
                var data = await _GdMovilService.OrdenXIdAsync(gestionDanioOrdenId);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_OrdenId_ResponseDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
        #endregion

        #region RechazaryComprometerOrden
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("RechazarOrdenGD", Name = "RechazarOrdenGD")]
        [Consumes("application/json")]
        public async Task<IActionResult> RechazarOrdenGD([FromBody] Op360GD_RechazarOrdenRequestDto op360GD_RechazarOrdenRequest)
        {
            try
            {
                var data = await _GdMovilService.RechazarOrdenAsync(op360GD_RechazarOrdenRequest);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("ComprometerOrdenGD", Name = "ComprometerOrdenGD")]
        [Consumes("application/json")]
        public async Task<IActionResult> ComprometerOrdenGD([FromBody] Op360GD_ComprometerOrdenRequestDto op360GD_ComprometerOrdenRequest)
        {
            try
            {
                var data = await _GdMovilService.ComprometerOrdenAsync(op360GD_ComprometerOrdenRequest);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #endregion

    }
}
