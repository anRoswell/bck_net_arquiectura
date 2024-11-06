using Core.Entities;
using Core.ModelResponse;
using Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public class PerfilesXusuarioService : IPerfilesXusuarioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PerfilesXusuarioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PerfilesXusuarioView>> GetListado()
        {
            return await _unitOfWork.PerfilesXusuarioRepository.GetListado();
        }

        public async Task<List<PerfilesXusuario>> GetPorId(int id)
        {
            return await _unitOfWork.PerfilesXusuarioRepository.GetPorId(id);
        }

        public async Task<List<PerfilesXusuario>> GetPorIdUsuario(int idUsuario)
        {
            return await _unitOfWork.PerfilesXusuarioRepository.GetPorIdUsuario(idUsuario);
        }

        public async Task<List<ResponseAction>> PostCrear(PerfilesXusuario perfilesXusuario)
        {
            return await _unitOfWork.PerfilesXusuarioRepository.PostCrear(perfilesXusuario);
        }

        public async Task<List<ResponseAction>> PutActualizar(PerfilesXusuario perfilesXusuario)
        {
            return await _unitOfWork.PerfilesXusuarioRepository.PutActualizar(perfilesXusuario);
        }

        public async Task<List<ResponseAction>> DeleteRegistro(PerfilesXusuario perfilesXusuario)
        {
            return await _unitOfWork.PerfilesXusuarioRepository.DeleteRegistro(perfilesXusuario);
        }
    }
}
