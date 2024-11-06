using Api.Responses;
using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Enumerations;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelResponse;
using Core.Options;
using Core.QueryFilters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize(Policy = "ShouldBeAnAdmin")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class AdobeSignController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly PathOptions _pathOptions;
        private readonly IAdobeSignService _adobeSignService;

        public AdobeSignController(
            IConfiguration configuration,
            IOptions<PathOptions> pathOptions,
            IAdobeSignService adobeSignService
        )
        {
            _configuration = configuration;
            _pathOptions = pathOptions.Value;
            _adobeSignService = adobeSignService;
        }

        /*[HttpGet("PruebaAdobe", Name = "PruebaAdobe")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> PruebaAdobe()
        {
            try
            {
                //var obj = await _adobeSignService.TransientDocumentsAsync(@"\\syspotecdata.file.core.windows.net\portal-proveedores\1-MERCADEO.pdf");
                var obj = await _adobeSignService.ValdiateDocumentAsync("CBJCHBCAABAA5TADCANnPk57IxSww0RDctrhwsTps_-4");
                //var obj = await _adobeSignService.DownloadDocumentAsync("CBJCHBCAABAAfFSs0d6wwoKMt80PQyvxO7o8RvLmIDrP");
                //MemoryStream ms = new(obj.Item1);
                //return new FileStreamResult(ms, obj.Item2);
                return Ok(obj);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }*/

        /// <summary>
        /// Método para generar la descarga de documento firmado desde Adobe Sign
        /// </summary>
        /// <param name="queryDownload"></param>
        /// <returns></returns>
        [HttpPost("DownloadPdfAdobe", Name = "DescargarPdfAdobeSign")]
        [Consumes("application/json")]
        public async Task<IActionResult> DescargarPdfAdobeSign([FromBody] QueryDownloadPdfAdb queryDownload)
        {
            try
            {
                _adobeSignService.Tipo_Agreement = queryDownload.Tipo_Acuerdo;
                string pathFS = string.Empty;
                switch (queryDownload.Tipo_Acuerdo)
                {
                    case AdbSign_TipoAcuerdo.Contrato:
                        pathFS = Path.Combine(_pathOptions.Path_FileServer_root, _pathOptions.Folder_Archivos_Contrato, queryDownload.Id.ToString());
                        break;
                    case AdbSign_TipoAcuerdo.Proveedor:
                        pathFS = _pathOptions.Path_FileServer;
                        break;
                    default:
                        break;
                }
                bool wasCreated = await _adobeSignService.DownloadDocumentAsync(queryDownload, pathFS, _pathOptions.IdPathFileServer);
                List<ResponseAction> responseActions = new()
                {
                    new()
                    {
                        estado = wasCreated,
                        mensaje = wasCreated ? "Documento descargado exitosamente!" : "Hubo inconvenientes para crear el archivo en el File Server" // Mensaje para el usuario
                    }
                };
                var response = new ApiResponse<List<ResponseAction>>(responseActions, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error. Detalle: {e.Message}");
            }
        }
    }
}