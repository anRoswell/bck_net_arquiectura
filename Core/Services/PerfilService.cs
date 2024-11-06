using Core.Entities;
using Core.Interfaces;
using Core.ModelResponse;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public class PerfilService : IPerfilService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PerfilService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<List<Perfil>> GetPerfiles()
        {
            return _unitOfWork.PerfilesRepository.GetPerfiles();
        }
        
        public async Task<PerfilPaginador> GetPerfilesSSR(QuerySearchRequerimientosSSR data)
        {
            PerfilPaginador perfiles = new PerfilPaginador()
            {
                perfiles = await _unitOfWork.PerfilesRepository.GetPerfilesSSR(data),
                TotatlRecords = await _unitOfWork.PerfilesRepository.GetTotalPerfilesSSR()
            };
            return perfiles;
        }

        public Task<List<Perfil>> Getperfil(int id)
        {
            return _unitOfWork.PerfilesRepository.Getperfil(id);
        }

        public Task<List<ResponseAction>> PostCrear(Perfil perfil)
        {
            return _unitOfWork.PerfilesRepository.PostCrear(perfil);
        }

        public Task<List<ResponseAction>> PutActualizar(Perfil perfil)
        {
            return _unitOfWork.PerfilesRepository.PutActualizar(perfil);
        }

        public Task<List<ResponseAction>> DeletePerfil(Perfil perfil)
        {
            return _unitOfWork.PerfilesRepository.DeletePerfil(perfil);
        }
    }
}
