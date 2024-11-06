using Core.Entities;
using Core.ModelResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPermisosUsuarioxMenuRepository : IRepository<PermisosUsuarioxMenu>
    {
        Task<List<PermisosUsuarioxMenu>> GetListado();
        Task<List<PermisosXUsuario>> GetPermisosXUsuario(int codUser);
        Task<List<PermisosXUsuario>> GetPermisosXUsuarioController(int codUser, string controlador);
        Task<List<PermisosUsuarioxMenu>> GetPorId(int id);
        Task<List<ResponseAction>> PostCrear(PermisosUsuarioxMenu permisosUsuarioxMenu);
        Task<List<ResponseAction>> PutActualizar(PermisosUsuarioxMenu permisosUsuarioxMenu);
        Task<List<ResponseAction>> DeleteRegistro(PermisosUsuarioxMenu permisosUsuarioxMenu);
    }
}
