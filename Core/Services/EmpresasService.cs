using Core.Entities;
using Core.Interfaces;
using Core.ModelResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public class EmpresasService : IEmpresasService
    {
        private readonly IUnitOfWork _unitOfWork;
        public List<Empresa> EmpresasActivas { get; set; }

        public EmpresasService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            EmpresasActivas = GetEmpresas().Result;
        }

        public async Task<List<Empresa>> GetEmpresas()
        {
            return await _unitOfWork.EmpresasRepository.GetEmpresas();
        }

        public async Task<List<Empresa>> GetListEmpresas()
        {
            return await _unitOfWork.EmpresasRepository.GetListEmpresas();
        }

        public async Task<List<ResponseAction>> PutEmpresa(Empresa empresa)
        { 
            return await _unitOfWork.EmpresasRepository.PutEmpresa(empresa);
        }

        public async Task<List<ResponseAction>> DeleteEmpresa(Empresa empresa)
        {
            return await _unitOfWork.EmpresasRepository.DeleteEmpresa(empresa);
        }

        public async Task<List<ResponseAction>> PostCrear(Empresa empresa)
        {
            return await _unitOfWork.EmpresasRepository.PostCrear(empresa);
        }
    }
}
