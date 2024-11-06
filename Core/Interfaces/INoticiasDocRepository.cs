using Core.Entities;
using Core.ModelResponse;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface INoticiasDocRepository
    {
        Task<List<NoticiasDoc>> GetDocumentosNoticia(int idNoticia);
        Task<List<ResponseActionUrl>> AgregarImagenNoticia(NoticiasDoc entity);
        Task<List<ResponseAction>> DeleteImagenNoticia(QueryDeleteImagenNoticia query);
    }
}
