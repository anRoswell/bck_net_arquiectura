using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class PrvEmpresasSelectedService: IPrvEmpresasSelectedService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PrvEmpresasSelectedService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PrvEmpresasSelected>> GetEmpresasSelected(int id)
        {
            return await _unitOfWork.PrvEmpresasSelectedRepository.GetEmpresasSelected(id);
        }
    }
}
