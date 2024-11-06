using Core.CustomEntities;
using Core.Entities;
using Core.Enumerations;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelResponse;
using Core.Options;
using Core.QueryFilters;
using Core.Tools;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Core.ModelProcess
{
    public class AdobeSignService : IAdobeSignService
    {
        public string BaseUrl { get; set; }
        public BaseUriClass BaseUri { get; set; }
        public string AccessToken { get; set; }
        public string UrlAgreements { get; set; }
        public string UrlDownloadDoc { get; set; }
        public string UrlTransientDoc { get; set; }
        public string UrlValidateDoc { get; set; }
        public string Tipo_Agreement { get; set; }

        private readonly IUnitOfWork _unitOfWork;
        private readonly AdobeSignOptions _options;

        public AdobeSignService(IOptions<AdobeSignOptions> options, IUnitOfWork unitOfWork)
        {
            _options = options.Value;
            _unitOfWork = unitOfWork;
            string baseUrl = _options.AdobeBaseUrl;
            BaseUrl = string.Concat(baseUrl, _options.BaseUriRelativeUrl);
            AccessToken = _options.AdobeAccessToken;
            BaseUri = GetBaseURIsAsync().Result;
            UrlAgreements = string.Concat(BaseUri.ApiAccessPoint, _options.AgreementsRelativeUrl);
            UrlDownloadDoc = string.Concat(BaseUri.ApiAccessPoint, _options.DownloadDocRelativeUrl);
            UrlTransientDoc = string.Concat(BaseUri.ApiAccessPoint, _options.TransientDocRelativeUrl);
            UrlValidateDoc = string.Concat(BaseUri.ApiAccessPoint, _options.ValidateDocRelativeUrl);
        }

        private async Task<BaseUriClass> GetBaseURIsAsync()
        {
            try
            {
                using HttpClient cliente = new HttpClient();
                string mensajeRespuesta = string.Empty;

                cliente.DefaultRequestHeaders.Add("Authorization", $"Bearer {AccessToken}");

                using HttpResponseMessage resultado = await cliente.GetAsync(BaseUrl);
                mensajeRespuesta = await resultado.Content.ReadAsStringAsync();

                if (resultado.IsSuccessStatusCode)
                {
                    BaseUriClass baseUri = JsonConvert.DeserializeObject<BaseUriClass>(mensajeRespuesta);
                    return baseUri;
                }
                else
                {
                    adobeErrorResponse errorResponse = JsonConvert.DeserializeObject<adobeErrorResponse>(mensajeRespuesta);
                    await _unitOfWork.AdobeSignRepository.GuardarLogErrorAdobeSign(errorResponse, "GetBaseUri");
                    throw new BusinessException("Ocurrió un error al intentar obtener ApiAccessPoint");
                }
            }
            catch (Exception e)
            {

                adobeErrorResponse adobeError = new adobeErrorResponse()
                {
                    Code = "BusinessException",
                    Message = e.Message
                };
                await _unitOfWork.AdobeSignRepository.GuardarLogErrorAdobeSign(adobeError, "GetBaseUri");
                throw new BusinessException("Error: " + e.Message);
            }
        }

        public async Task<TransientDocument> TransientDocumentsAsync(string filePath, int id)
        {
            string nombreProceso = string.Concat("TransientDocument", "-", Tipo_Agreement);
            try
            {
                FileInfo fileUp = new FileInfo(filePath);

                if (fileUp.Exists)
                {
                    string contentType = Funciones.GetContentType(filePath);
                    string mensajeRespuesta = string.Empty;

                    using HttpClient cliente = new HttpClient();
                    cliente.DefaultRequestHeaders.Add("Authorization", $"Bearer {AccessToken}");

                    // Consumimos la API Adobe para almacenar los archivos
                    MultipartFormDataContent data = new MultipartFormDataContent();

                    FileStream fileStream = fileUp.OpenRead();

                    try
                    {
                        HttpContent content;
                        content = new StreamContent(fileStream, (int)fileStream.Length);
                        content.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
                        content.Headers.ContentLength = fileUp.Length;
                        data.Add(content, "File", fileUp.Name);

                        using HttpResponseMessage resultado = await cliente.PostAsync(UrlTransientDoc, data);
                        mensajeRespuesta = await resultado.Content.ReadAsStringAsync();

                        if (resultado.IsSuccessStatusCode)
                        {
                            // Respuesta de la API 200
                            TransientDocument transientResponse = JsonConvert.DeserializeObject<TransientDocument>(mensajeRespuesta);
                            await fileStream.DisposeAsync();
                            fileStream.Close();
                            return transientResponse;
                        }
                        else
                        {
                            await fileStream.DisposeAsync();
                            fileStream.Close();
                            adobeErrorResponse errorResponse = JsonConvert.DeserializeObject<adobeErrorResponse>(mensajeRespuesta);
                            await _unitOfWork.AdobeSignRepository.GuardarLogErrorAdobeSign(errorResponse, nombreProceso, id);
                            throw new BusinessException($"Ocurrió un error al intentar subir Documento: código de error: {errorResponse.Code}, {errorResponse.Message}");
                        }
                    }
                    catch (Exception e)
                    {
                        await fileStream.DisposeAsync();
                        fileStream.Close();
                        adobeErrorResponse adobeError = new adobeErrorResponse()
                        {
                            Code = "BusinessException",
                            Message = e.Message
                        };
                        await _unitOfWork.AdobeSignRepository.GuardarLogErrorAdobeSign(adobeError, nombreProceso, id);
                        throw new BusinessException("Error: " + e.Message);
                    }
                }
                else
                {
                    throw new BusinessException($"Error: no existe el archivo, en la ruta especificada");
                }
            }
            catch (Exception e)
            {
                adobeErrorResponse adobeError = new adobeErrorResponse()
                {
                    Code = "BusinessException",
                    Message = e.Message
                };
                await _unitOfWork.AdobeSignRepository.GuardarLogErrorAdobeSign(adobeError, nombreProceso, id);
                throw new BusinessException("Error: " + e.Message);
            }
        }

        public async Task<AgreementsResponse> AgreementsAsync(string transientDocumentId, string nombreAcuerdo, string emailProveedor, int id, string emailRteLegal)
        {
            string nombreProceso = string.Concat("Agreements", "-", Tipo_Agreement);
            try
            {
                string mensajeRespuesta = string.Empty;

                using HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Add("Authorization", $"Bearer {AccessToken}");

                Agreements agreements = null;

                switch (Tipo_Agreement)
                {
                    case AdbSign_TipoAcuerdo.Contrato:
                        // Construimos el objeto a pasar a la solicitud
                        agreements = new Agreements
                        {
                            fileInfos = new List<Fileinfo>
                            {
                                new Fileinfo {
                                    transientDocumentId = transientDocumentId
                                }
                            },
                            name = nombreAcuerdo,
                            /*ccs = new List<CCInfo>
                            {
                                new CCInfo()
                                {
                                    email = AdbSign_MailsCC.Mail_Syspotec
                                }
                            },*/
                            participantSetsInfo = new List<Participantsetsinfo_Agree> {
                                new Participantsetsinfo_Agree() { // Proveedor
                                    memberInfos = new List<Memberinfo_Agree> {
                                        new Memberinfo_Agree {
                                            email = emailProveedor
                                        }
                                    },
                                order = 1, // Primer firmante (le llega primero el documento para firmarlo)
                                role = "SIGNER"
                            },
                                new Participantsetsinfo_Agree() { // Rte. Legal
                                    memberInfos = new List<Memberinfo_Agree> {
                                        new Memberinfo_Agree {
                                            email = emailRteLegal
                                        }
                                    },
                                    order = 2, // Segundo firmante (le llega de segundo el documento para firmarlo)
                                    role = "SIGNER"
                                }
                            },
                            signatureType = "ESIGN",
                            state = "IN_PROCESS"
                        };
                        break;
                    case AdbSign_TipoAcuerdo.Proveedor:
                        // Construimos el objeto a pasar a la solicitud
                        agreements = new Agreements
                        {
                            fileInfos = new List<Fileinfo>
                            {
                                new Fileinfo {
                                    transientDocumentId = transientDocumentId
                                }
                            },
                            name = nombreAcuerdo,
                            /*ccs = new List<CCInfo>
                            {
                                new CCInfo()
                                {
                                    email = AdbSign_MailsCC.Mail_Syspotec
                                }
                            },*/
                            participantSetsInfo = new List<Participantsetsinfo_Agree>{
                                new Participantsetsinfo_Agree() {
                                    memberInfos = new List<Memberinfo_Agree> {
                                        new Memberinfo_Agree {
                                            email = emailProveedor
                                        }
                                    },
                                    order = 1,
                                    role = "SIGNER"
                                }
                            },
                            signatureType = "ESIGN",
                            state = "IN_PROCESS"
                        };
                        break;
                    default:
                        break;
                }

                string bodyContent = JsonConvert.SerializeObject(agreements);
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(bodyContent);
                ByteArrayContent byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                using HttpResponseMessage resultado = await cliente.PostAsync(UrlAgreements, byteContent);
                mensajeRespuesta = await resultado.Content.ReadAsStringAsync();

                if (resultado.IsSuccessStatusCode)
                {
                    AgreementsResponse response = JsonConvert.DeserializeObject<AgreementsResponse>(mensajeRespuesta);
                    return response;
                }
                else
                {
                    adobeErrorResponse errorResponse = JsonConvert.DeserializeObject<adobeErrorResponse>(mensajeRespuesta);
                    await _unitOfWork.AdobeSignRepository.GuardarLogErrorAdobeSign(errorResponse, nombreProceso, id);
                    throw new BusinessException($"Ocurrió un error al intentar enviar Documento para firmar: código de error: {errorResponse.Code}, {errorResponse.Message}");
                }
            }
            catch (Exception e)
            {
                adobeErrorResponse adobeError = new adobeErrorResponse()
                {
                    Code = "BusinessException",
                    Message = e.Message
                };
                await _unitOfWork.AdobeSignRepository.GuardarLogErrorAdobeSign(adobeError, nombreProceso, id);
                throw new BusinessException("Error: " + e.Message);
            }
        }

        public async Task<bool> DownloadDocumentAsync(QueryDownloadPdfAdb queryDownload, string pathFs, int IdPathFS)
        {
            string nombreProceso = string.Concat("DownloadDocument", "-", Tipo_Agreement);
            bool wasCreated = false;
            try
            {
                string mensajeRespuesta = string.Empty;

                using HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Add("Authorization", $"Bearer {AccessToken}");

                string url = UrlDownloadDoc.Replace("{idDocument}", queryDownload.IdDocumento);
                using HttpResponseMessage resultado = await cliente.GetAsync(url);

                if (resultado.IsSuccessStatusCode)
                {
                    //Stream stream = await resultado.Content.ReadAsStreamAsync();
                    byte[] fileArray = await resultado.Content.ReadAsByteArrayAsync();

                    switch (Tipo_Agreement)
                    {
                        case AdbSign_TipoAcuerdo.Contrato:
                            List<Contrato> contrato = await ConsultarUrlContrato(queryDownload.Id);

                            if (!(contrato is null))
                            {
                                string rutaRelativaPdf = contrato[0].ContUrlPdf;
                                string nombreArchivo = rutaRelativaPdf.Split("/")[2];
                                wasCreated = Funciones.SaveStreamAsFileAsync(pathFs, fileArray, nombreArchivo);
                            }
                            break;
                        case AdbSign_TipoAcuerdo.Proveedor:
                            List<Proveedores> proveedor = await ConsultarUrlProveedor(queryDownload.Id, IdPathFS);

                            if (!(proveedor is null))
                            {
                                string rutaRelativaPdf = proveedor[0].PrvUrlPdfRel;
                                string nombreArchivo = rutaRelativaPdf.Split("/")[1];
                                wasCreated = Funciones.SaveStreamAsFileAsync(pathFs, fileArray, nombreArchivo);
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    mensajeRespuesta = await resultado.Content.ReadAsStringAsync();
                    adobeErrorResponse errorResponse = JsonConvert.DeserializeObject<adobeErrorResponse>(mensajeRespuesta);
                    await _unitOfWork.AdobeSignRepository.GuardarLogErrorAdobeSign(errorResponse, nombreProceso);
                    throw new BusinessException($"Ocurrió un error al intentar descargar Documento: código de error: {errorResponse.Code}, {errorResponse.Message}");
                }
            }
            catch (Exception e)
            {
                adobeErrorResponse adobeError = new adobeErrorResponse()
                {
                    Code = "BusinessException",
                    Message = e.Message
                };
                await _unitOfWork.AdobeSignRepository.GuardarLogErrorAdobeSign(adobeError, nombreProceso);
                throw new BusinessException("Error: " + e.Message);
            }

            return wasCreated;
        }

        public async Task<ValidateDocument> ValidateDocumentAsync(string idDocument)
        {
            string nombreProceso = string.Concat("ValidateDocument", "-", Tipo_Agreement);
            try
            {
                string mensajeRespuesta = string.Empty;

                using HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Add("Authorization", $"Bearer {AccessToken}");

                string url = UrlValidateDoc.Replace("{idDocument}", idDocument);
                using HttpResponseMessage resultado = await cliente.GetAsync(url);
                mensajeRespuesta = await resultado.Content.ReadAsStringAsync();

                if (resultado.IsSuccessStatusCode)
                {
                    ValidateDocument valDoc = JsonConvert.DeserializeObject<ValidateDocument>(mensajeRespuesta);
                    return valDoc;
                }
                else
                {
                    adobeErrorResponse errorResponse = JsonConvert.DeserializeObject<adobeErrorResponse>(mensajeRespuesta);
                    await _unitOfWork.AdobeSignRepository.GuardarLogErrorAdobeSign(errorResponse, nombreProceso);
                    throw new BusinessException($"Ocurrió un error al intentar validar Documento: código de error: {errorResponse.Code}, {errorResponse.Message}");
                }
            }
            catch (Exception e)
            {
                adobeErrorResponse adobeError = new adobeErrorResponse()
                {
                    Code = "BusinessException",
                    Message = e.Message
                };
                await _unitOfWork.AdobeSignRepository.GuardarLogErrorAdobeSign(adobeError, nombreProceso);
                throw new BusinessException("Error: " + e.Message);
            }
        }

        private async Task<List<Proveedores>> ConsultarUrlProveedor(int idPrv, int IdPathFS)
        {
            return await _unitOfWork.ProveedorRepository.GetProveedorPorID(idPrv, IdPathFS);
        }

        private async Task<List<Contrato>> ConsultarUrlContrato(int id, int IdPathFS = 0)
        {
            return await _unitOfWork.ContratoRepository.SearchById(id);
        }
    }
}
