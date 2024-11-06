using Core.CustomEntities;
using Core.ModelResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAdobeSignRepository
    {
        Task<List<ResponseAction>> GuardarTrazaAdobeSign_Prv(int idProveedor, int codAdobeSignEstado, string returnedAdobeSignId, string returnedAdobeSignJson);
        Task<List<ResponseAction>> GuardarTrazaAdobeSign_Contrato(int idContrato, int codAdobeSignEstado, string returnedAdobeSignId, string returnedAdobeSignJson);
        Task<List<ResponseAction>> GuardarLogErrorAdobeSign(adobeErrorResponse adobeError, string proceso, int idLlave = 0);
    }
}
