using Api.Responses;
using AutoMapper;
using Core.CustomEntities;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelResponse;
using Core.QueryFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class CertificadosEspecialesController : ControllerBase
    {
        private readonly ICertificadosEspecialesService _certificadosEspecialesService;
        private readonly IReportingService _reportingService;
        private readonly IMapper _mapper;

        public CertificadosEspecialesController(
            ICertificadosEspecialesService certificadosEspecialesService, 
            IMapper mapper, 
            IReportingService reportingService
        )
        {
            _certificadosEspecialesService = certificadosEspecialesService;
            _reportingService = reportingService;
            _mapper = mapper;
        }

        /// <summary>
        /// Método para consultar los certificados especiales
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchAll", Name = "SearchAllCertificadosEspeciales")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdmin")]
        public async Task<IActionResult> SearchAllCertificadosEspeciales()
        {
            try
            {
                List<CertificadosEspeciale> entities = await _certificadosEspecialesService.GetCertificadosEspeciales();
                var proveedoresDto = _mapper.Map<List<CertificadosEspecialesDto>>(entities);

                var response = new ApiResponse<List<CertificadosEspecialesDto>>(proveedoresDto, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para consultar los certificados de un proveedor especifico
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchAllProveedor", Name = "SearchAllCertificadosEspecialesProveedor")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        public async Task<IActionResult> SearchAllCertificadosEspecialesProveedor()
        {
            try
            {
                string user = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                List<CertificadosEspeciale> entities = await _certificadosEspecialesService.GetCertificadosEspecialesProveedor(user);
                var proveedoresDto = _mapper.Map<List<CertificadosEspecialesDto>>(entities);

                var response = new ApiResponse<List<CertificadosEspecialesDto>>(proveedoresDto, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para consultar certificados por medio del filtro.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("SearchFiltro", Name = "SearchCertificadosEspecialesFiltro")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        public async Task<IActionResult> SearchCertificadosEspecialesFiltro([FromQuery] QuerySearchFiltroCertificados query)
        {
            try
            {
                query.CodUser = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                List<CertificadosEspeciale> entities = await _certificadosEspecialesService.GetCertificadosEspecialesFiltro(query);
                var proveedoresDto = _mapper.Map<List<CertificadosEspecialesDto>>(entities);

                var response = new ApiResponse<List<CertificadosEspecialesDto>>(proveedoresDto, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para consultar un Certificado especial en especifico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("SearchDetalle", Name = "SearchCertificadoDetalle")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        public async Task<IActionResult> SearchCertificadoDetalle(int id)
        {
            try
            {
                CertificadoEspecialDetalle list = await _certificadosEspecialesService.GetCertificadoDetallePorId(id);
                var response = new ApiResponse<CertificadoEspecialDetalle>(list, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para consultar respuestas generadas por el gestor de certificados, de un certificado especifico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("SearchRespuestas", Name = "SearchRespuestasCertificado")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        public async Task<IActionResult> SearchRespuestasCertificado(int id)
        {
            try
            {
                List<RespuestasCertEspeciale> list = await _certificadosEspecialesService.GetRespuestasCertificadoPorID(id);
                var listDto = _mapper.Map<List<RespuestasCertEspecialeDto>>(list);
                var response = new ApiResponse<List<RespuestasCertEspecialeDto>>(listDto, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para consultar los parámtetros iniciales del modulo de certificados
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchParametrosIniciales", Name = "SearchParametrosIniciales")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        public async Task<IActionResult> SearchParametrosIniciales()
        {
            try
            {
                int user = int.Parse(HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0");
                string tipoUsuario = HttpContext.Items.ContainsKey("UserRole") ? HttpContext.Items["UserRole"]?.ToString() : "";
                ParametrosCertificados entities = await _certificadosEspecialesService.GetParametrosIniciales(user, tipoUsuario);
                //var proveedoresDto = _mapper.Map<ParametrosCertificados>(entities);

                var response = new ApiResponse<ParametrosCertificados>(entities, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para crear certificados especiales
        /// </summary>
        /// <param name="especialesDto"></param>
        /// <returns></returns>
        [HttpPost("Create", Name = "CrearCertificadoEspecial")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        public async Task<IActionResult> CrearCertificadoEspecial([FromBody] CertificadosEspecialesDto especialesDto)
        {
            try
            {
                especialesDto.CodUser = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                //CertificadosEspeciale certEspecialesDto = _mapper.Map<CertificadosEspeciale>(especialesDto);
                var responseAction = await _certificadosEspecialesService.CrearCertificadoEspecial(especialesDto);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);

                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para actualizar Certificados Especiales
        /// </summary>
        /// <param name="especialesDto"></param>
        /// <returns></returns>
        [HttpPut("Update", Name = "UpdateCertificadoEspecial")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        public async Task<IActionResult> UpdateCertificadoEspecial([FromBody] CertificadosEspecialesDto especialesDto)
        {
            try
            {
                especialesDto.CodUserUpdate = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                //CertificadosEspeciale certEspecialesDto = _mapper.Map<CertificadosEspeciale>(especialesDto);
                var responseAction = await _certificadosEspecialesService.UpdateCertificadoEspecial(especialesDto);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);

                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para actualizar el estado de un certificado específico
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPut("UpdateEstado", Name = "UpdateEstadoCertificadoEspecial")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        public async Task<IActionResult> UpdateEstadoCertificadoEspecial([FromBody] QueryUpdateEstadoCertificado parameters)
        {
            try
            {
                parameters.CodUserUpdate = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                var responseAction = await _certificadosEspecialesService.UpdateEstadoCertificadoEspecial(parameters);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);

                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para descargar certificado especial
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GenerateCertificadoEspecialPdf", Name = "DescargarCertificadoEspecialPdf")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        public async Task<IActionResult> DescargarCertificadoEspecialPdf(int id)
        {
            try
            {
                var ms = await _reportingService.CertificadoEspecialPdf(id);
                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = "certificado.pdf"
                };
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para descargar certificado de experiencia
        /// </summary>
        /// <returns></returns>
        [HttpGet("GenerateCertificadoExperienciaPdf", Name = "DescargarCertificadoExperienciaPdf")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        public async Task<IActionResult> DescargarCertificadoExperienciaPdf([FromQuery] QuerySearchCertificados parameters)
        {
            try
            {
                parameters.CodUser = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                parameters.TipoUsuario = HttpContext.Items.ContainsKey("UserRole") ? HttpContext.Items["UserRole"]?.ToString() : "";
                if (parameters.TipoUsuario == "Proveedor")
                {
                    parameters.NitProveedor = HttpContext.Items.ContainsKey("CodUser") ? HttpContext.Items["CodUser"]?.ToString() : "";
                }
                var ms = await _reportingService.CertificadoExperiencia(parameters);
                return new FileStreamResult(ms, "application/pdf")
                {
                    FileDownloadName = "certificadoExperiencia.pdf"
                };
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }
    }
}
