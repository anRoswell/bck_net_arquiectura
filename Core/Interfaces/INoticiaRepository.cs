
namespace Core.Interfaces
{
    using Core.Entities;
    using Core.ModelResponse;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INoticiaRepository
    {
        Task<List<Noticia>> GetNoticias(string Operacion, string CodUser);
        Task<List<Empresa>> GetEmpresas();
        Task<List<TiposPlantilla>> GetTiposPlantillas();
        Task<List<TipoNoticia>> GetTipoNoticia();
        Task<List<Alcance>> GetAlcance();
        Task<List<ResponseAction>> PostNoticia(Noticia noticia, List<NoticiasDoc> listDocuments);
        Task<List<ResponseAction>> PutNoticia(Noticia noticia, List<NoticiasDoc> listDocuments);
        Task<List<ResponseAction>> DeleteNoticia(Noticia noticia);
        Task<List<Noticia>> GetNoticiaPorID(int id);
    }
}
