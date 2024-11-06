namespace Core.Services
{
    using Core.CustomEntities;
    using Core.Entities;
    using Core.Enumerations;
    using Core.Exceptions;
    using Core.HubConfig;
    using Core.Interfaces;
    using Core.ModelResponse;
    using Core.QueryFilters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TerceroService : ITerceroService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly INotificationsService _notificationsService;

        public TerceroService(IUnitOfWork unitOfWork, INotificationsService notificationsService)
        {
            _unitOfWork = unitOfWork;
            _notificationsService = notificationsService;
        }

        public async Task<ParametrosProveedor> GetParametros(int idProveedor)
        {
            ParametrosProveedor paramsProv = new()
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
            ParametrosGestionProveedor paramsGstPrv = new()
            {
                PrvProdServ = await _unitOfWork.ParametrosInicialesRepository.GetProductosServicios(),
                EstadosProveedor = await _unitOfWork.ParametrosInicialesRepository.GetEstadoProveedores(),
            };

            return paramsGstPrv;
        }

        public async Task<List<ResponseAction>> SearchByCodValidation(QuerySearchByCodValidation data)
        {
            return await _unitOfWork.TerceroRepository.SearchByCodValidation(data);
        }

        public async Task<List<TerceroDto>> GetFiltroTercero(string idLocalidad, int idCategoriaServicio, int idEstado, string razonSocial, string nit, int idPathFileServer)
        {
            return await _unitOfWork.TerceroRepository.GetFiltroTercero(idLocalidad, idCategoriaServicio, idEstado, razonSocial, nit, idPathFileServer);
        }

        public async Task<TerceroDetalle> GetTerceroDetallePorID(int id, int idPathFileServer)
        {
            TerceroDetalle querySelect = new()
            {
                Terceros = await GetTerceroPorID(id, idPathFileServer),
                Socios = await _unitOfWork.PrvSociosRepository.GetSocio(id),
                Referencias = await _unitOfWork.PrvReferenciaRepository.GetReferencia(id),
                prvEmpresasSelecteds = await _unitOfWork.PrvEmpresasSelectedRepository.GetEmpresasSelected(id),
                prvProdServSelecteds = await _unitOfWork.PrvProdServSelectedRepository.GetPrvProdServSelected(id),
                prvDocumentos = await _unitOfWork.PrvDocumentoRepository.GetPrvDocumento(id)
            };
            return querySelect;
        }

        public async Task<List<TerceroDto>> GetTerceros(int idPathFileServer)
        {
            return await _unitOfWork.TerceroRepository.GetTerceros(idPathFileServer);
        }

        public async Task<List<TerceroDto>> GetTerceroPorID(int id, int idPathFileServer)
        {
            return await _unitOfWork.TerceroRepository.GetTerceroPorID(id, idPathFileServer);
        }

        public async Task<List<TerceroDto>> GetTerceroPorNit(string nit, int idPathFileServer)
        {
            return await _unitOfWork.TerceroRepository.GetTerceroPorNit(nit, idPathFileServer);
        }

        public async Task<List<Requerimientos>> GetRequerimientosTercero(int idProveedor)
        {
            return await _unitOfWork.TerceroRepository.GetRequerimientosTercero(idProveedor);
        }

        public async Task<List<ResponseAction>> ActualizarUrlPdf(int? id, string url)
        {
            return await _unitOfWork.TerceroRepository.ActualizarUrlPdf(id, url);
        }

        public async Task<List<ResponseActionUrl>> PostCrear(TerceroCrearDto proveedor, List<PrvDocumento> documentos, string Generatedpassword, string pathFs, string pathFSWeb, SignalParams<ProveedorHub, DataProveedores> signalParam, int idPathFileServer)
        {
            var response = await _unitOfWork.TerceroRepository.PostCrear(proveedor, documentos, Generatedpassword, signalParam);

            if (response != null &&
                response.Any() &&
                response.FirstOrDefault() != null &&
                response.FirstOrDefault().estado)
            {
                await _notificationsService.PushSocket(
                    new DataNotifications
                    {
                        Method = HubConectionsMethods.Notification,
                        Action = HubConectionsActions.CreateProveedor
                    });
            }
            return response;
        }

        public async Task<List<ResponseActionUrl>> PutActualizar(TerceroActualizarDto proveedor, List<PrvDocumento> documentos, List<PrvDocumento> listDocumentsOthers, string pathFs, string pathFSWeb, SignalParams<ProveedorHub, DataProveedores> signalParam, int idPathFileServer)
        {
            var response = await _unitOfWork.TerceroRepository.PutActualizar(proveedor, documentos, listDocumentsOthers, signalParam, idPathFileServer);

            if (response != null &&
                response.Any() &&
                response.FirstOrDefault() != null &&
                response.FirstOrDefault().estado)
            {
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
            return await _unitOfWork.TerceroRepository.PutUpdateEstadoDocument(data);
        }

        public async Task<List<ResponseAction>> PutTerceroAprobar(QueryUpdateEstadoPrv updEstPrv, string pathFs, int idPathFileServer)
        {
            List<ResponseAction> response;
            List<TerceroDto> proveedores = await GetTerceroPorNit(updEstPrv.PrvNit ?? "", idPathFileServer);

            if (!proveedores.Any())
            {
                throw new BusinessException("El Tercero no es válido");
            }

            try
            {
                // Actualizamos el estado del proveedor
                response = await _unitOfWork.TerceroRepository.PutTerceroAprobar(updEstPrv);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }

            return response;
        }

        public async Task<List<ResponseAction>> PutUpdateTrazaInspect(QueryUpdateTrazaInspect data)
        {
            return await _unitOfWork.TerceroRepository.PutUpdateTrazaInspect(data);
        }

        public async Task<List<ProveedorInspektor>> ValidateBlackList(string numIdenti, string nombre)
        {
            return await _unitOfWork.TerceroRepository.ValidateBlackList(numIdenti, nombre);
        }

        public async Task<List<ResponseAction>> AprobarDesaprobarInspektor(QueryAprobarDesaprobarInspektor data)
        {
            return await _unitOfWork.TerceroRepository.AprobarDesaprobarInspektor(data);
        }

        public async Task<List<ResponseAction>> ReenviarNotificacionCodigoValidacion(QueryReenvioNotificacionCodigoValidacion data)
        {
            return await _unitOfWork.TerceroRepository.ReenviarNotificacionCodigoValidacion(data);
        }

        public async Task<List<ResponseAction>> CambiarCorreo(QueryCambiarCorreoProveedor data)
        {
            return await _unitOfWork.TerceroRepository.CambiarCorreoTercero(data);
        }
    }
}