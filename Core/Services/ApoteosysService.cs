using Core.CustomEntities;
using Core.Entities;
using Core.Interfaces;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public class ApoteosysService : IApoteosysService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApoteosysService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Facturación
        public async Task<List<Empresa>> GetParametrosIniciales(int idUser)
        {
            return await _unitOfWork.ParametrosInicialesRepository.GetEmpresasByUserApoteosys(idUser);
        }

        public async Task<List<Empresa>> GetParametrosInicialesDashboard(int idUser)
        {
            return await _unitOfWork.ParametrosInicialesRepository.GetEmpresasByUserDashboardApoteosys(idUser);
        }

        #region Facturas Por Pagar
        public async Task<FacturasXPagarDetalle> EstadoCuentasXPorPagarDetalleApoteosys(QuerySearchEstadoCuentasXPagarDetalle parameters)
        {
            FacturasXPagarDetalle factDte = new FacturasXPagarDetalle()
            {
                DetallesFactura = await _unitOfWork.ApoteosysRepository.EstadoCuentasXPorPagarDetalleApoteosys(parameters),
                Factura = await _unitOfWork.SispoRepository.EstadoCuentasXPorPagarApoteosysDte(parameters)
            };
            return factDte;
        }

        public async Task<FacturasXPagarDetalle> EstadoCuentasXPorPagarDetalleApoteosys_Reporte(QuerySearchEstadoCuentasXPagarDetalle parameters)
        {
            FacturasXPagarDetalle factDte = new FacturasXPagarDetalle()
            {
                DetallesFactura = await _unitOfWork.ApoteosysRepository.EstadoCuentasXPorPagarDetalleApoteosys_Reporte(parameters),
                Factura = await _unitOfWork.SispoRepository.EstadoCuentasXPorPagarApoteosysDte(parameters)
            };
            return factDte;
        }
        #endregion

        #region Facturas Pagadas
        public async Task<List<EstadoCuentasPagadas>> EstadoCuentasPagadasApoteosys(QuerySearchEstadoCuentasPagadas parameters)
        {
            List<Empresa> empresas = await _unitOfWork.ParametrosInicialesRepository.GetEmpresasByUser(int.Parse(parameters.CodUser), parameters.TipoUsuario);
            return await _unitOfWork.ApoteosysRepository.EstadoCuentasPagadasApoteosys(parameters, empresas);
        }

        public async Task<List<EstadoCuentasPagadasDetalle>> EstadoCuentasPagadasDetalleApoteosys(QuerySearchEstadoCuentasPagadasDetalle parameters)
        {
            return await _unitOfWork.ApoteosysRepository.EstadoCuentasPagadasDetalleApoteosys(parameters);
        }

        public async Task<List<EstadoCuentasPagadasDetalle_Reporte>> EstadoCuentasPagadasDetalleApoteosys_Reporte(QuerySearchEstadoCuentasPagadasDetalle parameters)
        {
            return await _unitOfWork.ApoteosysRepository.EstadoCuentasPagadasDetalleApoteosys_Reporte(parameters);
        }

        #region Descuentos Aplicados en Facturas Pagadas
        public async Task<Detalle_FactPagas> EstadoCuentasXPorPagarDetalleApoteosysFactPagas(QuerySearchEstadoCuentasXPagar_FactPagas parameters)
        {
            List<EstadoCuentasXPagar_FactPagas_Maestro> entity = await _unitOfWork.ApoteosysRepository.EstadoCuentasXPorPagar_FactPagas_Maestro(parameters);
            Detalle_FactPagas resp = null;

            if (entity.Count > 0)
            {
                parameters.Tipo_Documento_Causacion = entity[0].Tipo_Documento_Causacion;
                parameters.Numero_Documento_Causacion = entity[0].Numero_Documento_Causacion;
                resp = new Detalle_FactPagas
                {
                    DetallesFactura = await _unitOfWork.ApoteosysRepository.EstadoCuentasXPorPagar_FactPagas_Detalle(parameters)
                };
            }
            
            return resp;
        }
        #endregion

        #region Consultas desde SP SQL Server
        public async Task<List<FacturasPagas_SQL>> GetFacturasPagadas_SpSQL(QuerySearchFactPagas_SQL parameters)
        {
            return await _unitOfWork.ApoteosysRepository.GetFacturasPagadas_SpSQL(parameters);
        }

        public async Task<List<FacturasPagas_SQL>> GetRetencionesFacturasPagadas_SpSQL(QuerySearchFactPagas_SQL parameters)
        {
            return await _unitOfWork.ApoteosysRepository.GetRetencionesFacturasPagadas_SpSQL(parameters);
        }
        #endregion
        #endregion
        #endregion

        #region Certificado
        public async Task<List<CertificadoRetencionMaestro>> CertificadosRetencionApoteosys(QuerySearchCertificados parameters)
        {
            return await _unitOfWork.ApoteosysRepository.CertificadosRetencionApoteosys(parameters);
        }

        public async Task<List<CertificadoRetencionFuenteDte>> CertificadosRetencionFuenteApoteosys(QuerySearchCertificados parameters)
        {
            return await _unitOfWork.ApoteosysRepository.CertificadosRetencionFuenteApoteosys(parameters);
        }

        public async Task<List<CertificadoRetencionIvaDte>> CertificadosRetencionIvaApoteosys(QuerySearchCertificados parameters)
        {
            return await _unitOfWork.ApoteosysRepository.CertificadosRetencionIvaApoteosys(parameters);
        }

        public async Task<List<CertificadoRetencionIcaDte>> CertificadosRetencionIcaApoteosys(QuerySearchCertificadosICA parameters)
        {
            return await _unitOfWork.ApoteosysRepository.CertificadosRetencionIcaApoteosys(parameters);
        }

        public async Task<List<CertificadoRetencionEstampillaBoyacaDte>> CertificadosRetencionEstampillaBoyacaApoteosys(QuerySearchCertificados parameters)
        {
            return await _unitOfWork.ApoteosysRepository.CertificadosRetencionEstampillaBoyacaApoteosys(parameters);
        }
        #endregion

        #region Requerimientos
        public async Task<List<SolicitudesApoteosys>> GetSolicitudes(QuerySearchSolicitudesApoteosys parameters)
        {
            return await _unitOfWork.ApoteosysRepository.GetSolicitudes(parameters);
        }        
        #endregion
    }
}
