using Core.Entities;
using Core.ModelResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPermisosMenuXPerfilService
    {
        Task<List<PermisosMenuXperfil>> GetListado();
        Task<List<PermisosMenuXperfil>> GetPorId(int id);
        Task<List<ResponseAction>> PostCrear(PermisosMenuXperfil permisosMenuXperfil);
        Task<List<ResponseAction>> PutActualizar(PermisosMenuXperfil permisosMenuXperfil);
        Task<List<ResponseAction>> DeleteRegistro(PermisosMenuXperfil permisosMenuXperfil);
    }
}
