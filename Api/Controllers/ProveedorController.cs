﻿namespace Api.Controllers
{
    using Api.Responses;
    using AutoMapper;
    using Core.CustomEntities;
    using Core.DTOs;
    using Core.DTOs.FilesDto;
    using Core.Entities;
    using Core.Enumerations;
    using Core.Exceptions;
    using Core.HubConfig;
    using Core.Interfaces;
    using Core.ModelProcess;
    using Core.ModelResponse;
    using Core.QueryFilters;
    using FluentValidation;
    using Infrastructure.Interfaces;
    using Infrastructure.Validators;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedorController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IProveedorService _proveedorService;
        private readonly IPrvDocumentoService _documentoService;
        private readonly IMapper _mapper;
        private readonly IFilesProcess _filesProcess;
        private readonly IPasswordService _passwordService;
        private readonly IMailService _mailService;
        private readonly IHubContext<ProveedorHub> _hubContext;
        private int IdPathFS { get; set; }

        public ProveedorController(
            IConfiguration configuration,
            IProveedorService proveedorService,
            IPrvDocumentoService documentoService,
            IMapper mapper,
            IFilesProcess filesProcess,
            IPasswordService passwordService,
            IMailService mailService,
            IHubContext<ProveedorHub> hubContext)
        {
            _configuration = configuration;
            _proveedorService = proveedorService;
            _documentoService = documentoService;
            _mapper = mapper;
            _filesProcess = filesProcess;
            _passwordService = passwordService;
            _mailService = mailService;
            _hubContext = hubContext;
            IdPathFS = int.Parse(_configuration.GetSection("IdPathFileServer")?.Value);
        }

        /// <summary>
        /// Consultar proveedor por Nit
        /// </summary>
        /// <param name="nit"></param>
        /// <returns></returns>
        [HttpGet("Search", Name = "SearchProveedor")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchProveedor(string nit)
        {
            try
            {
                var list = await _proveedorService.GetProveedorPorNit(nit, IdPathFS);
                var entidad = list.Count > 0 ? list[0] : null;

                if (entidad is null)
                {
                    return Ok(ErrorResponse.GetError(false, "Proveedor Inválido", 400));
                }

                ProveedorDto proveedorDto = _mapper.Map<ProveedorDto>(entidad);
                var response = new ApiResponse<ProveedorDto>(proveedorDto, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Consultar proveedor con su detalle por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("SearchDetalle", Name = "SearchProveedorDetalle")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchProveedorDetalle(int id)
        {
            try
            {
                var list = await _proveedorService.GetProveedorDetallePorID(id, IdPathFS);

                var response = new ApiResponse<ProveedorDetalle>(list, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        [HttpGet("SearchDocProveedor", Name = "SearchDocProveedor")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchDocProveedor(int id)
        {
            try
            {
                var list = await _documentoService.GetPrvDocumento(id);

                var response = new ApiResponse<List<PrvDocumento>>(list, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Consultar todos los proveedores
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchAll", Name = "SearchAllProveedores")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdmin")]
        public async Task<IActionResult> SearchAllProveedores()
        {
            try
            {
                List<Proveedores> proveedores = await _proveedorService.GetProveedores(IdPathFS);
                var proveedoresDto = _mapper.Map<List<ProveedorDto>>(proveedores);

                var response = new ApiResponse<List<ProveedorDto>>(proveedoresDto, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Parametros para registro de proveedores
        /// </summary>
        /// <returns></returns>
        [HttpGet("ParametrosProveedor", Name = "GetParametros")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> GetParametros(int idProveedor)
        {
            try
            {
                var perfiles = await _proveedorService.GetParametros(idProveedor);
                var response = new ApiResponse<ParametrosProveedor>(perfiles, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Parametros para registro de proveedores
        /// </summary>
        /// <returns></returns>
        [HttpGet("ParametrosGestionProveedor", Name = "GetParametrosGestionProveedor")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdmin")]
        public async Task<IActionResult> GetParametrosGestionProveedor()
        {
            try
            {
                ParametrosGestionProveedor parametrosGestionProveedor = await _proveedorService.GetParametrosGestionProveedor();
                List<Proveedores> proveedores = await _proveedorService.GetProveedores(IdPathFS);
                parametrosGestionProveedor.Proveedores = _mapper.Map<List<ProveedorDto>>(proveedores);

                var response = new ApiResponse<ParametrosGestionProveedor>(parametrosGestionProveedor, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Parametros para registro de proveedores
        /// </summary>
        /// <returns></returns>
        [HttpGet("FiltroProveedor", Name = "GetProveedoresFiltro")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdmin")]
        public async Task<IActionResult> GetFiltroProveedor(string idLocalidad, int idCategoriaServicio, int idEstado, string razonSocial, string nit)
        {
            try
            {
                var perfiles = await _proveedorService.GetFiltroProveedor(idLocalidad, idCategoriaServicio, idEstado, razonSocial, nit, IdPathFS);
                var response = new ApiResponse<List<Proveedores>>(perfiles, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para consultar los requerimientos a los que puede aplicar el proveedor
        /// </summary>
        /// <returns></returns>
        [HttpGet("SearchReqProveedor", Name = "GetRequerimientosProveedor")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        public async Task<IActionResult> GetRequerimientosProveedor()
        {
            try
            {
                int idProveedor = int.Parse(HttpContext.Items["UserID"]?.ToString());
                var entity = await _proveedorService.GetRequerimientosProveedor(idProveedor);
                var response = new ApiResponse<List<Requerimientos>>(entity, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Crear proveedor
        /// </summary>
        /// <returns></returns>
        [HttpPost("Create", Name = "CreateProveedor")]
        [Consumes("multipart/form-data")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateProveedor([FromForm] QueryCreateProveedores parameters)
        {
            try
            {
                var proveedor = JsonConvert.DeserializeObject<ProveedorDto>(parameters.Proveedor);

                DataModelSignalResponse<DataProveedores> dataModel = new()
                {
                    Id = proveedor.PrvNit,
                    Data = new() { Progress = 20 }
                };
                await _hubContext.Clients.All.SendAsync(HubConectionsMethods.CreateProveedor, dataModel);

                SignalParams<ProveedorHub, DataProveedores> signalParam = new()
                {
                    DataModel = dataModel,
                    HubContext = _hubContext
                };

                ProveedoresValidator val = new();
                var validationResult = val.Validate(proveedor, e => e.IncludeRuleSets("CreateValidation"));
                
                if (!validationResult.IsValid)
                {
                    signalParam.DataModel.Data.Progress = 100;
                    await _hubContext.Clients.All.SendAsync(HubConectionsMethods.CreateProveedor, signalParam.DataModel);
                    return Ok(ErrorResponse.GetError(false, validationResult.ToString("|"), 400));
                }

                var listDocumentsInfo = JsonConvert.DeserializeObject<List<DocumentoDto>>(parameters.ListDocuments);
                List<List<FileResponse>> fileListCreated = new();
                List<PrvDocumento> listDocuments = new(); // listado de archivos que se guardarán en DB
                string fileFolder = _configuration.GetSection("Folder_Archivos")?.Value;

                #region Guardado de Documentos

                // Guardar archivos FilesRequired
                if (parameters.FilesRequired != null)
                {
                    FormDataImagen dataFile = new()
                    {
                        Carpeta = fileFolder,
                        Files = parameters.FilesRequired,
                        IdPathFileServer = IdPathFS
                    };

                    //List<FileResponse> filesReq = await _filesProcess.GetFilesCreated(dataFile);
                    List<FileResponse> filesReq = new() { };
                    fileListCreated.Add(filesReq);

                    // Procesamos los archivos para sacar la informacion de cada uno y guardar en la BD
                    foreach (var item in filesReq)
                    {
                        string nombre = item.NombreOriginal;
                        int codDocumento = int.Parse(nombre.Split("-")[0]);
                        string nombreDocumento = nombre.Split("-")[1];
                        int tamano = int.Parse(item.Size.ToString());

                        // Buscamos info adicional de los documentos
                        DocumentoDto doc = listDocumentsInfo.Find(x => x.Id == codDocumento);

                        PrvDocumento documento = new()
                        {
                            PrvdSizeDocument = tamano,
                            PrvdExtDocument = item.Extension,
                            PrvdUrlDocument = item.PathWebAbsolute,
                            PrvdUrlRelDocument = item.PathWebRelative,
                            PrvdNameDocument = $"{item.NombreInterno}{item.Extension}",
                            PrvdOriginalNameDocument = $"{nombreDocumento}{item.Extension}",
                            PrvdCodDocumento = doc.Id,
                            PrvdExpedicion = doc.VigenciaDate
                        };

                        listDocuments.Add(documento);
                    }
                }
                else if (parameters.FilesRequired is null)
                {
                    signalParam.DataModel.Data.Progress = 100;
                    await _hubContext.Clients.All.SendAsync(HubConectionsMethods.CreateProveedor, signalParam.DataModel);
                    return Ok(ErrorResponse.GetError(false, "Error, al menos debe haber un archivo.", 400));
                }

                signalParam.DataModel.Data.Progress = 30;
                await _hubContext.Clients.All.SendAsync(HubConectionsMethods.CreateProveedor, signalParam.DataModel);

                // Guardar archivos OthersFiles
                if (parameters.OthersFiles != null)
                {
                    FormDataImagen dataFile = new()
                    {
                        Carpeta = fileFolder,
                        Files = parameters.OthersFiles,
                        IdPathFileServer = IdPathFS
                    };

                    //List<FileResponse> othersFiles = await _filesProcess.GetFilesCreated(dataFile);
                    List<FileResponse> othersFiles = new() { };
                    fileListCreated.Add(othersFiles);

                    // Procesamos los archivos para sacar la informacion de cada uno y guardar en la BD
                    foreach (var item in othersFiles)
                    {
                        string nombre = item.NombreOriginal;
                        string codigoDocOthers = _configuration.GetSection("Codigo_Documento_Otros").Value;
                        int codDocumento = int.Parse(codigoDocOthers); // Otros
                        int tamano = int.Parse(item.Size.ToString());

                        PrvDocumento documento = new()
                        {
                            PrvdSizeDocument = tamano,
                            PrvdExtDocument = item.Extension,
                            PrvdUrlDocument = item.PathWebAbsolute,
                            PrvdUrlRelDocument = item.PathWebRelative,
                            PrvdNameDocument = $"{item.NombreInterno}{item.Extension}",
                            PrvdOriginalNameDocument = nombre,
                            PrvdCodDocumento = codDocumento
                        };

                        listDocuments.Add(documento);
                    }
                }

                #endregion Guardado de Documentos

                signalParam.DataModel.Data.Progress = 40;
                await _hubContext.Clients.All.SendAsync(HubConectionsMethods.CreateProveedor, signalParam.DataModel);

                string pathFS = _configuration.GetSection("Path_FileServer")?.Value;
                string pathFSWeb = _configuration.GetSection("Path_WebFileServer")?.Value;

                // Generamos contraseña temporal del proveedor
                string UsrPassword = _passwordService.Hash(proveedor?.PrvNit);

                var responseAction = await _proveedorService.PostCrear(proveedor, listDocuments, UsrPassword, pathFS, pathFSWeb, signalParam, IdPathFS);

                var response = new ApiResponse<List<ResponseActionUrl>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }

                signalParam.DataModel.Data.Progress = 100;
                await _hubContext.Clients.All.SendAsync(HubConectionsMethods.CreateProveedor, signalParam.DataModel);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error al intentar insertar registro. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para actualizar proveedor
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPut("Update", Name = "UpdateProveedor")]
        [Consumes("multipart/form-data")]
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        public async Task<IActionResult> UpdateProveedor([FromForm] QueryCreateProveedores parameters)
        {
            return Ok(await UpdateProveedores(parameters));
        }

        /// <summary>
        /// Método para actualizar proveedor sin seguridad
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPut("UpdatePrv", Name = "UpdateProveedorPrv")]
        [Consumes("multipart/form-data")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateProveedorPrv([FromForm] QueryCreateProveedores parameters)
        {
            return Ok(await UpdateProveedores(parameters));
        }

        /// <summary>
        /// Actualizar estado del proveedor
        /// </summary>
        /// <param name="updEstPrv"></param>
        /// <returns></returns>
        [HttpPut("UpdateEstado", Name = "UpdateEstadoProveedor")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdmin")]
        public async Task<IActionResult> UpdateEstadoProveedor([FromBody] QueryUpdateEstadoPrv updEstPrv)
        {
            try
            {
                updEstPrv.CodUserUpdate = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                string pathFS = _configuration.GetSection("Path_FileServer_root")?.Value;
                var responseAction = await _proveedorService.PutProveedorAprobar(updEstPrv, pathFS, IdPathFS);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);

                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la actualización del proveedor. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Reenviar correo de firma de documentos al proveedor
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("ReenviarNotificacionFirma", Name = "ReenviarNotificacionFirmaProveedor")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdmin")]
        public async Task<IActionResult> ReenviarNotificacionFirmaProveedor([FromBody] QueryReenviarNotificacionFirmaProveedor request)
        {
            try
            {
                string pathFS = _configuration.GetSection("Path_FileServer_root")?.Value;
                var responseAction = await _proveedorService.ReenviarNotificacionFirma(request, pathFS, IdPathFS);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);

                if (!responseAction.FirstOrDefault().estado)
                    response.Status = 400;

                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la actualización del proveedor. Detalle: {e.Message}");
            }
        }

        [HttpDelete("DeleteDocumento", Name = "DeleteDocumentoOtros")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdminOrProv")]
        public async Task<IActionResult> DeleteDocumentoOtros([FromBody] QueryDeleteDocOther documento)
        {
            try
            {
                documento.CodUserUpdate = int.Parse(HttpContext.Items["UserID"]?.ToString());
                var responseAction = await _documentoService.DeleteDocumentoOther(documento);
                var response = new ApiResponse<List<ResponseActionUrl>>(responseAction, 200);

                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                else
                {
                    // Eliminamos el archivo del File Server
                    string pathFSRoot = _configuration.GetSection("Path_FileServer_root")?.Value;
                    string path = Path.Combine(pathFSRoot, responseAction[0].url);
                    //_filesProcess.RemoveFile(path);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la actualización del proveedor. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Actualizar estado del Documento
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("UpdateEstadoDocumento", Name = "UpdateEstadoDocumento")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdmin")]
        public async Task<IActionResult> UpdateEstadoDocumento([FromBody] QueryUpdateEstadoDocument data)
        {
            try
            {
                data.CodUserUpdate = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                var responseAction = await _proveedorService.PutUpdateEstadoDocument(data);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);

                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la actualización del proveedor. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Actualiza la traza del proveedor
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("UpdateTrazaInspect", Name = "UpdateTrazaInspect")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdmin")]
        public async Task<IActionResult> UpdateTrazaInspect([FromBody] QueryUpdateTrazaInspect data)
        {
            try
            {
                data.CodUserUpdate = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                var responseAction = await _proveedorService.PutUpdateTrazaInspect(data);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);

                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la actualización de la traza proveedor. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Envio Masivo de correos a proveedores
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost("SendMasive", Name = "SendMailMasivePrv")]
        [Consumes("multipart/form-data")]
        [Authorize(Policy = "ShouldBeAnAdmin")]
        public async Task<IActionResult> SendMailMasivePrv([FromForm] QuerySendMailMasive parameters)
        {
            QuerySendMailMasiveValidator val = new();
            var validationResult = val.Validate(parameters);

            if (!validationResult.IsValid)
            {
                return Ok(ErrorResponse.GetError(false, validationResult.ToString("|"), 400));
            }

            var responseAction = await _mailService.SendMailMasive(parameters);

            var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);

            if (!responseAction[0].estado)
            {
                response.Status = 400;
            }
            return Ok(response);
        }

        /// <summary>
        /// Consultar proveedor por Nit
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpGet("SearchByCodValidation", Name = "Search código Validation")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchByCodValidation([FromQuery] QuerySearchByCodValidation data)
        {
            try
            {
                var list = await _proveedorService.SearchByCodValidation(data);
                var entidad = list.FirstOrDefault();

                if (entidad != null && entidad.estado)
                {
                    var response = new ApiResponse<ResponseAction>(entidad, 200);
                    return Ok(response);
                }

                if (entidad != null && !entidad.estado)
                {
                    return Ok(ErrorResponse.GetError(false, entidad.mensaje, 400));
                }

                return Ok(ErrorResponse.GetError(false, "error", 400));
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Rennviar codigo de validación
        /// </summary>
        /// <returns></returns>
        [HttpPost("ReenviarCodigoValidacion", Name = "ReenviarCodigoValidacion")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> ReenviarCodigoValidacion([FromBody] QueryReenvioNotificacionCodigoValidacion par)
        {
            try
            {
                var responseAction = await _proveedorService.ReenviarNotificacionCodigoValidacion(par);

                var response = responseAction.FirstOrDefault();

                if (response is null)
                    throw new BusinessException($"La respuesta: {nameof(response)} es nula.");

                if (!response.estado)
                    return Ok(new ApiResponse<List<ResponseAction>>(responseAction, 400));

                return Ok(new ApiResponse<List<ResponseAction>>(responseAction, 200));
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para cambiar correo del proveedor
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [HttpPost("CambiarCorreo", Name = "CambiarCorreo")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> CambiarCorreo([FromBody] QueryCambiarCorreoProveedor par)
        {
            try
            {
                var responseAction = await _proveedorService.CambiarCorreo(par);

                var response = responseAction.FirstOrDefault();

                if (response is null)
                    throw new BusinessException($"La respuesta: {nameof(response)} es nula.");

                if (!response.estado)
                    return Ok(new ApiResponse<List<ResponseAction>>(responseAction, 400));

                return Ok(new ApiResponse<List<ResponseAction>>(responseAction, 200));
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        private async Task<object> UpdateProveedores(QueryCreateProveedores parameters)
        {
            try
            {
                var proveedor = JsonConvert.DeserializeObject<ProveedorDto>(parameters.Proveedor);

                DataModelSignalResponse<DataProveedores> dataModel = new()
                {
                    Id = proveedor.PrvNit,
                    Data = new() { Progress = 20 }
                };
                await _hubContext.Clients.All.SendAsync("UpdateProveedorEmit", dataModel);

                SignalParams<ProveedorHub, DataProveedores> signalParam = new()
                {
                    DataModel = dataModel,
                    HubContext = _hubContext
                };

                ProveedoresValidator val = new();
                var validationResult = val.Validate(proveedor, e => e.IncludeRuleSets("UpdateValidation"));

                if (!validationResult.IsValid)
                {
                    signalParam.DataModel.Data.Progress = 100;
                    await _hubContext.Clients.All.SendAsync("UpdateProveedorEmit", signalParam.DataModel);
                    return ErrorResponse.GetError(false, validationResult.ToString("|"), 400);
                }

                List<DocumentoDto> listDocumentsInfo = JsonConvert.DeserializeObject<List<DocumentoDto>>(parameters.ListDocuments);
                List<List<FileResponse>> fileListCreated = new();
                List<PrvDocumento> listDocuments = new(); // listado de archivos que se actualizarán en DB
                List<PrvDocumento> listDocumentsOthers = new(); // listado de archivos que se guardarán en DB

                // Obtenemos los archivos existentes en la DB
                List<PrvDocumento> documentosAntiguos = await _documentoService.GetPrvDocumento(proveedor.Id);
                List<Proveedores> proveedorAntiguo = await _proveedorService.GetProveedorPorID(proveedor.Id, IdPathFS);
                List<int> idsDocAntiguosAEliminar = new();

                string fileFolder = _configuration.GetSection("Folder_Archivos")?.Value;

                #region Guardado de Documentos

                // Guardar archivos FilesRequired
                if (parameters.FilesRequired != null)
                {
                    FormDataImagen dataFile = new()
                    {
                        Carpeta = fileFolder,
                        Files = parameters.FilesRequired,
                        IdPathFileServer = IdPathFS
                    };

                    //List<FileResponse> filesReq = await _filesProcess.GetFilesCreated(dataFile);
                    List<FileResponse> filesReq = new() { };
                    fileListCreated.Add(filesReq);

                    // Procesamos los archivos para sacar la informacion de cada uno y guardar en la BD
                    foreach (var item in filesReq)
                    {
                        string nombre = item.NombreOriginal;
                        int codDocumento = int.Parse(nombre.Split("-")[0]);
                        string nombreDocumento = nombre.Split("-")[1];
                        int tamano = int.Parse(item.Size.ToString());

                        // Buscamos info adicional de los documentos
                        DocumentoDto doc = listDocumentsInfo.Find(x => x.Id == codDocumento);

                        PrvDocumento documento = new()
                        {
                            PrvdSizeDocument = tamano,
                            PrvdExtDocument = item.Extension,
                            PrvdUrlDocument = item.PathWebAbsolute,
                            PrvdUrlRelDocument = item.PathWebRelative,
                            PrvdNameDocument = $"{item.NombreInterno}{item.Extension}",
                            PrvdOriginalNameDocument = $"{nombreDocumento}{item.Extension}",
                            PrvdCodDocumento = doc.Id,
                            PrvdExpedicion = doc.VigenciaDate
                        };

                        idsDocAntiguosAEliminar.Add(codDocumento);
                        listDocuments.Add(documento);
                    }
                }

                signalParam.DataModel.Data.Progress = 30;
                await _hubContext.Clients.All.SendAsync("UpdateProveedorEmit", signalParam.DataModel);

                // Guardar archivos OthersFiles
                if (parameters.OthersFiles != null)
                {
                    FormDataImagen dataFile = new()
                    {
                        Carpeta = fileFolder,
                        Files = parameters.OthersFiles,
                        IdPathFileServer = IdPathFS
                    };

                    //List<FileResponse> othersFiles = await _filesProcess.GetFilesCreated(dataFile);
                    List<FileResponse> othersFiles = new() { };
                    fileListCreated.Add(othersFiles);

                    // Procesamos los archivos para sacar la informacion de cada uno y guardar en la BD
                    foreach (var item in othersFiles)
                    {
                        string nombre = item.NombreOriginal;
                        string codigoDocOthers = _configuration.GetSection("Codigo_Documento_Otros").Value;
                        int codDocumento = int.Parse(codigoDocOthers); // Otros
                        int tamano = int.Parse(item.Size.ToString());

                        PrvDocumento documento = new()
                        {
                            PrvdSizeDocument = tamano,
                            PrvdExtDocument = item.Extension,
                            PrvdUrlDocument = item.PathWebAbsolute,
                            PrvdUrlRelDocument = item.PathWebRelative,
                            PrvdNameDocument = $"{item.NombreInterno}{item.Extension}",
                            PrvdOriginalNameDocument = nombre,
                            PrvdCodDocumento = codDocumento
                        };

                        listDocumentsOthers.Add(documento);
                    }
                }

                #endregion Guardado de Documentos

                signalParam.DataModel.Data.Progress = 40;
                await _hubContext.Clients.All.SendAsync("UpdateProveedorEmit", signalParam.DataModel);

                string pathFS = _configuration.GetSection("Path_FileServer")?.Value;
                string pathFSWeb = _configuration.GetSection("Path_WebFileServer")?.Value;

                var responseAction = await _proveedorService.PutActualizar(proveedor, listDocuments, listDocumentsOthers, pathFS, pathFSWeb, signalParam, IdPathFS);

                var response = new ApiResponse<List<ResponseActionUrl>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                else
                {
                    string pathFSRoot = _configuration.GetSection("Path_FileServer_root")?.Value;

                    // Eliminamos archivos antiguos del File Server
                    var docsAEliminar = documentosAntiguos.FindAll(x => idsDocAntiguosAEliminar.Contains(x.PrvdCodDocumento));
                    string path = string.Empty;

                    foreach (var docAntg in docsAEliminar)
                    {
                        path = Path.Combine(pathFSRoot, docAntg.PrvdUrlRelDocument);
                        //_filesProcess.RemoveFile(path);
                    }

                    // Eliminamos archivo principal antiguo del File Server
                    path = Path.Combine(pathFSRoot, proveedorAntiguo[0].PrvUrlPdfRel);
                    //_filesProcess.RemoveFile(path);
                }

                signalParam.DataModel.Data.Progress = 100;
                await _hubContext.Clients.All.SendAsync("UpdateProveedorEmit", signalParam.DataModel);
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error al intentar insertar registro. Detalle: {e.Message}");
            }
        }

        #region Inspektor

        /// <summary>
        /// Validar si el proveedor existe en listas negras
        /// </summary>
        /// <param name="queryValidate"></param>
        /// <returns></returns>
        [HttpPost("ValidateBlackList", Name = "ValidateBlackListProveedor")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateBlackListProveedor([FromBody] QueryValidateBlackList queryValidate)
        {
            try
            {
                var resp = await _proveedorService.ValidateBlackList(queryValidate.NumIdenti, queryValidate.Nombre);
                var response = new ApiResponse<List<ProveedorInspektor>>(resp, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Método para definir si, un proveedor rechazado por Inspektor, se aprueba para registro o no.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("AprobarDesaprobarInspektor", Name = "AprobarDesaprobarInspektor")]
        [Consumes("application/json")]
        [Authorize(Policy = "ShouldBeAnAdmin")]
        public async Task<IActionResult> AprobarDesaprobarInspektor([FromBody] QueryAprobarDesaprobarInspektor data)
        {
            try
            {
                data.CodUserUpdate = HttpContext.Items.ContainsKey("UserID") ? HttpContext.Items["UserID"]?.ToString() : "0";
                var responseAction = await _proveedorService.AprobarDesaprobarInspektor(data);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);

                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la Aprobación del proveedor. Detalle: {e.Message}");
            }
        }

        #endregion Inspektor
    }
}