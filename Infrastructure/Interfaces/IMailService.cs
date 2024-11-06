using Core.ModelResponse;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IMailService
    {
        Task<List<ResponseAction>> SendMailMasive(QuerySendMailMasive parameters);
        Task<List<ResponseAction>> SendMail_ContratosPendientes(QuerySendMailMasive parameters);
        Task<List<ResponseAction>> SendMail_ContratosFirmasPendientes(QuerySendMailMasive parameters);
    }
}
