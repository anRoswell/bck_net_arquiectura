using Api.Responses;
using Azure;
using Core.CustomEntities;
using Core.DTOs;
using Core.DTOs.FilesDto;
using Core.DTOs.Gos.Mobile.ObtenerGestionesByGestorDto;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelResponse;
using Core.QueryFilters;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class Op360FilesController : ControllerBase
    {
        private readonly IFilesProcess _filesProcess;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOp360ReportingService _op360ReportingService;

        public Op360FilesController(
            IFilesProcess filesProcess,
            IUnitOfWork unitOfWork,
            IOp360ReportingService op360ReportingService
            )
        {
            _filesProcess = filesProcess;
            _unitOfWork = unitOfWork;
            _op360ReportingService = op360ReportingService;
        }

        #region Files
        /// <summary>
        /// Guardar arhivos en el servidor via fromform
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpPost("SaveFiles", Name = "ManagedFiles")]
        [Consumes("multipart/form-data")]
        [Authorize(Policy = "ShouldBeAnAdminAreaCentral")]        
        public async Task<IActionResult> ManagedFiles([FromForm] QueryOp360Files parameters)
        {
            try
            {
                // Convertir a JSON                
                parameters.id_usuario = Int32.Parse(HttpContext.Items.ContainsKey("id_usuario") ? HttpContext.Items["id_usuario"]?.ToString() : "0");
                var filesReq = await _filesProcess.SaveFilesOracle(parameters);
                var response = new ApiResponse<List<FileResponseOracle>>(filesReq.Datos, filesReq.Codigo, filesReq.Mensaje);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Guardar archivos via base64 en el servidor
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpPost("SaveFilesBase64", Name = "ManagedFilesBase64")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminAreaCentral")]
        public async Task<IActionResult> ManagedFilesBase64([FromBody] QueryOp360FileBase64 parameters)
        {
            try
            {
                parameters.id_usuario = Int32.Parse(HttpContext.Items.ContainsKey("id_usuario") ? HttpContext.Items["id_usuario"]?.ToString() : "0");
                var filesReq = await _filesProcess.SaveFileOracleBase64_Scr(parameters);
                var response = new ApiResponse<FileResponseOracle>(filesReq.Datos, filesReq.Codigo, filesReq.Mensaje);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// obtener archivos tomando como base la tabla gnl_soportes via bytes
        /// </summary>
        /// <param name="parameters"></param>        
        /// <returns></returns>        
        [HttpGet("GetFiles", Name = "ManagedReturnFiles")]
        [Consumes("multipart/form-data")]
        //[Authorize(Policy = "ShouldBeAnAdminAreaCentral")]
        public async Task<IActionResult> ManagedReturnFiles([FromQuery] QueryOp360GetFiles parameters)
        {
            try
            {
                var filesReq = await _filesProcess.GetFileBytes(parameters);
                var statusCode = filesReq._file != null ? 200 : 400;
                return statusCode == 200 ? File(filesReq._file, filesReq.TypeMime, filesReq.Nombre) : BadRequest($"No se encontro el archivo {parameters.Id_Soporte} con ruta {parameters.Id_Ruta}");
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la descarga del excel. Detalle: {e.Message}");
            }
        }

        [HttpGet("GetLogoCasa", Name = "GetLogoCasa")]
        [Consumes("multipart/form-data")]        
        public async Task<IActionResult> GetLogoCasa()
        {
            try
            {
                var filesReq = await _filesProcess.ObtenerLogo();
                var statusCode = filesReq._file != null ? 200 : 400;
                return statusCode == 200 ? File(filesReq._file, filesReq.TypeMime, filesReq.Nombre) : BadRequest($"No se encontro el archivo");
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la descarga del excel. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// obtener archivos tomando como base la tabla gnl_soportes via base64
        /// </summary>
        /// <param name="parameters"></param>        
        /// <returns></returns>        
        [HttpGet("GetFilesBase64", Name = "ManagedReturnFilesBase64")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminAreaCentralTerritorial")]
        public async Task<IActionResult> ManagedReturnFilesBase64([FromQuery] QueryOp360GetFiles parameters)
        {
            try
            {
                var filesReq = await _filesProcess.GetFileBase64(parameters);
                var response = new ApiResponse<FileResponseOracleBase64>(filesReq.Datos, filesReq.Codigo, filesReq.Mensaje);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la descarga del excel. Detalle: {e.Message}");
            }
        }
        #endregion
        #region Plantillas
        /// <summary>
        /// Retorna archivo con plantilla en formato base64
        /// Carlos Vargas
        /// </summary>
        /// <param name="parameters"></param>        
        /// <returns></returns>        
        [HttpGet("GetPlantillaBase64", Name = "ManagedReturnFilesPlantillasBase64")]
        [Consumes("application/json")]        
        [AllowAnonymous]
        public async Task<IActionResult> ManagedReturnFilesPlantillasBase64([FromQuery] QueryOp360ObtenerGnlPlantilla parameters)
        {
            try
            {
                var filesReq = await _filesProcess.GetFilePlantillasBase64(parameters);
                var response = new ApiResponse<FileResponsePlantillasOracleBase64Dto>(filesReq.Datos, filesReq.Codigo == 0 ? 200: 400, filesReq.Mensaje);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la descarga de la plantilla. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Retorna archivo con plantilla en formato base64 para Cargue Masivo
        /// </summary>
        /// <param name="parameters"></param>        
        /// <returns></returns>   
        [HttpGet("GetPlantillaCMBase64", Name = "ManagedReturnFilesPlantillasCMBase64")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> ManagedReturnFilesPlantillasCMBase64([FromQuery] QueryOp360ObtenerGnlPlantilla parameters)
        {
            try
            {
                var filesReq = await _filesProcess.GetFilePlantillasCMBase64(parameters);                
                var response = new ApiResponse<FileResponseOracleBase64>(filesReq.Datos, filesReq.Codigo == 0 ? 200 : 400, filesReq.Mensaje);

                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la descarga de la plantilla. Detalle: {e.Message}");
            }
        }
        #endregion

        #region JasperReports
        /// <summary>
        /// Retorna archivos de la tabla gnl_plantilla para usarlos en JasperReports
        /// tales como el logo de la empresa, etc
        /// </summary>
        /// <param name="parameters"></param>        
        /// <returns></returns>        
        [HttpGet("GetPlantillaReporte", Name = "ManagedReturnFilesPlantillaReporte")]
        [Consumes("multipart/form-data")]
        [AllowAnonymous]
        public async Task<IActionResult> ManagedReturnFilesPlantillaReporte([FromQuery] QueryOp360ObtenerGnlPlantilla parameters)
        {
            try
            {
                ResponseDto<FileResponseReportesJasper> vfile = await _filesProcess.GetFilePlantillas(parameters);                
                return File(vfile.Datos.archivobyte, vfile.Datos.typemime, vfile.Datos.nombre_archivo);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la descarga de la plantilla. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Obtener archivos tomando como base la tabla gnl_soportes via bytes para usarlos en JasperReportss con diferente seguridad
        /// </summary>
        /// <param name="parameters"></param>        
        /// <returns></returns>        
        [HttpGet("GetFilesReporte", Name = "ManagedReturnFilesReporte")]
        [Consumes("multipart/form-data")]
        [AllowAnonymous]
        public async Task<IActionResult> ManagedReturnFilesReporte([FromQuery] QueryOp360ObtenerGnlPlantilla parameters)
        {
            try
            {
                ResponseDto<FileResponseReportesJasper> vfile = await _filesProcess.GetFileGnlSoporte(parameters);
                if (new[] { 400, 401, 402, 403, 404, 405 }.Contains(vfile.Codigo))
                {
                    return File(vfile.Datos.archivobyte, vfile.Datos.typemime, vfile.Datos.nombre_archivo);
                }
                else
                {
                    return PhysicalFile(vfile.Datos.url_interna, vfile.Datos.typemime, vfile.Datos.nombre_archivo);
                }                
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la descarga del archivo. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para generar archivos de reporte de JasperReports
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet("GetJasperReportPdf", Name = "ObtenerPdfJasperReports")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminAreaCentral")]
        public async Task<IActionResult> ObtenerPdfJasperReports([FromQuery] QueryJasperReportsParams parameters)
        {
            try
            {
                var reportFile = await _op360ReportingService.ObtenerJasperReportPdf(parameters);            
                if (reportFile.Response.IsSuccessStatusCode)
                {
                    return new FileStreamResult(reportFile.File, "application/pdf");
                }
                else
                {
                    return StatusCode((int)reportFile.StatusCode, reportFile.Mensaje);
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la descarga del pdf. Detalle: {e.Message}");
            }
        }

        [HttpGet("GetJasperReportPdfBase64", Name = "ObtenerActaOrdenSCRBase64")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminAreaCentral")]
        public async Task<IActionResult> ObtenerActaOrdenSCRBase64([FromQuery] QueryJasperReportsParams parameters)
        {
            try
            {
                var ms = await _op360ReportingService.ObtenerJasperReportPdf(parameters);
                ResponseDto<FileResponseOracleBase64> filesReq = await _filesProcess.GetFileActaOrdenSCRBase64(ms);                                
                var response = new ApiResponse<FileResponseOracleBase64>(filesReq.Datos, filesReq.Codigo, filesReq.Mensaje);

                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la descarga de la plantilla. Detalle: {e.Message}, {e.InnerException?.Message ?? ""}");
            }
        }
        #endregion
    }
}