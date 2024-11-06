using Core.Entities;
using Core.ModelResponse;
using Core.QueryFilters;
using Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPerfilService
    {
        Task<List<Perfil>> GetPerfiles();
        Task<PerfilPaginador> GetPerfilesSSR(QuerySearchRequerimientosSSR data);
        Task<List<Perfil>> Getperfil(int id);
        Task<List<ResponseAction>> PostCrear(Perfil perfil);
        Task<List<ResponseAction>> PutActualizar(Perfil perfil);
        Task<List<ResponseAction>> DeletePerfil(Perfil perfil);
    }
}
