using Core.Entities;
using Core.ModelResponse;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPerfilRepository : IRepository<Perfil>
    {
        Task<List<Perfil>> GetPerfiles();
        Task<List<Perfil>> GetPerfilesSSR(QuerySearchRequerimientosSSR data);
        Task<int> GetTotalPerfilesSSR();

        Task<List<Perfil>> Getperfil(int id);
        Task<List<ResponseAction>> PostCrear(Perfil perfil);
        Task<List<ResponseAction>> PutActualizar(Perfil perfil);
        Task<List<ResponseAction>> DeletePerfil(Perfil perfil);
    }
}
