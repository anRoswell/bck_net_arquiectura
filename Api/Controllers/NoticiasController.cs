using Api.Responses;
using AutoMapper;
using Core.CustomEntities;
using Core.DTOs;
using Core.DTOs.FilesDto;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelProcess;
using Core.ModelResponse;
using Core.QueryFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize(Policy = "ShouldBeAnAdmin")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class NoticiasController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IFilesProcess _filesProcess;
        private readonly INoticiaService _noticiaService;
        private readonly INoticiasDocService _noticiasDocService;
        private readonly IMapper _mapper;
        private string Operacion = "1";
        private string ConsultaNoticiasPorUsuarioYempresa = "9";
        private string NoUsuario = "0";

        public NoticiasController(
            IConfiguration configuration,
            IFilesProcess filesProcess,
            INoticiaService noticiaService,
            INoticiasDocService noticiasDocService,
            IMapper mapper
        )
        {
            _configuration = configuration;
            _filesProcess = filesProcess;
            _noticiaService = noticiaService;
            _noticiasDocService = noticiasDocService;
            _mapper = mapper;
        }

        /// <summary>
        /// Consultar Noticia con su detalle por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("SearchDetalle", Name = "SearchNoticiaDetalle")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchNoticiaDetalle(int id)
        {
            try
            {
                NoticiasDetalle list = await _noticiaService.GetNoticiaPorID(id);

                var response = new ApiResponse<NoticiasDetalle>(list, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Parametros para Noticias
        /// </summary>
        /// <returns></returns>
        [HttpGet("ParametrosNoticias", Name = "GetNoticias")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> GetParametros(int idNoticias)
        {
            try
            {
                string UserID = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                var noticias = await _noticiaService.GetParametros(idNoticias, UserID, Operacion);
                var response = new ApiResponse<ParametrosNoticias>(noticias, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Buscar Parametros de la Noticia
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchParametrosNoticias", Name = "SearchParametrosNoticias")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchParametrosNoticias()
        {
            var CodUser = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : NoUsuario;
            if (CodUser != NoUsuario)
                Operacion = ConsultaNoticiasPorUsuarioYempresa;
            
            ParametrosNoticias parametrosNoticias = await _noticiaService.ParametrosNoticias(Operacion, CodUser );
            ApiResponse<ParametrosNoticias> response = new(parametrosNoticias, 200);
            return Ok(response);
        }

        /// <summary>
        /// Método para crear noticias
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPost("Create", Name = "CreateNoticia")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateNoticia([FromForm] QueryCreateNoticia parametros)
        {
            try
            {
                NoticiaDto noticiaDto = JsonConvert.DeserializeObject<NoticiaDto>(parametros.Noticia);
                noticiaDto.CodUser = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";

                // Validator

                List<NoticiasDoc> listDocuments = new(); // listado de archivos que se guardarán en DB

                string fileFolder = _configuration.GetSection("Folder_Archivos_Noticias")?.Value;
                int idPathFS = int.Parse(_configuration.GetSection("IdPathFileServer")?.Value);

                // Guardar archivos FilePrincipal (Imagen Portada de Noticia)
                if (parametros.FilePrincipal != null)
                {
                    if (parametros.FilePrincipal.Count > 1)
                    {
                        return Ok(ErrorResponse.GetError(false, "Error, sólo puede haber un archivo de portada.", 400));
                    }

                    FormDataImagen dataFile = new()
                    {
                        Carpeta = fileFolder,
                        Files = parametros.FilePrincipal,
                        IdPathFileServer = idPathFS
                    };

                    //List<FileResponse> filesReq = await _filesProcess.GetFilesCreated(dataFile);
                    List<FileResponse> filesReq = new() { };

                    // Procesamos los archivos para sacar la informacion de cada uno y guardar en la BD
                    foreach (var item in filesReq)
                    {
                        string nombre = item.NombreOriginal;
                        string nombreDocumento = string.Concat(item.NombreOriginal, item.Extension);
                        int tamano = int.Parse(item.Size.ToString());

                        NoticiasDoc documento = new()
                        {

                            NotdSizeDocument = tamano,
                            NotdExtDocument = item.Extension,
                            NotdUrlDocument = item.PathWebAbsolute,
                            NotdUrlRelDocument = item.PathWebRelative,
                            NotdNameDocument = $"{item.NombreInterno}{item.Extension}",
                            NotdOriginalNameDocument = $"{nombreDocumento}"
                        };

                        listDocuments.Add(documento);
                    }
                }
                else
                {
                    return Ok(ErrorResponse.GetError(false, "Error, al menos debe haber un archivo portada.", 400));
                }

                // Guardar archivos FilesSecondary
                /*if (parametros.FilesSecondary != null)
                {
                    FormDataImagen dataFile = new()
                    {
                        Carpeta = fileFolder,
                        Files = parametros.FilesSecondary,
                        IdPathFileServer = idPathFS
                    };

                    List<FileResponse> filesReq = await _filesProcess.GetFilesCreated(dataFile);

                    // Procesamos los archivos para sacar la informacion de cada uno y guardar en la BD
                    foreach (var item in filesReq)
                    {
                        string nombre = item.NombreOriginal;
                        string nombreDocumento = string.Concat(item.NombreOriginal, item.Extension);
                        int tamano = int.Parse(item.Size.ToString());

                        NoticiasDoc documento = new()
                        {
                            
                            NotdSizeDocument = tamano,
                            NotdExtDocument = item.Extension,
                            NotdUrlDocument = item.PathWebAbsolute,
                            NotdUrlRelDocument = item.PathWebRelative,
                            NotdNameDocument = $"{item.NombreInterno}{item.Extension}",
                            NotdOriginalNameDocument = $"{nombreDocumento}",
                            NotdDocumentoPrincipal = false
                        };

                        listDocuments.Add(documento);
                    }
                }*/

                Noticia entity = _mapper.Map<Noticia>(noticiaDto);
                var responseAction = await _noticiaService.PostNoticia(entity, listDocuments);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error al intentar insertar registro. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para actualizar noticias
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPut("Update", Name = "UpdateNoticia")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateNoticia([FromForm] QueryCreateNoticia parametros)
        {
            try
            {
                NoticiaDto noticiaDto = JsonConvert.DeserializeObject<NoticiaDto>(parametros.Noticia);
                noticiaDto.CodUserUpdate = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";

                // Validator
                
                List<NoticiasDoc> listDocuments = new(); // listado de archivos que se guardarán en DB

                // Guardar archivos FilePrincipal (Imagen Portada de Noticia)
                if (parametros.FilePrincipal != null)
                {

                    string fileFolder = _configuration.GetSection("Folder_Archivos_Noticias")?.Value;
                    int idPathFS = int.Parse(_configuration.GetSection("IdPathFileServer")?.Value);

                    if (parametros.FilePrincipal.Count > 1)
                    {
                        return Ok(ErrorResponse.GetError(false, "Error, sólo puede haber un archivo de portada.", 400));
                    }

                    FormDataImagen dataFile = new()
                    {
                        Carpeta = fileFolder,
                        Files = parametros.FilePrincipal,
                        IdPathFileServer = idPathFS
                    };

                    //List<FileResponse> filesReq = await _filesProcess.GetFilesCreated(dataFile);
                    List<FileResponse> filesReq = new() { };

                    // Procesamos los archivos para sacar la informacion de cada uno y guardar en la BD
                    foreach (var item in filesReq)
                    {
                        string nombre = item.NombreOriginal;
                        string nombreDocumento = string.Concat(item.NombreOriginal, item.Extension);
                        int tamano = int.Parse(item.Size.ToString());

                        NoticiasDoc documento = new()
                        {

                            NotdSizeDocument = tamano,
                            NotdExtDocument = item.Extension,
                            NotdUrlDocument = item.PathWebAbsolute,
                            NotdUrlRelDocument = item.PathWebRelative,
                            NotdNameDocument = $"{item.NombreInterno}{item.Extension}",
                            NotdOriginalNameDocument = $"{nombreDocumento}"
                        };

                        listDocuments.Add(documento);
                    }
                }

                Noticia entity = _mapper.Map<Noticia>(noticiaDto);
                var responseAction = await _noticiaService.PutNoticia(entity, listDocuments);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error al intentar insertar registro. Detalle: {e.Message}");
            }
        }

        /*[HttpPut("UpdateDocuments", Name = "UpdateDocumentosNoticia")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateDocumentosNoticia([FromForm] QueryUpdateDocsNoticia parameters)
        {
            try
            {
                // Validator

                // Guardar archivos FilesRequired
                if (parameters.Files != null)
                {
                    string fileFolder = _configuration.GetSection("Folder_Archivos_Noticias")?.Value;
                    int idPathFS = int.Parse(_configuration.GetSection("IdPathFileServer")?.Value);

                    FormDataImagen dataFile = new()
                    {
                        Carpeta = fileFolder,
                        Files = parameters.Files,
                        IdPathFileServer = idPathFS
                    };

                    List<FileResponse> filesReq = await _filesProcess.GetFilesCreated(dataFile);

                    // Procesamos los archivos para sacar la informacion de cada uno y guardar en la BD
                    foreach (var item in filesReq)
                    {
                        string nombre = item.NombreOriginal;
                        string nombreDocumento = string.Concat(item.NombreOriginal, item.Extension);
                        int tamano = int.Parse(item.Size.ToString());

                        NoticiasDoc documento = new()
                        {

                            NotdSizeDocument = tamano,
                            NotdExtDocument = item.Extension,
                            NotdUrlDocument = item.PathWebAbsolute,
                            NotdUrlRelDocument = item.PathWebRelative,
                            NotdNameDocument = $"{item.NombreInterno}{item.Extension}",
                            NotdOriginalNameDocument = $"{nombreDocumento}"
                        };

                        listDocuments.Add(documento);
                    }
                }

                Noticia entity = _mapper.Map<Noticia>(noticiaDto);
                var responseAction = await _noticiaService.PutNoticia(entity);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error al intentar insertar registro. Detalle: {e.Message}");
            }
        }*/

        /// <summary>
        /// Eliminar Noticia
        /// </summary>
        /// <param name="noticia"></param>
        /// <returns></returns>
        [HttpDelete("Delete", Name = "DeleteNoticia")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteNoticia(Noticia noticia)
        {
            try
            {
                var responseAction = await _noticiaService.DeleteNoticia(noticia);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la eliminación del registro. Detalle: {e.Message}");
            }
        }

        #region Gestion de Imágenes de la notica
        /*[HttpPost("AddImage", Name = "AgregarImagenNoticia")]
        [Consumes("application/json")]
        public async Task<IActionResult> AgregarImagenNoticia([FromBody] QueryAddImagenNoticia query)
        {
            try
            {
                query.CodUser = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";

                if (query.Files == null)
                {
                    return Ok(ErrorResponse.GetError(false, "Error, al menos debe haber un archivo.", 400));
                }

                var responseAction = await _noticiasDocService.AgregarImagenNoticia(query);
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
        }*/

        /// <summary>
        /// Método para eliminar una imagen de una noticia en especifico
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpDelete("DeleteImage", Name = "DeleteImagenNoticia")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteImagenNoticia([FromQuery] QueryDeleteImagenNoticia query)
        {
            try
            {
                query.CodUserUpdate = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                var responseAction = await _noticiasDocService.DeleteImagenNoticia(query);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la eliminación del registro. Detalle: {e.Message}");
            }
        }
        #endregion
    }
}
