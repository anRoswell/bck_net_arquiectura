
namespace Core.Interfaces
{
    using Core.CustomEntities;
    using Core.Entities;
    using Core.ModelResponse;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INoticiaService
    {
        Task<ParametrosNoticias> ParametrosNoticias(string Operacion, string CodUser);
        Task<List<Noticia>> GetNoticias(string Operacion, string CodUser);
        Task<List<TiposPlantilla>> GetTiposPlantillas();
        Task<List<Alcance>> GetAlcance();
        Task<ParametrosNoticias> GetParametros(int idProveedor, string CodUser, string Operacion);
        Task<NoticiasDetalle> GetNoticiaPorID(int id);
        Task<List<ResponseAction>> PostNoticia(Noticia noticia, List<NoticiasDoc> listDocuments);
        Task<List<ResponseAction>> PutNoticia(Noticia noticia, List<NoticiasDoc> listDocuments);
        Task<List<ResponseAction>> DeleteNoticia(Noticia noticia);
    }
}
