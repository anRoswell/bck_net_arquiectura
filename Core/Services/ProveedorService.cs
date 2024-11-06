namespace Core.Services
{
    using Core.CustomEntities;
    using Core.DTOs;
    using Core.Entities;
    using Core.Enumerations;
    using Core.Exceptions;
    using Core.HubConfig;
    using Core.Interfaces;
    using Core.ModelProcess;
    using Core.ModelResponse;
    using Core.QueryFilters;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProveedorService : IProveedorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAdobeSignService _adobeSignService;
        private readonly INotificationsService _notificationsService;


        public ProveedorService(IUnitOfWork unitOfWork, IAdobeSignService adobeSignService, INotificationsService notificationsService)
        {
            _unitOfWork = unitOfWork;
            _adobeSignService = adobeSignService;
            _notificationsService = notificationsService;
        }

        public async Task<ParametrosProveedor> GetParametros(int idProveedor)
        {
            ParametrosProveedor paramsProv = new ParametrosProveedor
            {
                Ciudades = await _unitOfWork.ParametrosInicialesRepository.GetCiudades(),
                Departamentos = await _unitOfWork.ParametrosInicialesRepository.GetDepartamentos(),
                Empresas = await _unitOfWork.ParametrosInicialesRepository.GetEmpresas(),
                CondicionesPagos = await _unitOfWork.ParametrosInicialesRepository.GetCondicionesPago(),
                ProductoServicios = await _unitOfWork.ParametrosInicialesRepository.GetProductosServicios(),
                TipoProveedores = await _unitOfWork.ParametrosInicialesRepository.GetTipoProveedores(),
                Documentos = await _unitOfWork.ParametrosInicialesRepository.GetDocumentos(),
                Bancos = await _unitOfWork.ParametrosInicialesRepository.GetBancos(),
                ListaRestrictivas = await _unitOfWork.ParametrosInicialesRepository.GetListaRestrictivas(idProveedor),
                EstadosProveedor = await _unitOfWork.ParametrosInicialesRepository.GetEstadoProveedores()
            };

            return paramsProv;
        }

        public async Task<ParametrosGestionProveedor> GetParametrosGestionProveedor()
        {
            ParametrosGestionProveedor paramsGstPrv = new ParametrosGestionProveedor
            {
                PrvProdServ = await _unitOfWork.ParametrosInicialesRepository.GetProductosServicios(),
                EstadosProveedor = await _unitOfWork.ParametrosInicialesRepository.GetEstadoProveedores(),
            };

            return paramsGstPrv;
        }

        public async Task<List<ResponseAction>> SearchByCodValidation(QuerySearchByCodValidation data)
        {
            return await _unitOfWork.ProveedorRepository.SearchByCodValidation(data);
        }

        public async Task<List<Proveedores>> GetFiltroProveedor(string idLocalidad, int idCategoriaServicio, int idEstado, string razonSocial, string nit, int idPathFileServer)
        {
            return await _unitOfWork.ProveedorRepository.GetFiltroProveedor(idLocalidad, idCategoriaServicio, idEstado, razonSocial, nit, idPathFileServer);
        }

        public async Task<ProveedorDetalle> GetProveedorDetallePorID(int id, int idPathFileServer)
        {
            ProveedorDetalle querySelect = new ProveedorDetalle
            {
                Proveedores = await GetProveedorPorID(id, idPathFileServer),
                Socios = await _unitOfWork.PrvSociosRepository.GetSocio(id),
                Referencias = await _unitOfWork.PrvReferenciaRepository.GetReferencia(id),
                prvEmpresasSelecteds = await _unitOfWork.PrvEmpresasSelectedRepository.GetEmpresasSelected(id),
                prvProdServSelecteds = await _unitOfWork.PrvProdServSelectedRepository.GetPrvProdServSelected(id),
                prvDocumentos = await _unitOfWork.PrvDocumentoRepository.GetPrvDocumento(id)
            };
            return querySelect;
        }

        public async Task<List<Proveedores>> GetProveedores(int idPathFileServer)
        {
            return await _unitOfWork.ProveedorRepository.GetProveedores(idPathFileServer);
        }

        public async Task<List<Proveedores>> GetProveedorPorID(int id, int idPathFileServer)
        {
            return await _unitOfWork.ProveedorRepository.GetProveedorPorID(id, idPathFileServer);
        }

        public async Task<List<Proveedores>> GetProveedorPorNit(string nit, int idPathFileServer)
        {
            return await _unitOfWork.ProveedorRepository.GetProveedorPorNit(nit, idPathFileServer);
        }

        public async Task<List<Requerimientos>> GetRequerimientosProveedor(int idProveedor)
        {
            return await _unitOfWork.ProveedorRepository.GetRequerimientosProveedor(idProveedor);
        }

        public async Task<List<ResponseAction>> ActualizarUrlPdf(int? id, string url)
        {
            return await _unitOfWork.ProveedorRepository.ActualizarUrlPdf(id, url);
        }

        public async Task<List<ResponseActionUrl>> PostCrear(ProveedorDto proveedor, List<PrvDocumento> documentos, string Generatedpassword, string pathFs, string pathFSWeb, SignalParams<ProveedorHub, DataProveedores> signalParam, int idPathFileServer)
        {
            var response = await _unitOfWork.ProveedorRepository.PostCrear(proveedor, documentos, Generatedpassword, signalParam);

            if (response != null &&
                response.Any() &&
                response.FirstOrDefault() != null &&
                response.FirstOrDefault().estado)
            {
                var firstResponse = response.FirstOrDefault();

                List<Proveedores> proveedores = await GetProveedorPorID(firstResponse.Id ?? 0, idPathFileServer);

                var firstProveedorFinded = proveedores.FirstOrDefault();

                var listadoSocios = await _unitOfWork.PrvSociosRepository.GetSocio(firstResponse.Id ?? 0);
                var listadoRefs = await _unitOfWork.PrvReferenciaRepository.GetReferencia(firstResponse.Id ?? 0);
                var listadoEmps = await _unitOfWork.PrvEmpresasSelectedRepository.GetEmpresasSelected(firstResponse.Id ?? 0);
                var listadoProdServs = await _unitOfWork.PrvProdServSelectedRepository.GetPrvProdServSelected(firstResponse.Id ?? 0);

                if (proveedores.Count > 0)
                {
                    string urlFinal = ProveedoresProcess.generarPdfHojadeVida(firstProveedorFinded, listadoSocios, listadoRefs, listadoEmps, listadoProdServs, pathFs, firstResponse.Id.ToString());
                    firstResponse.url = pathFSWeb + urlFinal;
                    await ActualizarUrlPdf(firstResponse.Id, urlFinal);
                }

                await _notificationsService.PushSocket(
                    new DataNotifications
                    {
                        Method = HubConectionsMethods.Notification,
                        Action = HubConectionsActions.CreateProveedor
                    });
            }
            return response;
        }

        public async Task<List<ResponseActionUrl>> PutActualizar(ProveedorDto proveedor, List<PrvDocumento> documentos, List<PrvDocumento> listDocumentsOthers, string pathFs, string pathFSWeb, SignalParams<ProveedorHub, DataProveedores> signalParam, int idPathFileServer)
        {
            var response = await _unitOfWork.ProveedorRepository.PutActualizar(proveedor, documentos, listDocumentsOthers, signalParam, idPathFileServer);

            if (response != null &&
                response.Any() &&
                response.FirstOrDefault() != null &&
                response.FirstOrDefault().estado)
            {
                var firstResponse = response.FirstOrDefault();

                List<Proveedores> proveedores = await GetProveedorPorID(firstResponse.Id ?? 0, idPathFileServer);

                var firstProveedorFinded = proveedores.FirstOrDefault();

                var listadoSocios = await _unitOfWork.PrvSociosRepository.GetSocio(firstResponse.Id ?? 0);
                var listadoRefs = await _unitOfWork.PrvReferenciaRepository.GetReferencia(firstResponse.Id ?? 0);
                var listadoEmps = await _unitOfWork.PrvEmpresasSelectedRepository.GetEmpresasSelected(firstResponse.Id ?? 0);
                var listadoProdServs = await _unitOfWork.PrvProdServSelectedRepository.GetPrvProdServSelected(firstResponse.Id ?? 0);

                if (proveedores.Count > 0)
                {
                    string urlFinal = ProveedoresProcess.generarPdfHojadeVida(firstProveedorFinded, listadoSocios, listadoRefs, listadoEmps, listadoProdServs, pathFs, firstResponse.Id.ToString());
                    firstResponse.url = pathFSWeb + urlFinal;
                    await ActualizarUrlPdf(firstResponse.Id, urlFinal);
                }

                await _notificationsService.PushSocket(
                    new DataNotifications
                    {
                        Method = HubConectionsMethods.Notification,
                        Action = HubConectionsActions.UpdateProveedor
                    });
            }
            return response;
        }

        public async Task<List<ResponseAction>> PutUpdateEstadoDocument(QueryUpdateEstadoDocument data)
        {
            return await _unitOfWork.ProveedorRepository.PutUpdateEstadoDocument(data);
        }

        public async Task<List<ResponseAction>> PutProveedorAprobar(QueryUpdateEstadoPrv updEstPrv, string pathFs, int idPathFileServer)
        {
            List<ResponseAction> response;
            List<Proveedores> proveedores = await GetProveedorPorNit(updEstPrv.PrvNit ?? "", idPathFileServer);
            var listadoSocios = await _unitOfWork.PrvSociosRepository.GetSocio(proveedores[0].Id);
            var listadoRefs = await _unitOfWork.PrvReferenciaRepository.GetReferencia(proveedores[0].Id);
            var listadoEmps = await _unitOfWork.PrvEmpresasSelectedRepository.GetEmpresasSelected(proveedores[0].Id);
            var listadoProdServs = await _unitOfWork.PrvProdServSelectedRepository.GetPrvProdServSelected(proveedores[0].Id);

            if (proveedores.Count > 0)
            {
                try
                {
                    if (updEstPrv.Estado == 2) // si se está aprobando el proveedor
                    {
                        // Generamos proceso para Firmas con AdobeSign
                        //1-- Subimos el pdf generado
                        //, proveedores[0].PrvUrlPdfRel
                        string savedFilePath = Path.Combine(pathFs, proveedores[0].PrvUrlPdfRel);
                        string urlFinal = ProveedoresProcess.generarPdfHojadeVida(proveedores[0], listadoSocios, listadoRefs, listadoEmps, listadoProdServs, pathFs, proveedores[0].Id.ToString(), proveedores[0].PrvUrlPdfRel);

                        _adobeSignService.Tipo_Agreement = AdbSign_TipoAcuerdo.Proveedor;
                        TransientDocument transientPdfDocument = await _adobeSignService.TransientDocumentsAsync(savedFilePath, id: proveedores[0].Id);

                        if (!(transientPdfDocument is null))
                        {
                            // Registramos evento Adobde Sign en la tabla [prv].[PrvAdobeSign] en Estado 1 (UPLOADED)
                            response = await _unitOfWork.AdobeSignRepository.GuardarTrazaAdobeSign_Prv(proveedores[0].Id, (int)AdobeSign_Estados.UPLOADED, transientPdfDocument.TransientDocumentId, null);

                            if (response[0].estado)
                            {
                                //2-- Enviamos el pdf para firma
                                AgreementsResponse sendAgreements = await _adobeSignService.AgreementsAsync(
                                    transientDocumentId: transientPdfDocument.TransientDocumentId,
                                    nombreAcuerdo: AdobeSign_NombreAcuerdos.Documento_Proveedor,
                                    emailProveedor: proveedores[0].PrvMail,
                                    id: proveedores[0].Id
                                );

                                if (!(sendAgreements is null))
                                {
                                    // Registramos evento Adobde Sign en la tabla [prv].[PrvAdobeSign] en Estado 2 (OUT_FOR_SIGNATURE)
                                    response = await _unitOfWork.AdobeSignRepository.GuardarTrazaAdobeSign_Prv(proveedores[0].Id, (int)AdobeSign_Estados.OUT_FOR_SIGNATURE, sendAgreements.Id, null);

                                    if (response[0].estado)
                                    {
                                        // Actualizamos el estado del proveedor a 'Activo (Pendiente firma)'
                                        response = await _unitOfWork.ProveedorRepository.PutProveedorAprobar(updEstPrv);
                                    }
                                }
                                else
                                {
                                    throw new BusinessException("No se pudo generar documento para firma, por favor contactar al administrador del sistema.");
                                }
                            }
                        }
                        else
                        {
                            throw new BusinessException("No se pudo cargar documento para firma, por favor contactar al administrador del sistema.");
                        }
                    }
                    else
                    {
                        // Actualizamos el estado del proveedor
                        response = await _unitOfWork.ProveedorRepository.PutProveedorAprobar(updEstPrv);
                    }
                }
                catch (Exception e)
                {
                    throw new BusinessException($"Error: {e.Message}");
                }
            }
            else
            {
                throw new BusinessException("El proveedor no es válido");
            }

            return response;
        }

        public async Task<List<ResponseAction>> ReenviarNotificacionFirma(QueryReenviarNotificacionFirmaProveedor query, string pathFs, int idPathFileServer)
        {
            List<ResponseAction> response = new List<ResponseAction>();
            List<Proveedores> proveedores = await GetProveedorPorNit(query.PrvNit ?? "", idPathFileServer);
            if (proveedores.Any())
            {
                Proveedores proveedor = proveedores.FirstOrDefault();

                try
                {
                    string savedFilePath = Path.Combine(pathFs, proveedor.PrvUrlPdfRel);

                    _adobeSignService.Tipo_Agreement = AdbSign_TipoAcuerdo.Proveedor;
                    TransientDocument transientPdfDocument = await _adobeSignService.TransientDocumentsAsync(savedFilePath, id: proveedor.Id);

                    if (!(transientPdfDocument is null))
                    {
                        // Registramos evento Adobde Sign en la tabla [prv].[PrvAdobeSign] en Estado 1 (UPLOADED)
                        response = await _unitOfWork.AdobeSignRepository.GuardarTrazaAdobeSign_Prv(proveedor.Id, (int)AdobeSign_Estados.UPLOADED, transientPdfDocument.TransientDocumentId, null);

                        if (response.FirstOrDefault().estado)
                        {
                            //2-- Enviamos el pdf para firma
                            AgreementsResponse sendAgreements = await _adobeSignService.AgreementsAsync(
                                transientDocumentId: transientPdfDocument.TransientDocumentId,
                                nombreAcuerdo: AdobeSign_NombreAcuerdos.Documento_Proveedor,
                                emailProveedor: proveedor.PrvMail,
                                id: proveedor.Id
                            );

                            if (!(sendAgreements is null))
                            {
                                // Registramos evento Adobde Sign en la tabla [prv].[PrvAdobeSign] en Estado 2 (OUT_FOR_SIGNATURE)
                                response = await _unitOfWork.AdobeSignRepository.GuardarTrazaAdobeSign_Prv(proveedor.Id, (int)AdobeSign_Estados.OUT_FOR_SIGNATURE, sendAgreements.Id, null);
                            }
                            else
                            {
                                throw new BusinessException("No se pudo generar documento para firma, por favor contactar al administrador del sistema.");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new BusinessException($"Error: {e.Message}");
                }
            }

            return response;
        }

        public async Task<List<ResponseAction>> PutUpdateTrazaInspect(QueryUpdateTrazaInspect data)
        {
            return await _unitOfWork.ProveedorRepository.PutUpdateTrazaInspect(data);
        }

        public async Task<List<ProveedorInspektor>> ValidateBlackList(string numIdenti, string nombre)
        {
            return await _unitOfWork.ProveedorRepository.ValidateBlackList(numIdenti, nombre);
        }

        public async Task<List<ResponseAction>> AprobarDesaprobarInspektor(QueryAprobarDesaprobarInspektor data)
        {
            return await _unitOfWork.ProveedorRepository.AprobarDesaprobarInspektor(data);
        }

        public async Task<List<ResponseAction>> ReenviarNotificacionCodigoValidacion(QueryReenvioNotificacionCodigoValidacion data)
        {
            return await _unitOfWork.ProveedorRepository.ReenviarNotificacionCodigoValidacion(data);
        }

        public async Task<List<ResponseAction>> CambiarCorreo(QueryCambiarCorreoProveedor data)
        {
            return await _unitOfWork.ProveedorRepository.CambiarCorreoProveedor(data);
        }
    }
}