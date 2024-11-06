using Core.Entities;
using Core.ModelResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPerfilesXusuarioService
    {
        Task<List<PerfilesXusuarioView>> GetListado();
        Task<List<PerfilesXusuario>> GetPorId(int id);
        Task<List<PerfilesXusuario>> GetPorIdUsuario(int idUsuario);
        Task<List<ResponseAction>> PostCrear(PerfilesXusuario perfilesXusuario);
        Task<List<ResponseAction>> PutActualizar(PerfilesXusuario perfilesXusuario);
        Task<List<ResponseAction>> DeleteRegistro(PerfilesXusuario perfilesXusuario);
    }
}
