using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Tools;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public class PlantillaService : IPlantillaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlantillaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<RequerimientoComparativoDto>> ConsultarJson(int idReq)
        {
            return await _unitOfWork.PlantillaRepository.ConsultarJson(idReq);
        }

        public async Task<PruebaDataTable> ConsultarMultiplesTablas()
        {
            var tablas = await _unitOfWork.PlantillaRepository.ConsultarMultiplesTablas();

            if (tablas.Count == 0)
                return new PruebaDataTable();

            PruebaDataTable pruebaDataTable = new PruebaDataTable()
            {
                FormasPagos = BaseMultiplesTables.ConvertToList<FormasPago>(tablas[0]),
                TipoUsuarios = BaseMultiplesTables.ConvertToList<TipoUsuario>(tablas[1])
            };

            return pruebaDataTable;
        }
    }
}
