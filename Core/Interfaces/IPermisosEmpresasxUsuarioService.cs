using Core.Entities;
using Core.ModelResponse;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPermisosEmpresasxUsuarioService
    {
        Task<List<PermisosEmpresasxUsuario>> GetListado();
        Task<List<PermisosEmpresasxUsuario>> GetPorId(int id);
        Task<List<ResponseAction>> PostCrear(QueryCreatePermisoEmpresa permisosEmpresasxUsuario);
        Task<List<ResponseAction>> PutActualizar(QueryCreatePermisoEmpresa permisosEmpresasxUsuario);
        Task<List<ResponseAction>> DeleteRegistro(PermisosEmpresasxUsuario permisosEmpresasxUsuario);
    }
}
