using Core.Entities;
using Core.ModelResponse;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ITokenService
    {
        Task<List<ResponseActionUrl>> GenerateToken();
        Task<List<ResponseAction>> UpdateToken(string html, int id);
        Task<List<HashCertifiedValidation>> ValidateTokenCertificado(QueryToken query);
    }
}
