using Core.Entities;
using Core.Interfaces;
using Core.ModelResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public class PermisosUsuarioxMenuService : IPermisosUsuarioxMenuService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PermisosUsuarioxMenuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<PermisosUsuarioxMenu>> GetListado()
        {
            return _unitOfWork.PermisosUsuarioxMenuRepository.GetListado();
        }

        public Task<List<PermisosUsuarioxMenu>> GetPorId(int id)
        {
            return _unitOfWork.PermisosUsuarioxMenuRepository.GetPorId(id);
        }

        public Task<List<ResponseAction>> PostCrear(PermisosUsuarioxMenu permisosUsuarioxMenu)
        {
            return _unitOfWork.PermisosUsuarioxMenuRepository.PostCrear(permisosUsuarioxMenu);
        }

        public Task<List<ResponseAction>> PutActualizar(PermisosUsuarioxMenu permisosUsuarioxMenu)
        {
            return _unitOfWork.PermisosUsuarioxMenuRepository.PutActualizar(permisosUsuarioxMenu);
        }

        public Task<List<ResponseAction>> DeleteRegistro(PermisosUsuarioxMenu permisosUsuarioxMenu)
        {
            return _unitOfWork.PermisosUsuarioxMenuRepository.DeleteRegistro(permisosUsuarioxMenu);
        }

        public Task<List<PermisosXUsuario>> GetPermisosXUsuario(int codUser)
        {
            return _unitOfWork.PermisosUsuarioxMenuRepository.GetPermisosXUsuario(codUser);
        }

        public async Task<List<PermisosXUsuario>> GetPermisosXUsuarioController(int codUser, string controlador)
        {
            return await _unitOfWork.PermisosUsuarioxMenuRepository.GetPermisosXUsuarioController(codUser, controlador);
        }
    }
}
