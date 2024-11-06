using Core.Entities;
using Core.Interfaces;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public class SispoService : ISispoSevice
    {
        private readonly IUnitOfWork _unitOfWork;

        public SispoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Facturas Por Pagar
        public async Task<List<EstadoCuentasXPorPagar>> EstadoCuentasXPorPagarApoteosys(QuerySearchEstadoCuentasXPagar parameters)
        {
            List<Empresa> empresas = await _unitOfWork.ParametrosInicialesRepository.GetEmpresasByUser(int.Parse(parameters.CodUser), parameters.TipoUsuario);
            return await _unitOfWork.SispoRepository.EstadoCuentasXPorPagarApoteosys(parameters, empresas);
        }

        public async Task<List<EstadoCuentasXPorPagar>> EstadoCuentasXPorPagarApoteosysDte(QuerySearchEstadoCuentasXPagarDetalle parameters)
        {
            return await _unitOfWork.SispoRepository.EstadoCuentasXPorPagarApoteosysDte(parameters);
        }
        #endregion
    }
}
