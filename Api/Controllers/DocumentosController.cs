namespace Api.Controllers
{
    using Api.Responses;
    using AutoMapper;
    using Core.DTOs;
    using Core.Entities;
    using Core.Exceptions;
    using Core.Interfaces;
    using Core.ModelResponse;
    using Infrastructure.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Authorize(Policy = "ShouldBeAnAdminOrProv")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class DocumentosController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDocumentoService _documentoService;

        public DocumentosController(   
            IMapper mapper,
            IDocumentoService documentoService
        )
        {
            _mapper = mapper;
            _documentoService = documentoService;
        }

        /// <summary>
        /// Método para consultar documentos.
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        [HttpGet("Search", Name = "SearchDocumentos")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchDocumentos()
        {
            try
            {
                List<Documento> list = await _documentoService.GetDocumentos();
                if (list is null)
                    list = new List<Documento>();
                List<DocumentoDto> documentos = _mapper.Map<List<DocumentoDto>>(list);
                var response = new ApiResponse<List<DocumentoDto>>(documentos, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Método para consultar documento específico.
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        [HttpGet("SearchById", Name = "SearchDocumentosById")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchDocumentosById(int idReq)
        {
            try
            {
                List<Documento> list = await _documentoService.GetDocumento(idReq);
                if (list is null)
                    list = new List<Documento>();
                List<DocumentoDto> documentos = _mapper.Map<List<DocumentoDto>>(list);
                var response = new ApiResponse<List<DocumentoDto>>(documentos, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Método para crear documentos.
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "ShouldBeAnAdmin")]
        [HttpPost("Create", Name = "CrearDocumento")]
        [Consumes("application/json")]
        public async Task<IActionResult> CrearDocumento([FromBody] DocumentoDto documento)
        {
            try
            {
                documento.CodUser = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                List<ResponseAction> list = await _documentoService.PostCrear(documento);
                var response = new ApiResponse<List<ResponseAction>>(list, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Método para actualizar documentos.
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "ShouldBeAnAdmin")]
        [HttpPut("Update", Name = "ActualizarDocumento")]
        [Consumes("application/json")]
        public async Task<IActionResult> ActualizarDocumento([FromBody] DocumentoDto documento)
        {
            try
            {
                documento.CodUserUpdate = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                List<ResponseAction> list = await _documentoService.PutActualizar(documento);
                var response = new ApiResponse<List<ResponseAction>>(list, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Método para eliminar documentos.
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "ShouldBeAnAdmin")]
        [HttpDelete("Delete", Name = "EliminarDocumento")]
        [Consumes("application/json")]
        public async Task<IActionResult> EliminarDocumento([FromBody] DocumentoDto documento)
        {
            try
            {
                documento.CodUserUpdate = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                List<ResponseAction> list = await _documentoService.Delete(documento);
                var response = new ApiResponse<List<ResponseAction>>(list, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
    }
}