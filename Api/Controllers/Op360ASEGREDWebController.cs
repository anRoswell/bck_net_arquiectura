using Api.Responses;
using Core.DTOs;
using Core.DTOs.ASEGREDWebDto;
using Core.Entities.ASEGREDWebEntities;
using Core.Exceptions;
using Core.Interfaces;
using Core.QueryFilters.QueryFiltersASEGREDWeb;
using Infrastructure.Validators.ASEGREDWebValidators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class Op360ASEGREDWebController : ControllerBase
    {
        private readonly IOp360ASEGREDWebService _op360ASEGREDWebService;

        public Op360ASEGREDWebController(IOp360ASEGREDWebService op360ASEGREDWebService)
        {
            _op360ASEGREDWebService = op360ASEGREDWebService;
        }

        #region End-pointListos

        #region CargaInicial
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("CargaInicialASEGREDWeb", Name = "CargaInicialASEGREDWeb")]
        [Consumes("application/json")]
        public async Task<IActionResult> CargaInicialASEGREDWeb()
        {
            try
            {
                var respDto = await _op360ASEGREDWebService.CargaInicialAsync();
                var statusCode = respDto.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_DataCargaInicialWebDto>(respDto.Datos, statusCode, respDto.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region Crud Obras y unidades constructivas
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPost("CrearObra", Name = "CrearObra")]
        [Consumes("application/json")]
        public async Task<IActionResult> CrearObra([FromBody] Op360ASEGRED_CrearObraDto op360ASEGRED_CrearObra)
        {
            try
            {
                var data = await _op360ASEGREDWebService.CrearObra(op360ASEGRED_CrearObra);
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

        #endregion

        #region CascaronesConModelo

        #region Crud Obras y unidades constructivas
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("GetObras", Name = "GetObras")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetObras()
        {
            try
            {
                var data = await _op360ASEGREDWebService.ListarObras();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_ListarObrasDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("EditarObra", Name = "EditarObra")]
        [Consumes("application/json")]
        public async Task<IActionResult> EditarObra([FromBody] Op360ASEGRED_EditarObraDto op360ASEGRED_EditarObra)
        {
            try
            {
                var data = await _op360ASEGREDWebService.EditarObra(op360ASEGRED_EditarObra);
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
        [HttpDelete("EliminarObra/{id}", Name = "EliminarObra")]
        [Consumes("application/json")]
        public async Task<IActionResult> EliminarObra([FromRoute] Op360ASEGRED_EliminarObraDto op360ASEGRED_EliminarObra)
        {
            try
            {
                var data = await _op360ASEGREDWebService.EliminarObra(op360ASEGRED_EliminarObra);
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
        [HttpGet("ConsultarObrasConFiltros", Name = "ConsultarObrasConFiltros")]
        [Consumes("application/json")]
        public async Task<IActionResult> ConsultarObrasConFiltros([FromQuery] Op360ASEGRED_ConsultarObrasConFiltrosDto op360ASEGRED_ConsultarObrasConFiltros)
        {
            try
            {
                var data = await _op360ASEGREDWebService.ConsultarObrasConFiltros(op360ASEGRED_ConsultarObrasConFiltros);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_ConsultarObrasConFiltrosResponseDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ListadoUnidadesConstructivasCerradas", Name = "ListadoUnidadesConstructivasCerradas")]
        [Consumes("application/json")]
        public async Task<IActionResult> ListadoUnidadesConstructivasCerradas(Op360ASEGRED_ListadoUnidadesConstructivasCerradasDto op360ASEGRED_ListadoUnidadesConstructivasCerradas)
        {
            try
            {
                var data = await _op360ASEGREDWebService.ListadoUnidadesConstructivasCerradas(op360ASEGRED_ListadoUnidadesConstructivasCerradas);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_ListadoUnidadesConstructivasCerradasResponseDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region CargueMasivoExcel
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPost("CargueMasivoExcel", Name = "CargueMasivoExcel")]
        [Consumes("application/json")]
        public async Task<IActionResult> CargueMasivoExcel([FromBody] Op360ASEGRED_CargueMasivoExcelDto op360ASEGRED_CargueMasivoExcel)
        {
            try
            {
                var data = await _op360ASEGREDWebService.CargueMasivoExel(op360ASEGRED_CargueMasivoExcel);
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
        [HttpDelete("EliminarCargueMasivoExcel/{id}", Name = "EliminarCargueMasivoExcel")]
        [Consumes("application/json")]
        public async Task<IActionResult> EliminarCargueMasivoExcel([FromRoute] Op360ASEGRED_EliminarCargueMasivoExcelDto op360ASEGRED_EliminarCargueMasivoExcel)
        {
            try
            {
                var data = await _op360ASEGREDWebService.EliminarCargueMasivoExcel(op360ASEGRED_EliminarCargueMasivoExcel);
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

        #region AsignarObraAliadoContratista
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("AsignarObraAliadoContratista", Name = "AsignarObraAliadoContratista")]
        [Consumes("application/json")]
        public async Task<IActionResult> AsignarObraAliadoContratista([FromBody] Op360ASEGRED_AsignarObraAliadoContratistaDto op360ASEGRED_AsignarObraAliadoContratista)
        {
            try
            {
                var data = await _op360ASEGREDWebService.AsignarObraAliadoContratista(op360ASEGRED_AsignarObraAliadoContratista);
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

        #region Estructuras
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ListarEstructura", Name = "ListarEstructura")]
        [Consumes("application/json")]
        public async Task<IActionResult> ListarEstructura()
        {
            try
            {
                var data = await _op360ASEGREDWebService.ListarEstructuras();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_ListarEstructurasDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("EditarEstrucutra", Name = "EditarEstrucutra")]
        [Consumes("application/json")]
        public async Task<IActionResult> EditarEstrucutra([FromBody] Op360ASEGRED_EditarEstructuraDto op360ASEGRED_EditarEstructura)
        {
            try
            {
                var data = await _op360ASEGREDWebService.EditarEstructura(op360ASEGRED_EditarEstructura);
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
        [HttpPost("AgregarEstructura", Name = "AgregarEstructura")]
        [Consumes("application/json")]
        public async Task<IActionResult> AgregarEstructura([FromBody] Op360ASEGRED_AgregarEstructuraDto op360ASEGRED_AgregarEstructura)
        {
            try
            {
                var data = await _op360ASEGREDWebService.AgregarEstructura(op360ASEGRED_AgregarEstructura);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpDelete("EliminarEstructura/{id}", Name = "EliminarEstructura")]
        [Consumes("application/json")]
        public async Task<IActionResult> EliminarEstructura([FromRoute] Op360ASEGRED_EliminarEstructuraDto op360ASEGRED_EliminarEstructura)
        {
            try
            {
                var data = await _op360ASEGREDWebService.EliminarEstructura(op360ASEGRED_EliminarEstructura);
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
        [HttpGet("ListarDetallesEstructura", Name = "ListarDetallesEstructura")]
        [Consumes("application/json")]
        public async Task<IActionResult> ListarDetallesEstructura()
        {
            try
            {
                var data = await _op360ASEGREDWebService.ListarDetalleEstructura();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_ListarDetalleEstructuraDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region GestionSoportes
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("ValidarSoportes", Name = "ValidarSoportes")]
        [Consumes("application/json")]
        public async Task<IActionResult> ValidarSoportes([FromBody] Op360ASEGRED_ValidarSoporteDto op360ASEGRED_ValidarSoporte)
        {
            try
            {
                var data = await _op360ASEGREDWebService.ValidarSoporte(op360ASEGRED_ValidarSoporte);
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
        [HttpPut("RechazarSoporte", Name = "RechazarSoporte")]
        [Consumes("application/json")]
        public async Task<IActionResult> RechazarSoporte([FromBody] Op360ASEGRED_RechazarSoporteDto op360ASEGRED_RechazarSoporte)
        {
            try
            {
                var data = await _op360ASEGREDWebService.RechazarSoporte(op360ASEGRED_RechazarSoporte);
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
        [HttpPost("CrearSoporte", Name = "CrearSoporte")]
        [Consumes("application/json")]
        public async Task<IActionResult> CrearSoporte([FromBody] Op360ASEGRED_CrearSoporteDto op360ASEGRED_CrearSoporte)
        {
            try
            {
                var data = await _op360ASEGREDWebService.CrearSoporte(op360ASEGRED_CrearSoporte);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Codigo);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpDelete("EliminarSoporte/{id}", Name = "EliminarSoporte")]
        [Consumes("application/json")]
        public async Task<IActionResult> EliminarSoporte([FromRoute] Op360ASEGRED_EliminarSoporteDto op360ASEGRED_EliminarSoporte)
        {
            try
            {
                var data = await _op360ASEGREDWebService.EliminarSoporte(op360ASEGRED_EliminarSoporte);
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

        #region Pre-Factura
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPost("NuevaPreFactura", Name = "NuevaPreFactura")]
        [Consumes("application/json")]
        public async Task<IActionResult> NuevaPreFactura([FromBody] Op360ASEGRED_NuevaPreFacturaDto op360ASEGRED_NuevaPreFactura)
        {
            try
            {
                var data = await _op360ASEGREDWebService.NuevaPreFactura(op360ASEGRED_NuevaPreFactura);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("HistorialPrefacturasParciales", Name = "HistorialPrefacturasParciales")]
        [Consumes("application/json")]
        public async Task<IActionResult> HistorialPrefacturasParciales()
        {
            try
            {
                var data = await _op360ASEGREDWebService.ListarHistorialPreFacturasParciales();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_ListarHistorialPreFacturasParcialesDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpDelete("EliminarPreFactura/{id}", Name = "EliminarPreFactura")]
        [Consumes("application/json")]
        public async Task<IActionResult> EliminarPreFactura([FromRoute] Op360ASEGRED_EliminarPreFacturaDto op360ASEGRED_EliminarPreFactura)
        {
            try
            {
                var data = await _op360ASEGREDWebService.EliminarPreFactura(op360ASEGRED_EliminarPreFactura);
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
        [HttpPut("EditarPreFactura", Name = "EditarPreFactura")]
        [Consumes("application/json")]
        public async Task<IActionResult> EditarPreFactura([FromBody] Op360ASEGRED_EditarPreFacturaDto op360ASEGRED_EditarPreFactura)
        {
            try
            {
                var data = await _op360ASEGREDWebService.EditarPreFactura(op360ASEGRED_EditarPreFactura);
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

        #region CacaronesSinModelo

        #region Crud Obras y unidades constructivas
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("CierreUnidadesConstructivas", Name = "CierreUnidadesConstructivas")]
        [Consumes("application/json")]
        public async Task<IActionResult> CierreUnidadesConstructivas([FromBody] Op360ASEGRED_CierreUnidadesConstructivasDto op360ASEGRED_CierreUnidadesConstructivas)
        {
            try
            {
                var data = await _op360ASEGREDWebService.CierreUnidadesConstructivas(op360ASEGRED_CierreUnidadesConstructivas);
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
        [HttpGet("ImprimirObra", Name = "ImprimirObra")]
        [Consumes("application/json")]
        public async Task<IActionResult> ImprimirObra()
        {
            try
            {
                var data = await _op360ASEGREDWebService.ImprimirObra();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_ImprimirObraDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        #endregion

        #region Soportes Obra
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPost("CargaSoportesObra", Name = "CargaSoportesObra")]
        [Consumes("application/json")]
        public async Task<IActionResult> CargaSoportesObra([FromBody] Op360ASEGRED_CargaSoportesObraDto op360ASEGRED_CargaSoportesObra)
        {
            try
            {
                var data = await _op360ASEGREDWebService.CargaSoportesObra(op360ASEGRED_CargaSoportesObra);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpDelete("EliminarSoportesObra/{id}", Name = "EliminarSoportesObra")]
        [Consumes("application/json")]
        public async Task<IActionResult> EliminarSoportesObra([FromRoute] Op360ASEGRED_EliminarSoportesObraDto op360ASEGRED_EliminarSoportesObra)
        {
            try
            {
                var data = await _op360ASEGREDWebService.EliminarSoportesObra(op360ASEGRED_EliminarSoportesObra);
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

        #region Estructuras
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ImprimirPoste", Name = "ImprimirPoste")]
        [Consumes("application/json")]
        public async Task<IActionResult> ImprimirPoste()
        {
            try
            {
                var data = await _op360ASEGREDWebService.ImprimirPoste();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_ImprimirPosteDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("AuditoriaPoste", Name = "AuditoriaPoste")]
        [Consumes("application/json")]
        public async Task<IActionResult> AuditoriaPoste()
        {
            try
            {
                var data = await _op360ASEGREDWebService.AuditoriaPoste();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_AuditoriaPosteDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ListarEstructurasXGeoreferencia", Name = "ListarEstructurasXGeoreferencia")]
        [Consumes("application/json")]
        public async Task<IActionResult> ListarEstructurasXGeoreferencia()
        {
            try
            {
                var data = await _op360ASEGREDWebService.ListarEstructurasPorGeoreferencia();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_ListarEstructurasPorGeoreferenciaDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region GestionSoportes
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("DescargaDeSoportes", Name = "DescargaDeSoportes")]
        [Consumes("application/json")]
        public async Task<IActionResult> DescargaDeSoportes()
        {
            try
            {
                var data = await _op360ASEGREDWebService.DescargarSoportes();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_DescargarSoportesDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("DescargarDisenioObra", Name = "DescargarDisenioObra")]
        [Consumes("application/json")]
        public async Task<IActionResult> DescargarDisenioObra()
        {
            try
            {
                var data = await _op360ASEGREDWebService.DescargarDisenioObra();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_DescargarDisenioObraDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region Ordenes
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("PreLiquidarOrden", Name = "PreLiquidarOrden")]
        [Consumes("application/json")]
        public async Task<IActionResult> PreLiquidarOrden([FromBody] Op360ASEGRED_PreLiquidarOrdenDto op360ASEGRED_PreLiquidarOrden)
        {
            try
            {
                var data = await _op360ASEGREDWebService.PreLiquidarOrden(op360ASEGRED_PreLiquidarOrden);
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
        [HttpGet("ImprimirOrden", Name = "ImprimirOrden")]
        [Consumes("application/json")]
        public async Task<IActionResult> ImprimirOrden()
        {
            try
            {
                var data = await _op360ASEGREDWebService.ImprimirOrden();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_ImprimirOrdenDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region Pre-Factura
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("DescargarPreFactura", Name = "DescargarPreFactura")]
        [Consumes("application/json")]
        public async Task<IActionResult> DescargarPreFactura()
        {
            try
            {
                var data = await _op360ASEGREDWebService.DescargarPreFactura();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_DescargarPreFacturaDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("DescargarPosiblePreFactura", Name = "DescargarPosiblePreFactura")]
        [Consumes("application/json")]
        public async Task<IActionResult> DescargarPosiblePreFactura()
        {
            try
            {
                var data = await _op360ASEGREDWebService.DescargarPosiblePreFactura();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_DescargarPosiblePreFacturaDto>(data.Datos, statusCode, data.Mensaje);

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
