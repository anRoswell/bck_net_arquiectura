using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    class PrvReferenciaService : IPrvReferenciaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PrvReferenciaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PrvReferencia>> GetReferencia(int id)
        {
            return await _unitOfWork.PrvReferenciaRepository.GetReferencia(id);
        }
    }
}
