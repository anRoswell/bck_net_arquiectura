using Core.CustomEntities;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.ModelResponse;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public class CertificadosEspecialesService : ICertificadosEspecialesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CertificadosEspecialesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<CertificadosEspeciale>> GetCertificadosEspeciales()
        {
            return await _unitOfWork.CertificadosEspecialesRepository.GetCertificadosEspeciales();
        }

        public async Task<List<CertificadosEspeciale>> GetCertificadosEspecialesProveedor(string idUser)
        {
            return await _unitOfWork.CertificadosEspecialesRepository.GetCertificadosEspecialesProveedor(idUser);
        }

        public async Task<List<CertificadosEspeciale>> GetCertificadosEspecialesFiltro(QuerySearchFiltroCertificados query)
        {
            return await _unitOfWork.CertificadosEspecialesRepository.GetCertificadosEspecialesFiltro(query);
        }

        public async Task<CertificadoEspecialDetalle> GetCertificadoDetallePorId(int id)
        {
            CertificadoEspecialDetalle certificadoEspecial = new CertificadoEspecialDetalle()
            {
                Certificado = await _unitOfWork.CertificadosEspecialesRepository.GetCertificadoPorID(id),
                ListadoEmpresasSelected = await _unitOfWork.EmpresasSelectedCertEspRepository.GetEmpresasSelectedCertificado(id),
                ListadoRespuestas = await _unitOfWork.CertificadosEspecialesRepository.GetRespuestasCertificadoPorID(id)
            };

            return certificadoEspecial;
        }

        public async Task<List<RespuestasCertEspeciale>> GetRespuestasCertificadoPorID(int id)
        {
            return await _unitOfWork.CertificadosEspecialesRepository.GetRespuestasCertificadoPorID(id);
        }

        public async Task<ParametrosCertificados> GetParametrosIniciales(int idUser, string tipoUsuario)
        {
            ParametrosCertificados parametros = new ParametrosCertificados()
            {
                TiposCertificadoEspeciales = await _unitOfWork.ParametrosInicialesRepository.GetTiposCertificadosEspeciales(),
                TiposCertificados = await _unitOfWork.ParametrosInicialesRepository.GetTiposCertificados(),
                Empresas = await _unitOfWork.ParametrosInicialesRepository.GetEmpresasByUser(idUser, tipoUsuario),
                EmpresasApoteosys = tipoUsuario == "Proveedor" ? await _unitOfWork.ParametrosInicialesRepository.GetEmpresasByUserApoteosys(idUser) : await _unitOfWork.ParametrosInicialesRepository.GetEmpresasByUserDashboardApoteosys(idUser)
            };

            return parametros;
        }

        public async Task<List<ResponseAction>> CrearCertificadoEspecial(CertificadosEspecialesDto certificados)
        {
            return await _unitOfWork.CertificadosEspecialesRepository.CrearCertificadoEspecial(certificados);
        }

        public async Task<List<ResponseAction>> UpdateCertificadoEspecial(CertificadosEspecialesDto certificados)
        {
            return await _unitOfWork.CertificadosEspecialesRepository.UpdateCertificadoEspecial(certificados);
        }

        public async Task<List<ResponseAction>> UpdateEstadoCertificadoEspecial(QueryUpdateEstadoCertificado certificados)
        {
            return await _unitOfWork.CertificadosEspecialesRepository.UpdateEstadoCertificadoEspecial(certificados);
        }
    }
}
