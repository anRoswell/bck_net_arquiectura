using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class PrvSocioService : IPrvSocioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PrvSocioService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<PrvSocio>> GetSocio(int id)
        {
            return await _unitOfWork.PrvSociosRepository.GetSocio(id);
        }
    }
}
