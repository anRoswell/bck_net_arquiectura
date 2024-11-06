using Core.Entities;
using Core.ModelResponse;
using Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public class PermisosMenuXPerfilService : IPermisosMenuXPerfilService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PermisosMenuXPerfilService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<PermisosMenuXperfil>> GetListado()
        {
            return _unitOfWork.PermisosMenuXPerfilRepository.GetListado();
        }

        public Task<List<PermisosMenuXperfil>> GetPorId(int id)
        {
            return _unitOfWork.PermisosMenuXPerfilRepository.GetPorId(id);
        }

        public Task<List<ResponseAction>> PostCrear(PermisosMenuXperfil permisosMenuXperfil)
        {
            return _unitOfWork.PermisosMenuXPerfilRepository.PostCrear(permisosMenuXperfil);
        }

        public Task<List<ResponseAction>> PutActualizar(PermisosMenuXperfil permisosMenuXperfil)
        {
            return _unitOfWork.PermisosMenuXPerfilRepository.PutActualizar(permisosMenuXperfil);
        }

        public Task<List<ResponseAction>> DeleteRegistro(PermisosMenuXperfil permisosMenuXperfil)
        {
            return _unitOfWork.PermisosMenuXPerfilRepository.DeleteRegistro(permisosMenuXperfil);
        }
    }
}
