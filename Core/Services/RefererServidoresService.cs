using Core.Entities;
using Core.Interfaces;
using System.Threading.Tasks;

namespace Core.Services
{
    /// <summary>
    /// Clase para ejecutar la unidad de trabajo, encargada de ejecutar el repositorio.
    /// </summary>
    public class RefererServidoresService : IRefererServidoresService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RefererServidoresService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> GetPermisoAccesoPorRefererServidores(string Referer, string Grupo)
        {
            return await _unitOfWork.RefererServidoresRepository.GetPermisoAccesoPorRefererServidores(Referer, Grupo);
        }

        public async Task<bool> GetPermisoAccesoPorRefererServidoresOracle(string Referer, string Grupo)
        {
            return await _unitOfWork.RefererServidoresRepositoryOracle.GetPermisoAccesoPorRefererServidoresCore(Referer, Grupo);
        }
    }
}
