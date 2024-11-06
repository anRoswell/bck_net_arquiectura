using Core.Entities;
using Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public class EmpresasSelectedCertEspService : IEmpresasSelectedCertEspService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmpresasSelectedCertEspService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<EmpresasSelectedCertEsp>> GetEmpresasSelectedCertificado(int id)
        {
            return await _unitOfWork.EmpresasSelectedCertEspRepository.GetEmpresasSelectedCertificado(id);
        }
    }
}
