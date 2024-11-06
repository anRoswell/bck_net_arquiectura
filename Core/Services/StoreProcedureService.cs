namespace Core.Services
{
    using System.Threading.Tasks;
    using Core.DTOs;
    using Core.Entities;
    using Core.Interfaces;
    using Core.QueryFilters;
    using Newtonsoft.Json;

    public class StoreProcedureService : IStoreProcedureService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StoreProcedureService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<ResponseDto<DatosDto>> Get()
        {
            var prueba = new RequestDto() { pagina = 1 };
            var param = JsonConvert.SerializeObject(prueba);

            return _unitOfWork.StoreProcedure<ResponseDto<DatosDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_generales.prc_test", param);
        }

        public Task<ResponseDto<Aire_Scr_Ord_OrdenDto>> UpdateOrden(Aire_Scr_Ord_OrdenDto aire_ord_ordenDto)
        {
            var param = JsonConvert.SerializeObject(aire_ord_ordenDto);
            return _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_Ord_OrdenDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_actualizar_orden", param);
        }

        public Task<ResponseDto<Datos_Dummi>> Obtener_Datos_Dummi(string StoreProcedure, QueryOp360Dummi parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            return _unitOfWork.StoreProcedure<ResponseDto<Datos_Dummi>>().ExecuteStoreProcedureAsync(StoreProcedure, param);
        }        
    }
}
