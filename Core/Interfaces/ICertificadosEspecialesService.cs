using Core.CustomEntities;
using Core.DTOs;
using Core.Entities;
using Core.ModelResponse;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICertificadosEspecialesService
    {
        Task<List<CertificadosEspeciale>> GetCertificadosEspeciales();
        Task<List<CertificadosEspeciale>> GetCertificadosEspecialesProveedor(string idUser);
        Task<List<CertificadosEspeciale>> GetCertificadosEspecialesFiltro(QuerySearchFiltroCertificados query);
        Task<CertificadoEspecialDetalle> GetCertificadoDetallePorId(int id);
        Task<List<RespuestasCertEspeciale>> GetRespuestasCertificadoPorID(int id);
        Task<ParametrosCertificados> GetParametrosIniciales(int idUser, string tipoUsuario);
        Task<List<ResponseAction>> CrearCertificadoEspecial(CertificadosEspecialesDto certificados);
        Task<List<ResponseAction>> UpdateCertificadoEspecial(CertificadosEspecialesDto certificados);
        Task<List<ResponseAction>> UpdateEstadoCertificadoEspecial(QueryUpdateEstadoCertificado certificados);

        //Task<List<CertificadoRetencion>> CertificadosRetencionApoteosys(QuerySearchCertificados parameters);
    }
}
