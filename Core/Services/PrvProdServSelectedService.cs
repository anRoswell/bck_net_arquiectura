using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class PrvProdServSelectedService : IPrvProdServSelectedService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PrvProdServSelectedService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<PrvProdServSelected>> GetPrvProdServSelected(int id)
        {
            return await _unitOfWork.PrvProdServSelectedRepository.GetPrvProdServSelected(id);
        }
    }
}
