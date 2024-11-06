namespace Api.Controllers
{
    using Api.Responses;
    using Api.Utils;
    using AutoMapper;
    using Core.CustomEntities;
    using Core.DTOs;
    using Core.DTOs.FilesDto;
    using Core.Entities;
    using Core.Enumerations;
    using Core.Exceptions;
    using Core.Interfaces;
    using Core.ModelProcess;
    using Core.ModelResponse;
    using Core.Options;
    using Core.QueryFilters;
    using Core.Tools;
    using Infrastructure.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize(Policy = "ShouldBeAnAdminOrProv")]
    [ApiController]
    public class ContratoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IContratoService _contratoService;
        private readonly IMapper _mapper;
        private readonly IFilesProcess _filesProcess;
        private readonly PathOptions _pathOptions;
        private readonly IMailService _mailService;

        public ContratoController(
            IConfiguration configuration,
            IOptions<PathOptions> pathOptions,
            IContratoService contratoService,
            IFilesProcess filesProcess,
            IMapper mapper,
            IMailService mailService
        )
        {
            _configuration = configuration;
            _pathOptions = pathOptions.Value;
            _contratoService = contratoService;
            _filesProcess = filesProcess;
            _mapper = mapper;
            _mailService = mailService;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void EliminarArchivo(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                //_filesProcess.DeleteFileFS(_pathOptions, path);
            }
        }

        #region GESTOR

        /// <summary>
        /// Metodo para obtner los parametros iniciales del contrato.
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchParametros", Name = "ParametrosIniciales")]
        [Consumes("application/json")]
        public async Task<IActionResult> ParametrosIniciales()
        {
            try
            {
                ParametrosContrato parametros = await _contratoService.GetParametrosContrato();
                var response = new ApiResponse<ParametrosContrato>(parametros, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la consulta de parametros del registro. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para consultar todos los contratos.
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchAll", Name = "SearchAll")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchAll()
        {
            try
            {
                List<Contrato> contratos = await _contratoService.SearchAll();
                var response = new ApiResponse<List<Contrato>>(contratos, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la consulta. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para buscar contrato especifico.
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpGet("SearchById", Name = "SearchById")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchById([FromQuery] QueryContrato par)
        {
            try
            {
                List<Contrato> contrato = await _contratoService.SearchById(par);
                var response = new ApiResponse<List<Contrato>>(contrato, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la consulta del registro. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para consultar el detalle de un contrato.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("SearchDetalleById", Name = "SearchDetalleById")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchDetalleById(int id)
        {
            try
            {
                ContratoDetalle contrato = await _contratoService.GetContratoDetallePorID(id);
                var response = new ApiResponse<ContratoDetalle>(contrato, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la consulta del registro. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para crear contrato.
        /// </summary>
        /// <param name="queryContrato"></param>
        /// <returns></returns>
        [HttpPost("CreateContrato", Name = "CreateContrato")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateContrato([FromForm] QueryCreateContrato queryContrato)
        {
            try
            {
                ContratoDto contrato = JsonConvert.DeserializeObject<ContratoDto>(queryContrato.Contrato);

                List<DocReqUploadDto> documentosAguardar = new(); // listado de archivos que se guardarán en DB

                /*
                 * Documentos de Tipo URL
                 */

                List<DocumentoContratoDto> listDocContratoURL = contrato.DocumentacionContrato.Where((doc) => { return doc.CodTipoDocumento == ((int)DocumentClass.URL); }).ToList();

                if (listDocContratoURL.Any())
                {
                    foreach (DocumentoContratoDto item in listDocContratoURL)
                    {
                        documentosAguardar.Add(new()
                        {
                            Id = item.CodArchivo ?? 0,
                            DrpuUrl = item.UrlArchivo,
                            KeyFile = item.KeyFile
                        });
                    }
                }

                List<DocReqPolizaDto> listDocReqPolizaURL = contrato.DocumentosReqPoliza.Where((doc) => { return doc.DrpoCodTipoDocumento == ((int)DocumentClass.URL); }).ToList();

                if (listDocReqPolizaURL.Any())
                {
                    foreach (DocReqPolizaDto item in listDocReqPolizaURL)
                    {
                        documentosAguardar.Add(new()
                        {
                            Id = item.CodArchivo ?? 0,
                            DrpuUrl = item.UrlArchivo,
                            KeyFile = item.KeyFile
                        });
                    }
                }

                /*
                 * Documentos de Tipo Archivo
                 */

                if (queryContrato.Files != null && queryContrato.Files.Any())
                {
                    FormDataImagen dataFile = new()
                    {
                        Carpeta = $"{_pathOptions.Folder_Archivos_Contrato}/new",
                        Files = queryContrato.Files,
                        IdPathFileServer = _pathOptions.IdPathFileServer
                    };

                    List<FileResponse> filesReq = new List<FileResponse>() { };

                    /*
                     * Documentos contrato con archivo
                     */

                    List<DocumentoContratoDto> listDocContratoArchivo = contrato.DocumentacionContrato.Where((doc) => { return doc.CodTipoDocumento == ((int)DocumentClass.Archivo); }).ToList();

                    if (listDocContratoArchivo.Any())
                    {
                        foreach (DocumentoContratoDto item in listDocContratoArchivo)
                        {
                            FileResponse file = filesReq.Find((file) => Functions.Validations.IsFileByFilename(file, item.NombreArchivo));

                            if (file != null)
                            {
                                int keyFile = int.Parse(file.NombreOriginal.Split("|")?[0]);
                                string nombreOriginal = file.NombreOriginal.Split("|")?[1];
                                string nombreDocumento = string.Concat(nombreOriginal, file.Extension);
                                int tamano = int.Parse(file.Size.ToString());

                                documentosAguardar.Add(new()
                                {
                                    Id = item.CodArchivo ?? 0,
                                    KeyFile = keyFile,
                                    DrcoUrlDocument = file.PathWebAbsolute,
                                    DrcoUrlRelDocument = file.PathWebRelative,
                                    DrcoExtDocument = file.Extension,
                                    DrcoSizeDocument = tamano,
                                    DrcoNameDocument = $"{file.NombreInterno}{file.Extension}",
                                    DrcoOriginalNameDocument = nombreDocumento
                                });
                            }
                        }
                    }

                    /*
                    * Los proveedores solo se guardara el registro de relacion no el archivo,
                    * tambien para los documentos Otros
                    */

                    /*
                     * Polizas con archivo
                     */
                    List<DocReqPolizaDto> listDocReqPolizaDtoArchivo = contrato.DocumentosReqPoliza.Where((doc) => { return doc.DrpoCodTipoDocumento == ((int)DocumentClass.Archivo); }).ToList();

                    if (listDocReqPolizaDtoArchivo.Any())
                    {
                        foreach (DocReqPolizaDto item in listDocReqPolizaDtoArchivo)
                        {
                            FileResponse file = filesReq.Find((file) => Functions.Validations.IsFileByFilename(file, item.NombreArchivo));

                            if (file != null)
                            {
                                int keyFile = int.Parse(file.NombreOriginal.Split("|")?[0]);
                                string nombreOriginal = file.NombreOriginal.Split("|")?[1];
                                string nombreDocumento = string.Concat(nombreOriginal, file.Extension);
                                int tamano = int.Parse(file.Size.ToString());

                                documentosAguardar.Add(new()
                                {
                                    Id = item.CodArchivo ?? 0,
                                    KeyFile = keyFile,
                                    DrcoUrlDocument = file.PathWebAbsolute,
                                    DrcoUrlRelDocument = file.PathWebRelative,
                                    DrcoExtDocument = file.Extension,
                                    DrcoSizeDocument = tamano,
                                    DrcoNameDocument = $"{file.NombreInterno}{file.Extension}",
                                    DrcoOriginalNameDocument = nombreDocumento
                                });
                            }
                        }
                    }

                    /*
                     * Formato de compra no presupuestada
                     */
                    FileResponse fileCompraNoPresupuestada = filesReq.Find((file) => Functions.Validations.IsFileByFilename(file, contrato.NombreArchivoCompraNoPresupuestada));

                    if (fileCompraNoPresupuestada != null)
                    {
                        int keyFile = int.Parse(fileCompraNoPresupuestada.NombreOriginal.Split("|")?[0]);
                        string nombreOriginal = fileCompraNoPresupuestada.NombreOriginal.Split("|")?[1];
                        string nombreDocumento = string.Concat(nombreOriginal, fileCompraNoPresupuestada.Extension);
                        int tamano = int.Parse(fileCompraNoPresupuestada.Size.ToString());

                        documentosAguardar.Add(new()
                        {
                            Id = contrato.ContArchivoCompraNoPresupuestada ?? 0,
                            KeyFile = keyFile,
                            DrcoUrlDocument = fileCompraNoPresupuestada.PathWebAbsolute,
                            DrcoUrlRelDocument = fileCompraNoPresupuestada.PathWebRelative,
                            DrcoExtDocument = fileCompraNoPresupuestada.Extension,
                            DrcoSizeDocument = tamano,
                            DrcoNameDocument = $"{fileCompraNoPresupuestada.NombreInterno}{fileCompraNoPresupuestada.Extension}",
                            DrcoOriginalNameDocument = nombreDocumento
                        });
                    }

                    /*
                     * Acta de Inicio
                     */
                    FileResponse fileActaInicio = filesReq.Find((file) => Functions.Validations.IsFileByFilename(file, contrato.NombreArchivoActaInicio));

                    if (fileActaInicio != null)
                    {
                        int keyFile = int.Parse(fileActaInicio.NombreOriginal.Split("|")?[0]);
                        string nombreOriginal = fileActaInicio.NombreOriginal.Split("|")?[1];
                        string nombreDocumento = string.Concat(nombreOriginal, fileActaInicio.Extension);
                        int tamano = int.Parse(fileActaInicio.Size.ToString());

                        documentosAguardar.Add(new()
                        {
                            Id = contrato.ContArchivoActaInicio ?? 0,
                            KeyFile = keyFile,
                            DrcoUrlDocument = fileActaInicio.PathWebAbsolute,
                            DrcoUrlRelDocument = fileActaInicio.PathWebRelative,
                            DrcoExtDocument = fileActaInicio.Extension,
                            DrcoSizeDocument = tamano,
                            DrcoNameDocument = $"{fileActaInicio.NombreInterno}{fileActaInicio.Extension}",
                            DrcoOriginalNameDocument = nombreDocumento
                        });
                    }
                }

                /*
                * Documentos desde registro de proveedor
                */

                List<DocReqProveedorDto> listDocProveedorArchivoOld = contrato.DocumentosPrv.Where((doc) => { return doc.DrpTipoVersion == 1; }).ToList();

                if (listDocProveedorArchivoOld.Any())
                {
                    foreach (DocReqProveedorDto item in listDocProveedorArchivoOld)
                    {
                        documentosAguardar.Add(new()
                        {
                            Id = item.CodArchivo ?? 0,
                            KeyFile = item.KeyFile,
                            DrcoUrlDocument = item.PrvdUrlDocument,
                            DrcoUrlRelDocument = item.PrvdUrlRelDocument,
                            DrcoExtDocument = item.PrvdExtDocument,
                            DrcoSizeDocument = item.PrvdSizeDocument,
                            DrcoNameDocument = item.PrvdNameDocument,
                            DrcoOriginalNameDocument = item.PrvdOriginalNameDocument
                        });
                    }
                }

                contrato.DocumentosAguardar = documentosAguardar;

                List<ResponseAction> responseAction = await _contratoService.Post(contrato);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la creación del registro. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para actualizar contrato.
        /// </summary>
        /// <param name="queryContrato"></param>
        /// <returns></returns>
        [HttpPut("UpdateContrato", Name = "UpdateContrato")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateContrato([FromForm] QueryCreateContrato queryContrato)
        {
            try
            {
                ContratoDto contrato = JsonConvert.DeserializeObject<ContratoDto>(queryContrato.Contrato);

                List<DocReqUploadDto> documentosAguardar = new(); // listado de archivos que se guardarán en DB

                /*
                 * Documentos de Tipo URL
                 */

                List<DocumentoContratoDto> listDocContratoURL = contrato.DocumentacionContrato.Where((doc) => { return doc.CodTipoDocumento == ((int)DocumentClass.URL); }).ToList();

                if (listDocContratoURL.Any())
                {
                    foreach (DocumentoContratoDto item in listDocContratoURL)
                    {
                        documentosAguardar.Add(new()
                        {
                            Id = item.CodArchivo ?? 0,
                            DrpuUrl = item.UrlArchivo,
                            KeyFile = item.KeyFile
                        });
                    }
                }

                List<DocReqPolizaDto> listDocReqPolizaURL = contrato.DocumentosReqPoliza.Where((doc) => { return doc.DrpoCodTipoDocumento == ((int)DocumentClass.URL); }).ToList();

                if (listDocReqPolizaURL.Any())
                {
                    foreach (DocReqPolizaDto item in listDocReqPolizaURL)
                    {
                        documentosAguardar.Add(new()
                        {
                            Id = item.CodArchivo ?? 0,
                            DrpuUrl = item.UrlArchivo,
                            KeyFile = item.KeyFile
                        });
                    }
                }

                /*
                 * Documentos de Tipo Archivo
                 */

                if (queryContrato.Files != null && queryContrato.Files.Any())
                {
                    FormDataImagen dataFile = new()
                    {
                        Carpeta = $"{_pathOptions.Folder_Archivos_Contrato}/{contrato.Id}",
                        Files = queryContrato.Files,
                        IdPathFileServer = _pathOptions.IdPathFileServer
                    };

                    //List<FileResponse> filesReq = await _filesProcess.GetFilesCreated(dataFile);
                    List<FileResponse> filesReq = new List<FileResponse>() { };

                    /*
                     * Documentos contrato con archivo
                     */

                    List<DocumentoContratoDto> listDocContratoArchivo = contrato.DocumentacionContrato.Where((doc) => { return doc.CodTipoDocumento == ((int)DocumentClass.Archivo); }).ToList();

                    if (listDocContratoArchivo.Any())
                    {
                        foreach (DocumentoContratoDto item in listDocContratoArchivo)
                        {
                            FileResponse file = filesReq.Find((file) => Functions.Validations.IsFileByFilename(file, item.NombreArchivo));

                            if (file != null)
                            {
                                int keyFile = int.Parse(file.NombreOriginal.Split("|")?[0]);
                                string nombreOriginal = file.NombreOriginal.Split("|")?[1];
                                string nombreDocumento = string.Concat(nombreOriginal, file.Extension);
                                int tamano = int.Parse(file.Size.ToString());

                                documentosAguardar.Add(new()
                                {
                                    Id = item.CodArchivo ?? 0,
                                    KeyFile = keyFile,
                                    DrcoUrlDocument = file.PathWebAbsolute,
                                    DrcoUrlRelDocument = file.PathWebRelative,
                                    DrcoExtDocument = file.Extension,
                                    DrcoSizeDocument = tamano,
                                    DrcoNameDocument = $"{file.NombreInterno}{file.Extension}",
                                    DrcoOriginalNameDocument = nombreDocumento
                                });
                            }
                        }
                    }

                    /*
                    * Los proveedores solo se guardara el registro de relacion no el archivo,
                    * tambien para los documentos Otros
                    */

                    /*
                     * Polizas con archivo
                     */
                    List<DocReqPolizaDto> listDocReqPolizaDtoArchivo = contrato.DocumentosReqPoliza.Where((doc) => { return doc.DrpoCodTipoDocumento == ((int)DocumentClass.Archivo); }).ToList();

                    if (listDocReqPolizaDtoArchivo.Any())
                    {
                        foreach (DocReqPolizaDto item in listDocReqPolizaDtoArchivo)
                        {
                            FileResponse file = filesReq.Find((file) => Functions.Validations.IsFileByFilename(file, item.NombreArchivo));

                            if (file != null)
                            {
                                int keyFile = int.Parse(file.NombreOriginal.Split("|")?[0]);
                                string nombreOriginal = file.NombreOriginal.Split("|")?[1];
                                string nombreDocumento = string.Concat(nombreOriginal, file.Extension);
                                int tamano = int.Parse(file.Size.ToString());

                                documentosAguardar.Add(new()
                                {
                                    Id = item.CodArchivo ?? 0,
                                    KeyFile = keyFile,
                                    DrcoUrlDocument = file.PathWebAbsolute,
                                    DrcoUrlRelDocument = file.PathWebRelative,
                                    DrcoExtDocument = file.Extension,
                                    DrcoSizeDocument = tamano,
                                    DrcoNameDocument = $"{file.NombreInterno}{file.Extension}",
                                    DrcoOriginalNameDocument = nombreDocumento
                                });
                            }
                        }
                    }

                    /*
                     * Formato de compra no presupuestada
                     */
                    FileResponse fileCompraNoPresupuestada = filesReq.Find((file) => Functions.Validations.IsFileByFilename(file, contrato.NombreArchivoCompraNoPresupuestada));
                    if (fileCompraNoPresupuestada != null)
                    {
                        int keyFile = int.Parse(fileCompraNoPresupuestada.NombreOriginal.Split("|")?[0]);
                        string nombreOriginal = fileCompraNoPresupuestada.NombreOriginal.Split("|")?[1];
                        string nombreDocumento = string.Concat(nombreOriginal, fileCompraNoPresupuestada.Extension);
                        int tamano = int.Parse(fileCompraNoPresupuestada.Size.ToString());

                        documentosAguardar.Add(new()
                        {
                            Id = contrato.ContArchivoCompraNoPresupuestada ?? 0,
                            KeyFile = keyFile,
                            DrcoUrlDocument = fileCompraNoPresupuestada.PathWebAbsolute,
                            DrcoUrlRelDocument = fileCompraNoPresupuestada.PathWebRelative,
                            DrcoExtDocument = fileCompraNoPresupuestada.Extension,
                            DrcoSizeDocument = tamano,
                            DrcoNameDocument = $"{fileCompraNoPresupuestada.NombreInterno}{fileCompraNoPresupuestada.Extension}",
                            DrcoOriginalNameDocument = nombreDocumento
                        });
                    }

                    /*
                     * Acta de Inicio
                     */
                    FileResponse fileActaInicio = filesReq.Find((file) => Functions.Validations.IsFileByFilename(file, contrato.NombreArchivoActaInicio));
                    if (fileActaInicio != null)
                    {
                        int keyFile = int.Parse(fileActaInicio.NombreOriginal.Split("|")?[0]);
                        string nombreOriginal = fileActaInicio.NombreOriginal.Split("|")?[1];
                        string nombreDocumento = string.Concat(nombreOriginal, fileActaInicio.Extension);
                        int tamano = int.Parse(fileActaInicio.Size.ToString());

                        documentosAguardar.Add(new()
                        {
                            Id = contrato.ContArchivoActaInicio ?? 0,
                            KeyFile = keyFile,
                            DrcoUrlDocument = fileActaInicio.PathWebAbsolute,
                            DrcoUrlRelDocument = fileActaInicio.PathWebRelative,
                            DrcoExtDocument = fileActaInicio.Extension,
                            DrcoSizeDocument = tamano,
                            DrcoNameDocument = $"{fileActaInicio.NombreInterno}{fileActaInicio.Extension}",
                            DrcoOriginalNameDocument = nombreDocumento
                        });
                    }
                }

                /*
                 * Documentos desde registro de proveedor
                 */

                List<DocReqProveedorDto> listDocProveedorArchivoOld = contrato.DocumentosPrv.Where((doc) => { return doc.DrpTipoVersion == 1; }).ToList();

                if (listDocProveedorArchivoOld.Any())
                {
                    foreach (DocReqProveedorDto item in listDocProveedorArchivoOld)
                    {
                        documentosAguardar.Add(new()
                        {
                            Id = item.CodArchivo ?? 0,
                            KeyFile = item.KeyFile,
                            DrcoUrlDocument = item.PrvdUrlDocument,
                            DrcoUrlRelDocument = item.PrvdUrlRelDocument,
                            DrcoExtDocument = item.PrvdExtDocument,
                            DrcoSizeDocument = item.PrvdSizeDocument,
                            DrcoNameDocument = item.PrvdNameDocument,
                            DrcoOriginalNameDocument = item.PrvdOriginalNameDocument
                        });
                    }
                }

                contrato.DocumentosAguardar = documentosAguardar;

                List<ResponseAction> responseAction = await _contratoService.Update(contrato);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la actualización del registro. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para asociar documentos del proveedor.
        /// </summary>
        /// <param name="queryDocumentos"></param>
        /// <returns></returns>
        [HttpPut("AsociarDocumentosProveedor", Name = "AsociarDocumentosProveedor")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AsociarDocumentosProveedor([FromForm] QueryAddDocProvContrato queryDocumentos)
        {
            try
            {
                DocumentosProveedorDto documentosProveedor = JsonConvert.DeserializeObject<DocumentosProveedorDto>(queryDocumentos.Documentos);

                List<DocReqUploadDto> documentosAguardar = new(); // listado de archivos que se guardarán en DB

                if (queryDocumentos.Files != null && queryDocumentos.Files.Any())
                {
                    FormDataImagen dataFile = new()
                    {
                        Carpeta = $"{_pathOptions.Folder_Archivos_Contrato}/{documentosProveedor.IdContrato}",
                        Files = queryDocumentos.Files,
                        IdPathFileServer = _pathOptions.IdPathFileServer
                    };

                    //List<FileResponse> filesReq = await _filesProcess.GetFilesCreated(dataFile);
                    List<FileResponse> filesReq = new List<FileResponse>() { };

                    /*
                     * Documentos proveedor con archivo
                     */

                    List<DocReqProveedorDto> listDocProveedor = documentosProveedor.DocumentosPrv.Where((doc) => { return doc.KeyFile > 0; }).ToList();

                    if (listDocProveedor.Any())
                    {
                        foreach (DocReqProveedorDto item in listDocProveedor)
                        {
                            FileResponse file = filesReq.Find((file) => Functions.Validations.IsFileByFilename(file, item.NombreArchivo));

                            if (file != null)
                            {
                                int keyFile = int.Parse(file.NombreOriginal.Split("|")?[0]);
                                string nombreOriginal = file.NombreOriginal.Split("|")?[1];
                                string nombreDocumento = string.Concat(nombreOriginal, file.Extension);
                                int tamano = int.Parse(file.Size.ToString());

                                documentosAguardar.Add(new()
                                {
                                    Id = item.CodArchivo ?? 0,
                                    KeyFile = keyFile,
                                    DrcoUrlDocument = file.PathWebAbsolute,
                                    DrcoUrlRelDocument = file.PathWebRelative,
                                    DrcoExtDocument = file.Extension,
                                    DrcoSizeDocument = tamano,
                                    DrcoNameDocument = $"{file.NombreInterno}{file.Extension}",
                                    DrcoOriginalNameDocument = nombreDocumento
                                });
                            }
                        }
                    }

                    /*
                     * Documentos proveedor otros con archivo
                     */

                    List<DocReqProveedorOtroDto> listDocProveedorOtros = documentosProveedor.DocumentosPrvOtros.Where((doc) => { return doc.KeyFile > 0; }).ToList();

                    if (listDocProveedorOtros.Any())
                    {
                        foreach (DocReqProveedorOtroDto item in listDocProveedorOtros)
                        {
                            FileResponse file = filesReq.Find((file) => Functions.Validations.IsFileByFilename(file, item.NombreArchivo));

                            if (file != null)
                            {
                                int keyFile = int.Parse(file.NombreOriginal.Split("|")?[0]);
                                string nombreOriginal = file.NombreOriginal.Split("|")?[1];
                                string nombreDocumento = string.Concat(nombreOriginal, file.Extension);
                                int tamano = int.Parse(file.Size.ToString());

                                documentosAguardar.Add(new()
                                {
                                    Id = item.CodArchivo ?? 0,
                                    KeyFile = keyFile,
                                    DrcoUrlDocument = file.PathWebAbsolute,
                                    DrcoUrlRelDocument = file.PathWebRelative,
                                    DrcoExtDocument = file.Extension,
                                    DrcoSizeDocument = tamano,
                                    DrcoNameDocument = $"{file.NombreInterno}{file.Extension}",
                                    DrcoOriginalNameDocument = nombreDocumento
                                });
                            }
                        }
                    }
                }

                documentosProveedor.DocumentosAguardar = documentosAguardar;

                List<ResponseAction> responseAction = await _contratoService.AsociarDocumentosProveedor(documentosProveedor);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la asociación de documentos del registro. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para rechazar los documentos y notificar al proveedor, por parte del gestor del contrato.
        /// </summary>
        /// <param name="queryRechazarDocumentos"></param>
        /// <returns></returns>
        [HttpPut("RechazarDocumentosProveedor", Name = "RechazarDocumentosProveedor")]
        public async Task<IActionResult> RechazarDocumentosProveedor([FromBody] QueryRechazarDocumentosProveedor queryRechazarDocumentos)
        {
            /*
            * Documentos desde registro de proveedor
            */

            List<DocReqUploadDto> documentosAguardar = new();

            List<DocReqProveedorDto> listDocProveedorArchivoOld = queryRechazarDocumentos.DocumentosPrv.Where((doc) => { return doc.DrpTipoVersion == 1; }).ToList();

            if (listDocProveedorArchivoOld.Any())
            {
                foreach (DocReqProveedorDto item in listDocProveedorArchivoOld)
                {
                    documentosAguardar.Add(new()
                    {
                        Id = item.CodArchivo ?? 0,
                        KeyFile = item.KeyFile,
                        DrcoUrlDocument = item.PrvdUrlDocument,
                        DrcoUrlRelDocument = item.PrvdUrlRelDocument,
                        DrcoExtDocument = item.PrvdExtDocument,
                        DrcoSizeDocument = item.PrvdSizeDocument,
                        DrcoNameDocument = item.PrvdNameDocument,
                        DrcoOriginalNameDocument = item.PrvdOriginalNameDocument
                    });
                }
            }

            queryRechazarDocumentos.DocumentosAguardar = documentosAguardar;

            List<ResponseAction> responseAction = await _contratoService.RechazarDocumentosProveedor(queryRechazarDocumentos);
            var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
            return Ok(response);
        }

        /// <summary>
        /// Método para actualizar estado del contrato.
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPut("UpdateEstado", Name = "UpdateEstado")]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateEstado([FromBody] QueryContrato par)
        {
            try
            {
                List<ResponseAction> responseAction = await _contratoService.UpdateEstado(par);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la eliminación del registro. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para eliminar contrato especifico.
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpDelete("DeleteContrato", Name = "DeleteContrato")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteContrato([FromForm] QueryContrato par)
        {
            try
            {
                List<ResponseAction> responseAction = await _contratoService.UpdateEstado(par);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la eliminación del registro. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para filtro de contrato.
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpGet("FilterContrato", Name = "FilterContrato")]
        [Consumes("application/json")]
        public async Task<IActionResult> FilterContrato([FromBody] QueryContrato par)
        {
            try
            {
                List<ResponseAction> responseAction = await _contratoService.UpdateEstado(par);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en el filtro del registro. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Eliminar Documento contrato
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteDocContrato", Name = "DeleteDocContrato")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeleteDocContrato(int id)
        {
            try
            {
                var responseAction = await _contratoService.EliminarDocContrato(id);
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

        /// <summary>
        /// Método para aprobación del contrato.
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPut("Aprobacion", Name = "AprobacionContrato")]
        [Consumes("application/json")]
        public async Task<IActionResult> AprobacionContrato([FromBody] QueryAprobacionContrato par)
        {
            try
            {
                par.CodUserUpdate = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                List<ResponseAction> responseAction = await _contratoService.AprobacionContrato(par);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la aprobacion del registro. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para habilitar contrato para firma electrónica, este proceso envía el contrato al Proveedor y al Rte. Legal.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost("HabilitarContratoFirma", Name = "HabilitarContratoFirmaElectronica")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> HabilitarContratoFirmaElectronica([FromForm] QueryHabilitarContrato parameters)
        {
            string pathRel = string.Empty;

            try
            {
                if (parameters.File != null)
                {
                    QueryHabilitarContratoInfo contrato = JsonConvert.DeserializeObject<QueryHabilitarContratoInfo>(parameters.Contrato);

                    parameters.Info = contrato;

                    string ext = Path.GetExtension(parameters.File.FileName).ToLower();

                    parameters.Info.NombreArchivo = string.Concat(Funciones.GetCodigoUnico(""), ext);
                    pathRel = Path.Combine(_pathOptions.Path_FileServer_root, _pathOptions.Folder_Archivos_Contrato, parameters.Info.CodContrato.ToString(), parameters.Info.NombreArchivo);

                    parameters.CodUserUpdate = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                    parameters.Info.PathTmp_Contrato = _pathOptions.Path_FileServer_TempFiles;
                    parameters.Info.Path_Contrato = Path.Combine(_pathOptions.Path_FileServer_root, _pathOptions.Folder_Archivos_Contrato);
                    parameters.Info.PathRel_Contrato = string.Concat(_pathOptions.Folder_Archivos_Contrato, "/", parameters.Info.CodContrato);
                    parameters.Info.PathRoot_FS = _pathOptions.Path_FileServer_root;
                    parameters.Info.PathWebAbsolute_Contrato = _pathOptions.Path_WebFileServer;

                    // Validamos si ya había archivo en el File Server, de ser así se elimina para guardar el nuevo y así garantizamos que sólo haya un archivo de contrato
                    string pathRelFileExist = await _contratoService.ValidarArchivoContrato(parameters.Info.CodContrato);

                    var resp = await _contratoService.HabilitarContrato_FirmaElectronica(parameters);
                    if (resp[0].estado)
                    {
                        // Eliminamos archivo antiguo, si lo hay
                        EliminarArchivo(pathRelFileExist);
                    }
                    else
                    {
                        //_filesProcess.DeleteFileFS(_pathOptions, pathRel);
                        return Ok(ErrorResponse.GetError(false, resp[0].mensaje, 400));
                    }
                    var response = new ApiResponse<List<ResponseAction>>(resp, 200);
                    return Ok(response);
                }
                else
                {
                    return Ok(ErrorResponse.GetError(false, "Por favor, adjunte el contrato", 400));
                }
            }
            catch (Exception e)
            {
                // Eliminamos archivo actual, si se alcanzó a crear
                EliminarArchivo(pathRel);
                throw new BusinessException($"Error en la habilitación del contrato. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para cargar contrato previamente firmado en físico.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost("CargarContratoFirmado", Name = "CargarContratoFirmado")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CargarContratoFirmado([FromForm] QueryHabilitarContrato parameters)
        {
            string pathRel = string.Empty;
            try
            {
                if (parameters.File != null)
                {
                    QueryHabilitarContratoInfo contrato = JsonConvert.DeserializeObject<QueryHabilitarContratoInfo>(parameters.Contrato);

                    parameters.Info = contrato;

                    parameters.CodUserUpdate = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                    parameters.Info.PathRoot_FS = _pathOptions.Path_FileServer_root;

                    // Validamos si ya había archivo en el File Server, de ser así se elimina para guardar el nuevo y así garantizamos que sólo haya un archivo de contrato
                    string pathRelFileExist = await _contratoService.ValidarArchivoContrato(parameters.Info.CodContrato);

                    FormDataImagen dataFile = new()
                    {
                        Carpeta = string.Concat(_pathOptions.Folder_Archivos_Contrato, "/", parameters.Info.CodContrato),
                        Files = new List<IFormFile>() { parameters.File },
                        IdPathFileServer = _pathOptions.IdPathFileServer
                    };

                    //FileResponse filesReq = await _filesProcess.GetFileCreated(dataFile);
                    FileResponse filesReq = new() { };

                    pathRel = filesReq.PathWebRelative.Split("/PathFilePortalIbis/")[1];

                    var resp = await _contratoService.UpdateUrlContrato_CargadoManual(new QueryUpdateUrlContrato
                    {
                        CodContrato = parameters.Info.CodContrato,
                        CodUserUpdate = parameters.CodUserUpdate,
                        Url = pathRel,
                        UrlAbsolute = filesReq.PathWebAbsolute
                    });
                    if (resp[0].estado)
                    {
                        // Eliminamos archivo antiguo, si lo hay
                        EliminarArchivo(pathRelFileExist);
                    }
                    else
                    {
                        //_filesProcess.DeleteFileFS(_pathOptions, pathRel);
                        return Ok(ErrorResponse.GetError(false, resp[0].mensaje, 400));
                    }
                    var response = new ApiResponse<List<ResponseAction>>(resp, 200);
                    return Ok(response);
                }
                else
                {
                    return Ok(ErrorResponse.GetError(false, "Por favor, adjunte el contrato", 400));
                }
            }
            catch (Exception e)
            {
                // Eliminamos archivo actual, si se alcanzó a crear
                EliminarArchivo(pathRel);
                throw new BusinessException($"Error en la carga del contrato manual. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para Solicitar documentos a proveedor por email.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost("SolicitarDocumentosProveedor", Name = "SolicitarDocumentosProveedor")]
        public async Task<IActionResult> SolicitarDocumentosProveedor([FromBody] QuerySolicitarDocumentosProveedor parameters)
        {
            List<ResponseAction> responseAction = await _contratoService.SolicitarDocumentosProveedor(parameters);
            var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
            return Ok(response);
        }

        /// <summary>
        /// Método para consultar los parametros iniciales del proveedor logueado
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchParametrosByProveedor", Name = "SearchParametrosByProveedor")]
        public async Task<IActionResult> SearchParametrosByProveedor()
        {
            int user = int.Parse(HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0");

            ParametrosContrato parametros = await _contratoService.SearchByProveedor(user);
            var response = new ApiResponse<ParametrosContrato>(parametros, 200);

            return Ok(response);
        }

        /// <summary>
        /// Método para guardar un seguimiento a un contrato
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost("GuardarSeguimiento", Name = "GuardarSeguimiento")]
        public async Task<IActionResult> GuardarSeguimiento([FromForm] QuerySeguimiento parameters)
        {
            SeguimientosContratoDto seguimiento = JsonConvert.DeserializeObject<SeguimientosContratoDto>(parameters.Seguimiento);

            if (parameters.File != null)
            {
                FormDataImagen dataFile = new FormDataImagen()
                {
                    Carpeta = $"{_pathOptions.Folder_Archivos_Seguimientos_Contratos}/{seguimiento.ScoCodContrato}",
                    Files = new List<Microsoft.AspNetCore.Http.IFormFile>() { parameters.File },
                    IdPathFileServer = _pathOptions.IdPathFileServer
                };

                //List<FileResponse> filesReq = await _filesProcess.GetFilesCreated(dataFile);
                List<FileResponse> filesReq = new List<FileResponse>() { };

                if (filesReq.Any())
                {
                    var file = filesReq.FirstOrDefault();

                    if (file != null)
                    {
                        int keyFile = int.Parse(file.NombreOriginal.Split("|")?[0]);
                        string nombreOriginal = file.NombreOriginal.Split("|")?[1];
                        string nombreDocumento = string.Concat(nombreOriginal, file.Extension);
                        int tamano = int.Parse(file.Size.ToString());

                        seguimiento.DocumentoAGuardar = new()
                        {
                            Id = seguimiento.CodArchivo ?? 0,
                            KeyFile = keyFile,
                            DrcoUrlDocument = file.PathWebAbsolute,
                            DrcoUrlRelDocument = file.PathWebRelative,
                            DrcoExtDocument = file.Extension,
                            DrcoSizeDocument = tamano,
                            DrcoNameDocument = $"{file.NombreInterno}{file.Extension}",
                            DrcoOriginalNameDocument = nombreDocumento
                        };
                    }
                }
            }

            List<ResponseAction> responseAction = await _contratoService.GuardarSeguimiento(seguimiento);
            var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
            return Ok(response);
        }

        /// <summary>
        /// Metodo para adjuntar Notificación de No prorroga
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GuardarNotificacionNoProroga", Name = "GuardarNotificacionNoProroga")]
        public async Task<IActionResult> GuardarNotificacionNoProroga([FromForm] QueryNotificacionNoProrroga request)
        {
            var contrato = JsonConvert.DeserializeObject<ContratoNotificacionNoProrroga>(request.Contrato);

            if (request.File is null)
            {
                throw new BusinessException(@"No se envió un documento adjunto.");
            }

            var dataFile = new FormDataImagen()
            {
                Carpeta = $"{_pathOptions.Folder_Archivos_Notificaciones_NoProrroga_Contratos}/{contrato.IdContrato}",
                Files = new List<IFormFile>() { request.File },
                IdPathFileServer = _pathOptions.IdPathFileServer
            };

            //List<FileResponse> filesReq = await _filesProcess.GetFilesCreated(dataFile);            
            List<FileResponse> filesReq = new List<FileResponse>() { };

            var file = filesReq.FirstOrDefault();

            if (file is null)
            {
                throw new BusinessException(@"No se guardo el documento adjunto.");
            }

            int keyFile = int.Parse(file.NombreOriginal.Split("|")?[0]);
            string nombreOriginal = file.NombreOriginal.Split("|")?[1];
            string nombreDocumento = string.Concat(nombreOriginal, file.Extension);
            int tamano = int.Parse(file.Size.ToString());

            contrato.DocumentoAGuardar = new()
            {
                Id = contrato.CodArchivoNotificacionNoProrroga ?? 0,
                KeyFile = keyFile,
                DrcoUrlDocument = file.PathWebAbsolute,
                DrcoUrlRelDocument = file.PathWebRelative,
                DrcoExtDocument = file.Extension,
                DrcoSizeDocument = tamano,
                DrcoNameDocument = $"{file.NombreInterno}{file.Extension}",
                DrcoOriginalNameDocument = nombreDocumento
            };

            List<ResponseAction> responseAction = await _contratoService.GuardarNotificacionNoProrroga(contrato);
            var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
            return Ok(response);
        }

        /// <summary>
        /// Metodo para adjuntar Notificación Terminación
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GuardarNotificacionTermAnticipada", Name = "GuardarNotificacionTerminacionAnticipada")]
        public async Task<IActionResult> GuardarNotificacionTerminacionAnticipada([FromForm] QueryNotificacionTerminacion request)
        {
            var contrato = JsonConvert.DeserializeObject<ContratoNotificacionTerminacion>(request.Contrato);

            if (request.File is null)
            {
                throw new BusinessException(@"No se envió un documento adjunto.");
            }

            var dataFile = new FormDataImagen()
            {
                Carpeta = $"{_pathOptions.Folder_Archivos_Terminacion}/{contrato.IdContrato}",
                Files = new List<IFormFile>() { request.File },
                IdPathFileServer = _pathOptions.IdPathFileServer
            };

            //List<FileResponse> filesReq = await _filesProcess.GetFilesCreated(dataFile);
            List<FileResponse> filesReq = new List<FileResponse>() { };

            var file = filesReq.FirstOrDefault();

            if (file is null)
            {
                throw new BusinessException(@"No se guardo el documento adjunto.");
            }

            int keyFile = int.Parse(file.NombreOriginal.Split("|")?[0]);
            string nombreOriginal = file.NombreOriginal.Split("|")?[1];
            string nombreDocumento = string.Concat(nombreOriginal, file.Extension);
            int tamano = int.Parse(file.Size.ToString());

            contrato.DocumentoAGuardar = new()
            {
                Id = contrato.CodArchivoNotificacionTerminacion ?? 0,
                KeyFile = keyFile,
                DrcoUrlDocument = file.PathWebAbsolute,
                DrcoUrlRelDocument = file.PathWebRelative,
                DrcoExtDocument = file.Extension,
                DrcoSizeDocument = tamano,
                DrcoNameDocument = $"{file.NombreInterno}{file.Extension}",
                DrcoOriginalNameDocument = nombreDocumento
            };

            List<ResponseAction> responseAction = await _contratoService.GuardarNotificacionTerminacionAnticipada(contrato);
            var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
            return Ok(response);
        }

        /// <summary>
        /// Método para Aprobar la prorroga.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost("Prorroga/Aprobar", Name = "AprobarProrroga")]
        public async Task<IActionResult> AprobarProrroga([FromBody] QueryAprobarProrroga parameters)
        {
            try
            {
                List<ResponseAction> responseAction = await _contratoService.AprobarProrroga(parameters);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la Aprobación de Prórroga del Contrato. Detalle: {e.Message}");
            }

        }

        /// <summary>
        /// Método para adjuntar las polizas renovadas
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpPost("AsociarPolizasRenovadas", Name = "AsociarPolizasRenovadas")]
        public async Task<IActionResult> AsociarPolizasRenovadas([FromForm] QueryPolizasRenovadas request)
        {
            if (request is null)
            {
                throw new BusinessException(@"No se enviaron datos en la petición.");
            }

            if (request.Files is null || !request.Files.Any())
            {
                throw new BusinessException(@"No se enviarón los documentos adjuntos.");
            }

            var contrato = JsonConvert.DeserializeObject<QueryContratoPolizasRenovadas>(request.Contrato);

            List<DocReqUploadDto> documentosAguardar = new();

            List<DocReqPolizaDto> listDocReqPolizaURL = contrato.DocumentosReqPoliza.Where((doc) => { return doc.DrpoCodTipoDocumento == ((int)DocumentClass.URL); }).ToList();

            if (listDocReqPolizaURL.Any())
            {
                foreach (DocReqPolizaDto item in listDocReqPolizaURL)
                {
                    documentosAguardar.Add(new()
                    {
                        Id = item.CodArchivo ?? 0,
                        DrpuUrl = item.UrlArchivo,
                        KeyFile = item.KeyFile
                    });
                }
            }

            FormDataImagen dataFile = new()
            {
                Carpeta = $"{_pathOptions.Folder_Archivos_Contrato}/{contrato.IdContrato}",
                Files = request.Files,
                IdPathFileServer = _pathOptions.IdPathFileServer
            };

            //List<FileResponse> filesReq = await _filesProcess.GetFilesCreated(dataFile);
            List<FileResponse> filesReq = new List<FileResponse>() { };

            if (filesReq is null || !filesReq.Any())
            {
                throw new BusinessException(@"No se guardaron los documentos adjuntos.");
            }

            List<DocReqPolizaDto> listDocReqPolizaDtoArchivo = contrato.DocumentosReqPoliza.Where((doc) => { return doc.DrpoCodTipoDocumento == ((int)DocumentClass.Archivo); }).ToList();

            if (listDocReqPolizaDtoArchivo.Any())
            {
                foreach (DocReqPolizaDto item in listDocReqPolizaDtoArchivo)
                {
                    FileResponse file = filesReq.Find((file) => Functions.Validations.IsFileByFilename(file, item.NombreArchivo));

                    if (file != null)
                    {
                        int keyFile = int.Parse(file.NombreOriginal.Split("|")?[0]);
                        string nombreOriginal = file.NombreOriginal.Split("|")?[1];
                        string nombreDocumento = string.Concat(nombreOriginal, file.Extension);
                        int tamano = int.Parse(file.Size.ToString());

                        documentosAguardar.Add(new()
                        {
                            Id = item.CodArchivo ?? 0,
                            KeyFile = keyFile,
                            DrcoUrlDocument = file.PathWebAbsolute,
                            DrcoUrlRelDocument = file.PathWebRelative,
                            DrcoExtDocument = file.Extension,
                            DrcoSizeDocument = tamano,
                            DrcoNameDocument = $"{file.NombreInterno}{file.Extension}",
                            DrcoOriginalNameDocument = nombreDocumento
                        });
                    }
                }
            }

            contrato.DocumentosAguardar = documentosAguardar;

            List<ResponseAction> responseAction = await _contratoService.AsociarPolizasRenovadas(contrato);
            var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
            return Ok(response);
        }

        /// <summary>
        /// Metodo para adjuntar Acta de Liquidación
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GuardarActaLiquidacion", Name = "GuardarActaLiquidacion")]
        public async Task<IActionResult> GuardarActaLiquidacion([FromForm] QueryActaLiquidacion request)
        {
            var contrato = JsonConvert.DeserializeObject<ContratoActaLiquidacion>(request.Contrato);

            if (request.File is null)
            {
                throw new BusinessException(@"No se envió un documento adjunto.");
            }

            var dataFile = new FormDataImagen()
            {
                Carpeta = $"{_pathOptions.Folder_Archivos_Notificaciones_NoProrroga_Contratos}/{contrato.IdContrato}",
                Files = new List<IFormFile>() { request.File },
                IdPathFileServer = _pathOptions.IdPathFileServer
            };

            //List<FileResponse> filesReq = await _filesProcess.GetFilesCreated(dataFile);
            List<FileResponse> filesReq = new List<FileResponse>() { };

            var file = filesReq.FirstOrDefault();

            if (file is null)
            {
                throw new BusinessException(@"No se guardo el documento adjunto.");
            }

            int keyFile = int.Parse(file.NombreOriginal.Split("|")?[0]);
            string nombreOriginal = file.NombreOriginal.Split("|")?[1];
            string nombreDocumento = string.Concat(nombreOriginal, file.Extension);
            int tamano = int.Parse(file.Size.ToString());

            contrato.DocumentoAGuardar = new()
            {
                Id = contrato.CodArchivoActaLiquidacion ?? 0,
                KeyFile = keyFile,
                DrcoUrlDocument = file.PathWebAbsolute,
                DrcoUrlRelDocument = file.PathWebRelative,
                DrcoExtDocument = file.Extension,
                DrcoSizeDocument = tamano,
                DrcoNameDocument = $"{file.NombreInterno}{file.Extension}",
                DrcoOriginalNameDocument = nombreDocumento
            };

            List<ResponseAction> responseAction = await _contratoService.GuardarActaLiquidacion(contrato);
            var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
            return Ok(response);
        }

        /// <summary>
        /// Método para solicitar modificar el contrato.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost("SolicitudModificacionContrato", Name = "SolicitudModificacionContrato")]
        [Consumes("application/json")]
        public async Task<IActionResult> SolicitudModificacionContrato([FromBody] QuerySolicitudModificacion parameters)
        {
            try
            {
                List<ResponseAction> responseAction = await _contratoService.SolicitarModificacionContrato(parameters);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la Solicitud de Modificación de Contrato. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Metodo para listar los contratos con sus historicos
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("HistoricosContratos", Name = "HistoricosContratos")]
        [Consumes("application/json")]
        public async Task<IActionResult> HistoricosContratos()
        {
            var responseData = await _contratoService.GetParametrosHistoricos();
            var response = new ApiResponse<ParametrosHistoricos>(responseData, 200);
            return Ok(response);
        }

        /// <summary>
        /// Método para agregar un comentario a un contrato
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{idContrato:int}/Comentarios", Name = "GuardarComentario")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> GuardarComentario([FromForm] CommandCreateComentarioContrato command)
        {
            var responseAction = await _contratoService.GuardarComentarioContrato(command);
            var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
            return Ok(response);
        }

        /// <summary>
        /// Método para consultar los comentarios de un contrato en especifico
        /// </summary>
        /// <param name="idContrato"></param>
        /// <returns></returns>
        [HttpGet("{idContrato:int}/Comentarios", Name = "ConsultarComentarios")]
        [Consumes("application/json")]
        public async Task<IActionResult> ConsultarComentarios(int idContrato)
        {
            var responsedata = await _contratoService.ConsultarComentariosContrato(idContrato);
            var response = new ApiResponse<List<ContratoComentarios>>(responsedata, 200);

            return Ok(response);
        }

        /// <summary>
        /// Método para cambiar el estado de un contrato
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{idContrato:int}/Estado", Name = "CambiarEstadoContrato")]
        [Consumes("application/json")]
        public async Task<IActionResult> CambiarEstadoContrato([FromBody] CommandUpdateEstadoContrato command)
        {
            var responseAction = await _contratoService.CambiarEstadoContrato(command);
            var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
            return Ok(response);
        }

        #endregion GESTOR

        #region TRAZABILIDAD

        /// <summary>
        /// Método para consultar el Timeline.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Timeline", Name = "TimelineContratoById")]
        [Consumes("application/json")]
        public async Task<IActionResult> TimelineContratoById(int id)
        {
            var timeline = await _contratoService.GetTimelineContratoById(id);
            var response = new ApiResponse<List<TimelineDto>>(timeline, 200);
            return Ok(response);
        }

        #endregion TRAZABILIDAD

        #region NOTIFICACIONES

        /// <summary>
        /// Método para completar JOB SQL de notificaciones recurrentes.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost("SendEmailContratosPendientes", Name = "SendEmail_ActividadesPendientes")]
        [Consumes("application/json")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> SendEmail_ActividadesPendientes([FromBody] QuerySendMailMasive parameters)
        {
            try
            {
                List<ResponseAction> responseAction;

                switch (parameters.OpcionAEjecutar)
                {
                    case (int)Email_OpcionEjecutar.TodosLosEstados_ExceptoFirmaElectronica:
                        responseAction = await _mailService.SendMail_ContratosPendientes(parameters);
                        break;

                    case (int)Email_OpcionEjecutar.FirmaElectronica:
                        responseAction = await _mailService.SendMail_ContratosFirmasPendientes(parameters);
                        break;

                    default:
                        responseAction = new List<ResponseAction>(){
                            new ResponseAction()
                            {
                                error = "Error: Debe proporcionar una opción válida",
                                estado = false,
                                mensaje = "Debe proporcionar una opción válida",
                                Id = 0
                            }
                        };
                        break;
                }

                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en el envío de correo. Detalle: {e.Message}");
            }
        }

        #endregion NOTIFICACIONES

        /// <summary>
        /// Método para consultar un listado de contrato para la grid de reporte
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        //[AllowAnonymous]
        //[HttpPost("reportes/listado", Name = "ListadoContratosReporte")]
        //[Consumes("application/json")]
        //public async Task<IActionResult> ListadoContratosReporte([FromBody] QueryListadoContratosReporte query)
        //{
        //    try
        //    {
        //        var listado = await _contratoService.ListadoContratos(query);
        //        var response = new ApiResponse<List<ContratoListado>>(listado, 200, listado.Count);
        //        return Ok(response);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new BusinessException($"Error en la consulta del listado del reporte. Detalle: {e.Message}");
        //    }
        //}

        /// <summary>
        /// Método para generar iun archivo de excel de listado de contratos
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        //[HttpPost("reportes/listado/excel", Name = "ListadoContratosReporteExcel")]
        //[Consumes("application/json")]
        //public async Task<IActionResult> ListadoContratosReporteExcel([FromBody] QueryListadoContratosReporte query)
        //{
        //    try
        //    {
        //        var listado = await _contratoService.ListadoContratosExcel(query);
        //        return File(listado, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"listadoContratos{DateTime.Now:yyyyMMddhhmmss}.xlsx"); ;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new BusinessException($"Error en la generación del reporte. Detalle: {e.Message}");
        //    }
        //}

        /// <summary>
        /// Método para aprobar como administrador
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPut("{idContrato:int}/Aprobacion/Administrador", Name = "AprobarContratoAdministrador")]
        [Consumes("application/json")]
        public async Task<IActionResult> AprobarContratoAdministrador(int idContrato)
        {
            var responseAction = await _contratoService.AprobacionAdministrador(new CommandUpdateEstadoContrato { IdContrato = idContrato });
            var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
            return Ok(response);
        }
    }
}