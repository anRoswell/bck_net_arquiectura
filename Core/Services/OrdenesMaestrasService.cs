using Core.Entities;
using Core.Interfaces;
using Core.ModelResponse;
using Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class OrdenesMaestrasService : IOrdenesMaestrasService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdenesMaestrasService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    
        public async Task<List<OrdenesMaestras>> GetListado()
        {
            return await _unitOfWork.OrdenesMaestrasRepository.GetListado();
        }

        public async Task<List<ResponseAction>> PostOrdenReq(OrdenReq ordenReq)
        {
            return await _unitOfWork.OrdenesMaestrasRepository.PostOrdenReq(ordenReq);
        }

        public async Task<List<OrdenesMaestras>> SearchByProveedor(QuerySearchOrdenes parameters)
        {
            return await _unitOfWork.OrdenesMaestrasRepository.SearchByProveedor(parameters);
        }
    }
}
