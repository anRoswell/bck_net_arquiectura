using Api.Responses;
using Core.DTOs;
using Core.DTOs.ASEGREDMovilDto;
using Core.DTOs.ASEGREDWebDto;
using Core.Exceptions;
using Core.Interfaces;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class Op360ASEGREDMovilController : ControllerBase
    {
        private readonly IOp360ASEGREDMovilService _op360ASEGREDMovilService;

        public Op360ASEGREDMovilController(IOp360ASEGREDMovilService op360ASEGREDMovilService)
        {
            _op360ASEGREDMovilService = op360ASEGREDMovilService;
        }

        #region End-pointListos

        #endregion

        #region CascaronesConModelo

        #region CierreDelProceso
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("CierreProceso", Name = "CierreProceso")]
        [Consumes("application/json")]
        public async Task<IActionResult> CierreProceso(Op360ASEGRED_CierreDeProcesoDto op360ASEGRED_CierreDeProceso)
        {
            try
            {
                var data = await _op360ASEGREDMovilService.CierreDeProceso(op360ASEGRED_CierreDeProceso);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Codigo);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region Obras
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("EjecucionObras", Name = "EjecucionObras")]
        [Consumes("application/json")]
        public async Task<IActionResult> EjecucionObras()
        {
            try
            {
                var data = await _op360ASEGREDMovilService.EjecucionDeObras();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_EjecucionDeObrasDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ListarObras", Name = "ListarObras")]
        [Consumes("application/json")]
        public async Task<IActionResult> ListarObras()
        {
            try
            {
                var data = await _op360ASEGREDMovilService.ListarObras();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_ListarObrasMovilDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region Estructuras
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("EjecucionDeEstructuras", Name = "EjecucionDeEstructuras")]
        [Consumes("application/json")]
        public async Task<IActionResult> EjecucionDeEstructuras()
        {
            try
            {
                var data = await _op360ASEGREDMovilService.EjecucionDeEstructuras();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_EjecucionDeEstructurasDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region Inspecciones
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ListarInspecciones", Name = "ListarInspecciones")]
        [Consumes("application/json")]
        public async Task<IActionResult> ListarInspecciones()
        {
            try
            {
                var data = await _op360ASEGREDMovilService.ListarInspecciones();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_ListarInspeccionesDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #endregion

        #region CacaronesSinModelo

        #region CargaInicial
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("CargaInicialASEGREDMovil", Name = "CargaInicialASEGREDMovil")]
        [Consumes("application/json")]
        public async Task<IActionResult> CargaInicialASEGREDMovil()
        {
            try
            {
                var data = await _op360ASEGREDMovilService.CargaInicialAsync();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_DataCargaInicialMovilDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region ConsultaDeOrdenes
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ConsultaDeOrdenesASEGRED", Name = "ConsultaDeOrdenesASEGRED")]
        [Consumes("application/json")]
        public async Task<IActionResult> ConsultaDeOrdenesASEGRED()
        {
            try
            {
                var data = await _op360ASEGREDMovilService.ConsultaOrdenesAsync();
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
        [HttpGet("ConsultarOrdenXIdASEGRED", Name = "ConsultarOrdenXIdASEGRED")]
        [Consumes("application/json")]
        public async Task<IActionResult> ConsultarOrdenXIdASEGRED([FromQuery] Op360ASEGRED_OrdenIdDto op360ASEGRED_OrdenIdDto)
        {
            try
            {
                var data = await _op360ASEGREDMovilService.OrdenXIdAsync(op360ASEGRED_OrdenIdDto);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
        #endregion

        #region GetOrdenes
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("GetOrdenesAsignadasTecnicoASEGRED/{id_contratista_persona}", Name = "GetOrdenesAsignadasTecnicoASEGRED/{id_contratista_persona}")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetOrdenesAsignadasTecnicoASEGRED([FromRoute] Op360ASEGRED_OrdenesAsignadasTecnicoMovilRequestDto OrdenesAsignadasTecnicoMovilRequestDto)
        {
            try
            {
                var data = await _op360ASEGREDMovilService.GetOrdenesAsignadasTecnicoAsync(OrdenesAsignadasTecnicoMovilRequestDto);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_OrdenesAsignadasTecnicoMovilDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region GuardarOrden
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("RegistarGestionOrdenASEGRED", Name = "RegistarGestionOrdenASEGRED")]
        [Consumes("application/json")]
        public async Task<IActionResult> RegistarGestionOrdenASEGRED([FromBody] Op360ASEGRED_GestionOrdenDto op360ASEGRED_GestionOrdenDto)
        {
            try
            {
                var data = await _op360ASEGREDMovilService.RegistarGestionOrdenAsync(op360ASEGRED_GestionOrdenDto);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"error: {e.Message}");
            }
        }
        #endregion

        #region RechazaryComprometerOrden
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("RechazarOrdenASEGRED", Name = "RechazarOrdenASEGRED")]
        [Consumes("application/json")]
        public async Task<IActionResult> RechazarOrdenASEGRED([FromBody] Op360ASEGRED_RechazarOrdenRequestDto op360ASEGRED_RechazarOrdenRequest)
        {
            try
            {
                var data = await _op360ASEGREDMovilService.RechazarOrdenAsync(op360ASEGRED_RechazarOrdenRequest);
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
        [HttpPut("ComprometerOrdenASEGRED", Name = "ComprometerOrdenASEGRED")]
        [Consumes("application/json")]
        public async Task<IActionResult> ComprometerOrdenASEGRED([FromBody] Op360ASEGRED_ComprometerOrdenRequestDto op360ASEGRED_ComprometerOrdenRequest)
        {
            try
            {
                var data = await _op360ASEGREDMovilService.ComprometerOrdenAsync(op360ASEGRED_ComprometerOrdenRequest);
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

        #region Obras
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ListarObraXContratista", Name = "ListarObraXContratista")]
        [Consumes("application/json")]
        public async Task<IActionResult> ListarObraXContratista()
        {
            try
            {
                var data = await _op360ASEGREDMovilService.ListarObraXContratista();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_ListarObraXContratistaDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region Estructuras
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("EditarEstructura", Name = "EditarEstructura")]
        [Consumes("application/json")]
        public async Task<IActionResult> EditarEstructura(Op360ASEGRED_EditarEstructuraMovilDto op360ASEGRED_EditarEstructuraMovil)
        {
            try
            {
                var data = await _op360ASEGREDMovilService.EditarEstructura(op360ASEGRED_EditarEstructuraMovil);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #endregion

    }
}
