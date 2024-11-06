using Api.Responses;
using AutoMapper;
using Core.CustomEntities;
using Core.CustomEntities.Parametros;
using Core.DTOs;
using Core.DTOs.FilesDto;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelProcess;
using Core.ModelResponse;
using Core.Options;
using Core.QueryFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize(Policy = "ShouldBeAnAdminOrProv")]
    [ApiController]
    //Controlador
    public class RequerimientosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IRequerimientosService _requerimientosService;
        private readonly IPrvDocumentoService _prvDocumentoService;
        private readonly IFilesProcess _filesProcess;
        private readonly IMapper _mapper;
        private readonly PathOptions _pathOptions;

        public RequerimientosController(
            IConfiguration configuration,
            IOptions<PathOptions> pathOptions,
            IFilesProcess filesProcess,
            IMapper mapper,
            IRequerimientosService requerimientosService,
            IPrvDocumentoService prvDocumentoService
        )
        {
            _configuration = configuration;
            _pathOptions = pathOptions.Value;
            _filesProcess = filesProcess;
            _requerimientosService = requerimientosService;
            _mapper = mapper;
            _prvDocumentoService = prvDocumentoService;
        }

        /// <summary>
        /// Buscar Parametros
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchParametrosRequerimientos", Name = "SearchParametrosRequerimientos")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchParametrosRequerimientos([FromQuery] QueryParametersRequerimientos parameters)
        {
            parameters.CodUser = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
            ParametrosRequerimientos listConsulta = await _requerimientosService.GetParametrosRequerimientos(parameters);
            ParametrosRequerimientosDto parametrosRequerimientosDto = _mapper.Map<ParametrosRequerimientosDto>(listConsulta);
            var response = new ApiResponse<ParametrosRequerimientosDto>(parametrosRequerimientosDto, 200);
            return Ok(response);
        }

        /// <summary>
        /// Buscar Parametros del Proveedor
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchParametrosRequerimientosPrv", Name = "SearchParametrosRequerimientosPrv")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchParametrosRequerimientosPrv([FromQuery] QueryParametersRequerimientos parameters)
        {
            parameters.CodUser = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
            ParamsInicialesReqPrv listConsulta = await _requerimientosService.GetParametrosRequerimientosPrv(parameters);
            var response = new ApiResponse<ParamsInicialesReqPrv>(listConsulta, 200);
            return Ok(response);
        }

        /// <summary>
        /// Buscar Requerimeintos filtros
        /// </summary>
        /// <returns></returns>
        [HttpGet("FiltroRequerimiento", Name = "GetFiltroRequerimiento")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetFiltroRequerimiento(int Id, int Estado, int Empresa, DateTime FechaInicio, DateTime FechaFin, int Categoria)
        {
            try
            {
                var requerimientos = await _requerimientosService.GetFiltroRequerimiento( Id,  Estado,  Empresa,  FechaInicio,  FechaFin,  Categoria);
                var response = new ApiResponse<List<Requerimientos>>(requerimientos, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Buscar Requerimientos
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchRequerimientos", Name = "SearchRequerimientos")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchRequerimientos()
        {
            List<Requerimientos> listConsulta = await _requerimientosService.GetRequerimientos();
            var response = new ApiResponse<List<Requerimientos>>(listConsulta, 200);
            return Ok(response);
        }

        /// <summary>
        /// Método para consultar los participantes de un requerimiento especifico.
        /// </summary>
        /// <param name="idRequerimiento"></param>
        /// <returns></returns>
        [HttpGet("SearchParticipantesRequerimiento", Name = "SearchParticipantesRequerimiento")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchParticipantesRequerimiento(int idRequerimiento)
        {
            List<Proveedores> listConsulta = await _requerimientosService.GetParticipantesRequerimiento(idRequerimiento);
            List<ProveedorDto> provDto = _mapper.Map<List<ProveedorDto>>(listConsulta);
            var response = new ApiResponse<List<ProveedorDto>>(provDto, 200);
            return Ok(response);
        }

        /// <summary>
        /// Buscar Requerimientos Adjudicados
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchRequerimientosAdjudi", Name = "SearchRequerimientosAdjuticados")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchRequerimientosAdjuticados()
        {
            int user = int.Parse(HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0");

            List<Requerimientos> listConsulta = await _requerimientosService.GetRequerimientosAdjudicados(user);
            var response = new ApiResponse<List<Requerimientos>>(listConsulta, 200);
            return Ok(response);
        }

        /// <summary>
        /// Consultar Requerimiento con su detalle por id
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpGet("SearchDetalle", Name = "SearchRequerimientoDetalle")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchRequerimientoDetalle([FromQuery] QuerySearchRequerimientos parametros)
        {
            try
            {
                parametros.CodUser = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                var list = await _requerimientosService.GetRequerimientoDetallePorID(parametros);
                var response = new ApiResponse<RequerimientoDetalle>(list, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para consultar comparativo de participantes de un requerimiento.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpGet("SearchComparativo", Name = "SearchComparativoRequerimiento")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchComparativoRequerimiento([FromQuery] QuerySearchComparativaReq parametros)
        {
            try
            {
                var list = await _requerimientosService.GetRequerimientoComparativo(parametros, _pathOptions);
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
        /// Método para consultar detalle de requerimiento en adjudicación.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpGet("SearchAdjudicacion", Name = "SearchAdjudicacionRequerimiento")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchAdjudicacionRequerimiento([FromQuery] QuerySearchComparativaReq parametros)
        {
            try
            {
                var list = await _requerimientosService.GetRequerimientoAdjudicacion(parametros, _pathOptions);
                if (list is null)
                    list = new List<RequerimientoAdjudicacionDto>();
                var response = new ApiResponse<List<RequerimientoAdjudicacionDto>>(list, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para consultar detalle de requerimiento en adjudicación, relacionando Órdenes de compras en los articulos. [NO SE ESTA USANDO]
        /// </summary>
        /// <param name="idRequerimiento"></param>
        /// <returns></returns>
        [HttpGet("SearchAdjudicacionWithOrdenes", Name = "SearchAdjudicacionWithOrdenes")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchAdjudicacionWithOrdenes([FromQuery] int idRequerimiento)
        {
            try
            {
                List<AdjudicacionOrdenes> list = await _requerimientosService.GetAdjudicacionRequerimientoOrden(idRequerimiento);
                if (list is null)
                    list = new List<AdjudicacionOrdenes>();
                var response = new ApiResponse<List<AdjudicacionOrdenes>>(list, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Buscar Requerimientos
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchRequerimientosSSR", Name = "SearchRequerimientosSSR")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchRequerimientosSSR()
        {
            List<Requerimientos> listConsulta = await _requerimientosService.GetRequerimientos();
            var response = new ApiResponse<List<Requerimientos>>(listConsulta, 200);
            return Ok(response);
        }

        /// <summary>
        /// Método para consultar las solicitudes existentes en Apoteosys y que no se hayan utilizado en Portal.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost("SearchArticulos", Name = "SearchArticulos")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchArticulos([FromBody] QuerySearchSolicitudesApoteosys parameters)
        {
            List<ArticulosRequerimiento> listConsulta = await _requerimientosService.GetArticulosParaRequerimiento(parameters);
            var response = new ApiResponse<List<ArticulosRequerimiento>>(listConsulta, 200);
            return Ok(response);
        }

        /// <summary>
        /// Crear Requerimiento
        /// </summary>   
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost("Create", Name = "CreateRequerimiento")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateRequerimiento([FromForm] QueryCreateRequerimiento parameters)
        {
            try
            {
                RequerimientoDto requerimiento = JsonConvert.DeserializeObject<RequerimientoDto>(parameters.Requerimiento);
                requerimiento.CodUser = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";

                // Validacion

                List<List<FileResponse>> fileListCreated = new();
                List<ReqArtSerRequeridosDoc> listDocuments = new(); // listado de archivos que se guardarán en DB

                if (parameters.Files != null)
                {
                    FormDataImagen dataFile = new()
                    {
                        Carpeta = _pathOptions.Folder_Archivos_Req,
                        Files = parameters.Files,
                        IdPathFileServer = _pathOptions.IdPathFileServer
                    };

                    //List<FileResponse> filesReq = await _filesProcess.GetFilesCreated(dataFile);
                    List<FileResponse> filesReq = new() { };

                    fileListCreated.Add(filesReq);

                    // Procesamos los articulos que traen archivos para sacar la informacion de cada uno y guardar en la BD
                    foreach (var item in filesReq)
                    {
                        int keyFile = int.Parse(item.NombreOriginal.Split("|")?[0]);
                        string nombreOriginal = item.NombreOriginal.Split("|")?[1];
                        string nombreDocumento = string.Concat(nombreOriginal, item.Extension);
                        int tamano = int.Parse(item.Size.ToString());

                        // Buscamos info adicional de los documentos
                        List<ReqArtSerRequeridosDto> docs = requerimiento.ReqArtSerRequeridos.FindAll(x => x.KeyFile == keyFile);

                        if (docs.Count > 1) // Quiere decir que son varios articulos agrupados
                        {
                            foreach (ReqArtSerRequeridosDto doc in docs)
                            {
                                ReqArtSerRequeridosDoc documento = new()
                                {
                                    RasrdSizeDocument = tamano,
                                    RasrdExtDocument = item.Extension,
                                    RasrdUrlDocument = item.PathWebAbsolute,
                                    RasrdUrlRelDocument = item.PathWebRelative,
                                    RasrdNameDocument = $"{item.NombreInterno}{item.Extension}",
                                    RasrdOriginalNameDocument = nombreDocumento,
                                    RasrdCodTipoDocArtSerRequeridos = doc.RasrTipoFichaTecnica,
                                    ItemArticulo = doc.RasrItem,
                                    RasrSolicitudApot = doc.RasrSolicitudApot,
                                    RasrCodigoArticuloApot = doc.RasrCodigoArticuloApot,
                                    RasrLineaApot = doc.RasrLineaApot,
                                    RasrEmpresaApot = doc.RasrEmpresaApot
                                };

                                listDocuments.Add(documento);
                            }
                        }
                        else // Quiere decir que es un articulo sin agrupar
                        {
                            ReqArtSerRequeridosDoc documento = new()
                            {
                                RasrdSizeDocument = tamano,
                                RasrdExtDocument = item.Extension,
                                RasrdUrlDocument = item.PathWebAbsolute,
                                RasrdUrlRelDocument = item.PathWebRelative,
                                RasrdNameDocument = $"{item.NombreInterno}{item.Extension}",
                                RasrdOriginalNameDocument = nombreDocumento,
                                RasrdCodTipoDocArtSerRequeridos = docs[0].RasrTipoFichaTecnica,
                                ItemArticulo = docs[0].RasrItem,
                                RasrSolicitudApot = docs[0].RasrSolicitudApot,
                                RasrCodigoArticuloApot = docs[0].RasrCodigoArticuloApot,
                                RasrLineaApot = docs[0].RasrLineaApot,
                                RasrEmpresaApot = docs[0].RasrEmpresaApot
                            };

                            listDocuments.Add(documento);
                        }
                    }

                    List<ReqArtSerRequeridosDto> otrosTipoDocumento = requerimiento.ReqArtSerRequeridos.FindAll(x => x.RasrTipoFichaTecnica == 2); // 2- Que sea de tipo URL

                    foreach (ReqArtSerRequeridosDto item in otrosTipoDocumento)
                    {
                        ReqArtSerRequeridosDoc otrosDocumentos = new()
                        {
                            RasrdCodTipoDocArtSerRequeridos = item.RasrTipoFichaTecnica,
                            RasrdUrlDocument = item.RasrFichaTecnica,
                            ItemArticulo = item.RasrItem,
                            RasrSolicitudApot = item.RasrSolicitudApot,
                            RasrCodigoArticuloApot = item.RasrCodigoArticuloApot,
                            RasrLineaApot = item.RasrLineaApot,
                            RasrEmpresaApot = item.RasrEmpresaApot
                        };

                        listDocuments.Add(otrosDocumentos);
                    }
                }
                else
                {
                    foreach (ReqArtSerRequeridosDto item in requerimiento.ReqArtSerRequeridos)
                    {
                        ReqArtSerRequeridosDoc otrosDocumentos = new()
                        {
                            RasrdCodTipoDocArtSerRequeridos = item.RasrTipoFichaTecnica,
                            RasrdUrlDocument = item.RasrFichaTecnica,
                            ItemArticulo = item.RasrItem,
                            RasrSolicitudApot = item.RasrSolicitudApot,
                            RasrCodigoArticuloApot = item.RasrCodigoArticuloApot,
                            RasrLineaApot = item.RasrLineaApot,
                            RasrEmpresaApot = item.RasrEmpresaApot
                        };

                        listDocuments.Add(otrosDocumentos);
                    }
                }

                List<ResponseAction> responseAction = await _requerimientosService.PostCrear(requerimiento, listDocuments);
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
        /// Actualizar Requerimientos
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPut("Update", Name = "UpdateRequerimientos")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateRequerimientos([FromForm] QueryCreateRequerimiento parameters)
        {
            try
            {
                var requerimiento = JsonConvert.DeserializeObject<RequerimientoDto>(parameters.Requerimiento);
                requerimiento.CodUser = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";

                // Validacion

                // Consultamos Paths de archivos de articulos del requerimiento
                List<PathArchivosArtSerReq> listDocs = await _requerimientosService.GetDocumentosTipoArchivoReq(requerimiento.Id);

                List<PathArchivosArtSerReq> listDocsAEliminarPorCambioArchivo = new();
                List<PathArchivosArtSerReq> listDocsAEliminarPorEliminacionArticulo = new();

                /*
                 * INICIO -- SECCION ANALISIS DE ARCHIVOS A ELIMINAR DEL FILE SERVER
                 */
                // Validamos si algun articulo se eliminó desde el front
                int[] articulosEliminados = listDocs.FindAll(x =>
                    !requerimiento.ReqArtSerRequeridos.Select(x => x.RasrIdReqArtSerRequeridos).Contains(x.RasrdCodReqArtSerRequeridos)
                ).Select(x => x.RasrdCodReqArtSerRequeridos).ToArray();

                listDocsAEliminarPorEliminacionArticulo = listDocs.FindAll(x => articulosEliminados.Contains(x.RasrdCodReqArtSerRequeridos));
                /*
                 * FIN -- SECCION ANALISIS DE ARCHIVOS A ELIMINAR DEL FILE SERVER
                 */

                List<List<FileResponse>> fileListCreated = new();
                List<ReqArtSerRequeridosDoc> listDocuments = new(); // listado de archivos que se guardarán en DB

                if (parameters.Files != null)
                {
                    FormDataImagen dataFile = new()
                    {
                        Carpeta = _pathOptions.Folder_Archivos_Req,
                        Files = parameters.Files,
                        IdPathFileServer = _pathOptions.IdPathFileServer
                    };

                    //List<FileResponse> filesReq = await _filesProcess.GetFilesCreated(dataFile);
                    List<FileResponse> filesReq = new() { };
                    fileListCreated.Add(filesReq);

                    // Procesamos los articulos que traen archivos para sacar la informacion de cada uno y guardar en la BD
                    foreach (var item in filesReq)
                    {
                        int keyFile = int.Parse(item.NombreOriginal.Split("|")?[0]);
                        string nombreOriginal = item.NombreOriginal.Split("|")?[1];
                        string nombreDocumento = string.Concat(nombreOriginal, item.Extension);
                        int tamano = int.Parse(item.Size.ToString());

                        // Buscamos info adicional de los documentos
                        List<ReqArtSerRequeridosDto> docs = requerimiento.ReqArtSerRequeridos.FindAll(x => x.KeyFile == keyFile);

                        if (docs.Count > 1) // Quiere decir que son varios articulos agrupados
                        {
                            foreach (ReqArtSerRequeridosDto doc in docs)
                            {
                                ReqArtSerRequeridosDoc documento = new()
                                {
                                    RasrdSizeDocument = tamano,
                                    RasrdExtDocument = item.Extension,
                                    RasrdUrlDocument = item.PathWebAbsolute,
                                    RasrdUrlRelDocument = item.PathWebRelative,
                                    RasrdNameDocument = $"{item.NombreInterno}{item.Extension}",
                                    RasrdOriginalNameDocument = nombreDocumento,
                                    RasrdCodTipoDocArtSerRequeridos = doc.RasrTipoFichaTecnica,
                                    ItemArticulo = doc.RasrItem,
                                    RasrSolicitudApot = doc.RasrSolicitudApot,
                                    RasrCodigoArticuloApot = doc.RasrCodigoArticuloApot,
                                    RasrLineaApot = doc.RasrLineaApot,
                                    RasrEmpresaApot = doc.RasrEmpresaApot
                                };

                                listDocuments.Add(documento);

                                // Validamos si alguno de los articulos se les modificó el archivo
                                foreach (var itemDoc in listDocs)
                                {
                                    if (doc.RasrIdReqArtSerRequeridos == itemDoc.RasrdCodReqArtSerRequeridos)
                                    {
                                        listDocsAEliminarPorCambioArchivo.Add(itemDoc);
                                    }
                                }
                            }
                        }
                        else // Quiere decir que es un articulo sin agrupar
                        {
                            ReqArtSerRequeridosDoc documento = new()
                            {
                                RasrdSizeDocument = tamano,
                                RasrdExtDocument = item.Extension,
                                RasrdUrlDocument = item.PathWebAbsolute,
                                RasrdUrlRelDocument = item.PathWebRelative,
                                RasrdNameDocument = $"{item.NombreInterno}{item.Extension}",
                                RasrdOriginalNameDocument = nombreDocumento,
                                RasrdCodTipoDocArtSerRequeridos = docs[0].RasrTipoFichaTecnica,
                                ItemArticulo = docs[0].RasrItem,
                                RasrSolicitudApot = docs[0].RasrSolicitudApot,
                                RasrCodigoArticuloApot = docs[0].RasrCodigoArticuloApot,
                                RasrLineaApot = docs[0].RasrLineaApot,
                                RasrEmpresaApot = docs[0].RasrEmpresaApot
                            };

                            listDocuments.Add(documento);

                            // Validamos si alguno de los articulos se les modificó el archivo
                            foreach (var itemDoc in listDocs)
                            {
                                if (docs[0].RasrIdReqArtSerRequeridos == itemDoc.RasrdCodReqArtSerRequeridos)
                                {
                                    listDocsAEliminarPorCambioArchivo.Add(itemDoc);
                                }
                            }
                        }
                    }

                    // Procesamos los articulos con Ficha Técnica de tipo URL
                    List<ReqArtSerRequeridosDto> otrosTipoDocumento = requerimiento.ReqArtSerRequeridos.FindAll(x => x.RasrTipoFichaTecnica == 2); // 2- Que sea de tipo URL

                    foreach (ReqArtSerRequeridosDto item in otrosTipoDocumento)
                    {
                        ReqArtSerRequeridosDoc otrosDocumentos = new()
                        {
                            RasrdCodTipoDocArtSerRequeridos = item.RasrTipoFichaTecnica,
                            RasrdUrlDocument = item.RasrFichaTecnica,
                            ItemArticulo = item.RasrItem,
                            RasrSolicitudApot = item.RasrSolicitudApot,
                            RasrCodigoArticuloApot = item.RasrCodigoArticuloApot,
                            RasrLineaApot = item.RasrLineaApot,
                            RasrEmpresaApot = item.RasrEmpresaApot
                        };

                        listDocuments.Add(otrosDocumentos);

                        // Validamos si alguno de los articulos se les quitó el archivo
                        foreach (var itemDoc in listDocs)
                        {
                            if (item.RasrIdReqArtSerRequeridos == itemDoc.RasrdCodReqArtSerRequeridos)
                            {
                                listDocsAEliminarPorCambioArchivo.Add(itemDoc);
                            }
                        }
                    }
                }
                else
                {
                    // Procesamos los articulos con Ficha Técnica de tipo URL
                    List<ReqArtSerRequeridosDto> urlsArticulos = requerimiento.ReqArtSerRequeridos.FindAll(x => x.RasrTipoFichaTecnica == 2); // 2- Que sea de tipo URL
                    foreach (ReqArtSerRequeridosDto item in urlsArticulos)
                    {
                        ReqArtSerRequeridosDoc otrosDocumentos = new()
                        {
                            RasrdCodTipoDocArtSerRequeridos = item.RasrTipoFichaTecnica,
                            RasrdUrlDocument = item.RasrFichaTecnica,
                            ItemArticulo = item.RasrItem,
                            RasrSolicitudApot = item.RasrSolicitudApot,
                            RasrCodigoArticuloApot = item.RasrCodigoArticuloApot,
                            RasrLineaApot = item.RasrLineaApot,
                            RasrEmpresaApot = item.RasrEmpresaApot
                        };

                        listDocuments.Add(otrosDocumentos);

                        // Validamos si alguno de los articulos se les quitó el archivo
                        foreach (var itemDoc in listDocs)
                        {
                            if (item.RasrIdReqArtSerRequeridos == itemDoc.RasrdCodReqArtSerRequeridos)
                            {
                                listDocsAEliminarPorCambioArchivo.Add(itemDoc);
                            }
                        }
                    }
                }

                var responseAction = await _requerimientosService.PutActualizar(requerimiento, listDocuments);
                if (responseAction[0].estado)
                {
                    // Eliminamos archivos de articulos que se les modificadó el archivo
                    foreach (var item in listDocsAEliminarPorCambioArchivo)
                    {
                        string path = Path.Combine(_pathOptions.Path_FileServer_root, item.RasrdUrlRelDocument);
                        //_filesProcess.RemoveFile(path);
                    }

                    // Eliminamos archivos de articulos eliminados
                    foreach (var item in listDocsAEliminarPorEliminacionArticulo)
                    {
                        string path = Path.Combine(_pathOptions.Path_FileServer_root, item.RasrdUrlRelDocument);
                        //_filesProcess.RemoveFile(path);
                    }
                }

                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la actualización del registro. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para Actualizar el estado del Requerimiento
        /// </summary>
        /// <param name="updateEstadoReq"></param>
        /// <returns></returns>
        [HttpPut("UpdateEstado", Name = "UpdateEstadoRequerimientos")]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateEstadoRequerimientos([FromBody] QueryUpdateEstadoReq updateEstadoReq)
        {
            try
            {
                updateEstadoReq.CodUserUpdate = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";

                var responseAction = await _requerimientosService.PutActualizarEstado(updateEstadoReq);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la actualización del registro. Detalle: {e.Message}");
            }
        }

        [HttpPost("PostNewComentario", Name = "PostNewComentario")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PostNewComentario([FromForm] QueryCreateComentario reqQuestionAnswer)
        {
            try
            {
                ReqQuestionAnswer comentario = JsonConvert.DeserializeObject<ReqQuestionAnswer>(reqQuestionAnswer.Comentario);
                comentario.CodUser = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                comentario.CodUserUpdate = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";

                #region Guardado de Documentos
                string fileFolder = _configuration.GetSection("Folder_Archivos")?.Value;
                int idPathFS = int.Parse(_configuration.GetSection("IdPathFileServer")?.Value);

                // Guardar archivos FilesRequired
                if (reqQuestionAnswer.Files != null)
                {
                    FormDataImagen dataFile = new()
                    {
                        Carpeta = fileFolder,
                        Files = reqQuestionAnswer.Files,
                        IdPathFileServer = idPathFS
                    };

                    //List<FileResponse> filesReq = await _filesProcess.GetFilesCreated(dataFile);
                    List<FileResponse> filesReq = new() { };

                    comentario.RqaFileExt = filesReq[0].Extension;
                    comentario.RqaFilePath = filesReq[0].PathWebAbsolute;
                    comentario.RqaFileSize = Convert.ToInt32(filesReq[0].Size);
                    comentario.RqaFileRelativo = filesReq[0].PathWebRelative;
                }
                else
                {
                    comentario.RqaFileExt = string.Empty;
                    comentario.RqaFilePath = string.Empty;
                    comentario.RqaFileSize = 0;
                    comentario.RqaFileRelativo = string.Empty;
                }

                var responseAction = await _requerimientosService.PostNewComentario(comentario);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
                #endregion
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la actualización del registro. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Crear participación
        /// </summary>   
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost("Participar", Name = "CreateParticipacion")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateParticipacion([FromForm] QueryParticipacion parameters)
        {
            List<PrvDocumento> listDocuments_Proveedor = new(); // listado de archivos que se guardarán en DB
            List<ReqDocumentosRequerido> listDocuments_Otros = new(); // listado de archivos que se guardarán en DB

            try
            {
                // Deserializamos Participacion
                ReqParticipantesDto participacion = JsonConvert.DeserializeObject<ReqParticipantesDto>(parameters.Participacion);
                participacion.CodUser = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";

                // Deserializamos DocumentosRequeridos
                List<ReqListDocumentosDto> listDocsReqParticipacion = JsonConvert.DeserializeObject<List<ReqListDocumentosDto>>(parameters.DocumentosRequeridos);
                List<int> idsDocsPrv = listDocsReqParticipacion.Where(x => x.CodDocumento != _pathOptions.Codigo_Documento_Otros).Select(x => x.CodDocumento).ToList();

                // Obtenemos los ids de ReqListDocumentos tanto para documentos del proveedor como documentos 'Otros'
                List<int> idsFilesPrv = listDocsReqParticipacion.Where(x => x.CodDocumento != _pathOptions.Codigo_Documento_Otros).Select(x => x.Id).ToList();
                List<int> idsFilesOtros = listDocsReqParticipacion.Where(x => x.CodDocumento == _pathOptions.Codigo_Documento_Otros).Select(x => x.Id).ToList();

                // Consultamos Paths de archivos de documentos del Proveedor
                List<PrvDocumento> listDocsPrvExistentes = await _prvDocumentoService.GetDocumentosProveedorReq(new ParamDocumentosPrvReq { 
                    CodProveedor = participacion.CodProveedor,
                    IdsDocumentos = idsDocsPrv
                });

                List<List<FileResponse>> fileListCreated = null;

                if (listDocsReqParticipacion.Any())
                {
                    if (parameters.FilesRequired != null)
                    {
                        string pathCarpetaOtros = Path.Combine(_pathOptions.Folder_Archivos_ParticipacionReq, participacion.CodRequerimento.ToString(), participacion.CodUser);
                        var documentosProveedor = parameters.FilesRequired.Where(x => idsFilesPrv.Contains(int.Parse(x.FileName.Split('|')[0])))?.ToList();
                        var documentosOtros = parameters.FilesRequired.Where(x => idsFilesOtros.Contains(int.Parse(x.FileName.Split('|')[0])))?.ToList();

                        if (documentosProveedor.Any())
                        {
                            // Creamos archivos del Proveedor
                            //fileListCreated = await _filesProcess.GuardarFileAsync(new SaveProcessFiles
                            //{
                            //    Files = documentosProveedor,
                            //    PathCarpeta = _pathOptions.Folder_Archivos,
                            //    PathOptions = _pathOptions
                            //});
                        }

                        if (documentosOtros.Any())
                        {
                            // Creamos archivos Otros
                            //fileListCreated = await _filesProcess.GuardarFileAsync(new SaveProcessFiles
                            //{
                            //    Files = documentosOtros,
                            //    PathCarpeta = pathCarpetaOtros,
                            //    PathOptions = _pathOptions
                            //});
                        }

                        // Procesamos los documentos que traen archivos para sacar la informacion de cada uno y guardar en la BD
                        foreach (var filesReq in fileListCreated)
                        {
                            foreach (var item in filesReq)
                            {
                                int idReqListDocumentos = int.Parse(item.NombreOriginal.Split("|")?[0]);
                                //string nombreOriginal = item.NombreOriginal.Split("|")?[1];
                                //string nombreDocumento = string.Concat(nombreOriginal, item.Extension);
                                int tamano = int.Parse(item.Size.ToString());

                                // Buscamos info adicional de los documentos
                                ReqListDocumentosDto doc = listDocsReqParticipacion.Find(x => x.Id == idReqListDocumentos);

                                if (doc.CodDocumento != _pathOptions.Codigo_Documento_Otros) // Diferente a "Otros"
                                {
                                    PrvDocumento documento = new()
                                    {
                                        PrvdSizeDocument = tamano,
                                        PrvdExtDocument = item.Extension,
                                        PrvdUrlDocument = item.PathWebAbsolute,
                                        PrvdUrlRelDocument = item.PathWebRelative,
                                        PrvdNameDocument = $"{item.NombreInterno}{item.Extension}",
                                        PrvdOriginalNameDocument = doc.DocumentoAGuardar,
                                        PrvdCodDocumento = doc.CodDocumento,
                                        PrvdExpedicion = doc.FechaExpedicion
                                    };

                                    listDocuments_Proveedor.Add(documento);
                                }
                                else
                                {
                                    ReqDocumentosRequerido documento = new()
                                    {
                                        RdrSizeDocument = tamano,
                                        RdrExtDocument = item.Extension,
                                        RdrUrlDocument = item.PathWebAbsolute,
                                        RdrUrlRelDocument = item.PathWebRelative,
                                        RdrNameDocument = $"{item.NombreInterno}{item.Extension}",
                                        RdrOriginalNameDocument = doc.DocumentoAGuardar,
                                        RdrCodReqListDocumentos = doc.Id,
                                        RdrCodProveedor = participacion.CodProveedor
                                    };

                                    listDocuments_Otros.Add(documento);
                                }
                            }
                        }
                    }
                }

                List<ResponseAction> responseAction = await _requerimientosService.PostParticipar(new ParamParticipacionReq
                {
                    PathOptions = _pathOptions,
                    ReqParticipantesDto = participacion,
                    PrvDocumentos = listDocuments_Proveedor,
                    ReqDocumentosRequeridos = listDocuments_Otros
                });

                if (responseAction[0].estado)
                {
                    // Eliminamos del File Server los documentos del Proveedor que fueron reemplazados
                    foreach (var item in listDocsPrvExistentes)
                    {
                        //_filesProcess.DeleteFileFS(_pathOptions, item.PrvdUrlRelDocument);
                    }
                }

                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                //-) Eliminamos documentos del proveedor del File Server posiblemente creados, en el proceso
                foreach (var item in listDocuments_Proveedor)
                {
                    //_filesProcess.DeleteFileFS(_pathOptions, item.PrvdUrlRelDocument);
                }

                //-) Eliminamos documentos Otros del File Server posiblemente creados, en el proceso
                foreach (var item in listDocuments_Otros)
                {
                    //_filesProcess.DeleteFileFS(_pathOptions, item.RdrUrlRelDocument);
                }

                throw new BusinessException($"Error al intentar insertar registro. Detalle: {e.Message}");
            }
        }
        
        /// <summary>
        /// Editar participación
        /// </summary>   
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPut("Participar", Name = "EditarParticipacion")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> EditarParticipacion([FromForm] QueryParticipacion parameters)
        {
            List<PrvDocumento> listDocuments_Proveedor = new(); // listado de archivos que se guardarán en DB
            List<ReqDocumentosRequerido> listDocuments_Otros = new(); // listado de archivos que se guardarán en DB

            try
            {
                ReqParticipantesDto participacion = JsonConvert.DeserializeObject<ReqParticipantesDto>(parameters.Participacion);
                participacion.CodUser = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";

                // Deserializamos DocumentosRequeridos
                List<ReqListDocumentosDto> listDocsReqParticipacion = JsonConvert.DeserializeObject<List<ReqListDocumentosDto>>(parameters.DocumentosRequeridos);
                List<int> idsDocsPrv = listDocsReqParticipacion.Where(x => x.CodDocumento != _pathOptions.Codigo_Documento_Otros).Select(x => x.CodDocumento).ToList();

                // Obtenemos los ids de ReqListDocumentos tanto para documentos del proveedor como documentos 'Otros'
                List<int> idsFilesPrv = listDocsReqParticipacion.Where(x => x.CodDocumento != _pathOptions.Codigo_Documento_Otros).Select(x => x.Id).ToList();
                List<int> idsFilesOtros = listDocsReqParticipacion.Where(x => x.CodDocumento == _pathOptions.Codigo_Documento_Otros).Select(x => x.Id).ToList();

                // Consultamos Paths de archivos de documentos del Proveedor
                List<PrvDocumento> listDocsPrvExistentes = await _prvDocumentoService.GetDocumentosProveedorReq(new ParamDocumentosPrvReq
                {
                    CodProveedor = participacion.CodProveedor,
                    IdsDocumentos = idsDocsPrv
                });

                List<List<FileResponse>> fileListCreated = null;

                if (listDocsReqParticipacion.Any())
                {
                    if (parameters.FilesRequired != null)
                    {
                        string pathCarpetaOtros = Path.Combine(_pathOptions.Folder_Archivos_ParticipacionReq, participacion.CodRequerimento.ToString(), participacion.CodUser);
                        var documentosProveedor = parameters.FilesRequired.Where(x => idsFilesPrv.Contains(int.Parse(x.FileName.Split('|')[0])))?.ToList();
                        var documentosOtros = parameters.FilesRequired.Where(x => idsFilesOtros.Contains(int.Parse(x.FileName.Split('|')[0])))?.ToList();

                        if (documentosProveedor.Any())
                        {
                            //// Creamos archivos del Proveedor
                            //fileListCreated = await _filesProcess.GuardarFileAsync(new SaveProcessFiles
                            //{
                            //    Files = documentosProveedor,
                            //    PathCarpeta = _pathOptions.Folder_Archivos,
                            //    PathOptions = _pathOptions
                            //});
                        }

                        if (documentosOtros.Any())
                        {
                            //// Creamos archivos Otros
                            //fileListCreated = await _filesProcess.GuardarFileAsync(new SaveProcessFiles
                            //{
                            //    Files = documentosOtros,
                            //    PathCarpeta = pathCarpetaOtros,
                            //    PathOptions = _pathOptions
                            //});
                        }

                        // Procesamos los documentos que traen archivos para sacar la informacion de cada uno y guardar en la BD
                        foreach (var filesReq in fileListCreated)
                        {
                            foreach (var item in filesReq)
                            {
                                int idReqListDocumentos = int.Parse(item.NombreOriginal.Split("|")?[0]);
                                //string nombreOriginal = item.NombreOriginal.Split("|")?[1];
                                //string nombreDocumento = string.Concat(nombreOriginal, item.Extension);
                                int tamano = int.Parse(item.Size.ToString());

                                // Buscamos info adicional de los documentos
                                ReqListDocumentosDto doc = listDocsReqParticipacion.Find(x => x.Id == idReqListDocumentos);

                                if (doc.CodDocumento != _pathOptions.Codigo_Documento_Otros) // Diferente a "Otros"
                                {
                                    PrvDocumento documento = new()
                                    {
                                        PrvdSizeDocument = tamano,
                                        PrvdExtDocument = item.Extension,
                                        PrvdUrlDocument = item.PathWebAbsolute,
                                        PrvdUrlRelDocument = item.PathWebRelative,
                                        PrvdNameDocument = $"{item.NombreInterno}{item.Extension}",
                                        PrvdOriginalNameDocument = doc.DocumentoAGuardar,
                                        PrvdCodDocumento = doc.CodDocumento,
                                        PrvdExpedicion = doc.FechaExpedicion
                                    };

                                    listDocuments_Proveedor.Add(documento);
                                }
                                else
                                {
                                    ReqDocumentosRequerido documento = new()
                                    {
                                        RdrSizeDocument = tamano,
                                        RdrExtDocument = item.Extension,
                                        RdrUrlDocument = item.PathWebAbsolute,
                                        RdrUrlRelDocument = item.PathWebRelative,
                                        RdrNameDocument = $"{item.NombreInterno}{item.Extension}",
                                        RdrOriginalNameDocument = doc.DocumentoAGuardar,
                                        RdrCodReqListDocumentos = doc.Id,
                                        RdrCodProveedor = participacion.CodProveedor
                                    };

                                    listDocuments_Otros.Add(documento);
                                }
                            }
                        }
                    }
                }

                List<ResponseAction> responseAction = await _requerimientosService.PutParticipar(new ParamParticipacionReq { 
                    FilesProcess = _filesProcess,
                    PathOptions = _pathOptions,
                    ReqParticipantesDto = participacion
                });

                if (responseAction[0].estado)
                {
                    // Eliminamos del File Server los documentos del Proveedor que fueron reemplazados
                    foreach (var item in listDocsPrvExistentes)
                    {
                        //_filesProcess.DeleteFileFS(_pathOptions, item.PrvdUrlRelDocument);
                    }
                }

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
        /// Método para actualizar el articulo del proveedor elegido, por el gestor de requerimientos, como válido en un requerimiento.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPut("UpdateItemComparativa", Name = "UpdateItemComparativaRequerimientos")]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateItemComparativaRequerimientos([FromBody] QueryUpdateItemComparativa parameters)
        {
            try
            {
                parameters.CodUserUpdate = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                var responseAction = await _requerimientosService.UpdateItemValidadoComparativa(parameters);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la actualización del registro. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para generar adjudicación de requerimientos.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost("AdjudicarRequerimiento", Name = "CreateAdjudicarRequerimiento")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateAdjudicarRequerimiento([FromBody] QueryAdjudicarReq parameters)
        {
            try
            {
                parameters.CodUser = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                var responseAction = await _requerimientosService.CreateAdjudicarRequerimiento(parameters);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la adjudicacion del requerimiento. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para actualizar adjudicación de requerimientos.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPut("UpdateAdjudicarRequerimiento", Name = "UpdateAdjudicarRequerimiento")]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateAdjudicarRequerimiento([FromBody] QueryAdjudicarReq parameters)
        {
            try
            {
                parameters.CodUserUpdate = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                var responseAction = await _requerimientosService.UpdateAdjudicarRequerimiento(parameters);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la adjudicacion del requerimiento. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para asociar órdenes a los articulos ofrecidos por cada proveedor del requerimiento.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost("AsociarOrdenesRequerimiento", Name = "AsociarOrdenesRequerimiento")]
        [Consumes("application/json")]
        public async Task<IActionResult> AsociarOrdenesRequerimiento([FromBody] QueryAsociarOrdenesReq parameters)
        {
            try
            {
                parameters.CodUserUpdate = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                var responseAction = await _requerimientosService.AsociarOrdenesRequerimiento(parameters);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la asociación de órdenes. Detalle: {e.Message}");
            }
        }
    }
}