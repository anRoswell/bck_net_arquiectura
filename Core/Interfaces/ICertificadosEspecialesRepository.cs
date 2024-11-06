using Core.DTOs;
using Core.Entities;
using Core.ModelResponse;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICertificadosEspecialesRepository
    {
        Task<List<CertificadosEspeciale>> GetCertificadosEspeciales();
        Task<List<CertificadosEspeciale>> GetCertificadosEspecialesProveedor(string idUser);
        Task<List<CertificadosEspeciale>> GetCertificadosEspecialesFiltro(QuerySearchFiltroCertificados query);
        Task<List<CertificadosEspeciale>> GetCertificadoPorID(int id);
        Task<List<RespuestasCertEspeciale>> GetRespuestasCertificadoPorID(int id);
        Task<List<ResponseAction>> CrearCertificadoEspecial(CertificadosEspecialesDto certificados);
        Task<List<ResponseAction>> UpdateCertificadoEspecial(CertificadosEspecialesDto certificados);
        Task<List<ResponseAction>> UpdateEstadoCertificadoEspecial(QueryUpdateEstadoCertificado certificados);
    }
}
