namespace Core.Services
{
    using Core.DTOs.Logdto;
    using Core.Entities;
    using Core.Interfaces;
    using Core.ModelResponse;
    using System.Threading.Tasks;

    public class LogErroresService : ILogErroresService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LogErroresService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseAction> SaveError(LogErrores error)
        {
            LogRequestDto logRequestDto = new()
            {
                e_nombre_up = $"{error.Controlador} - {error.Funcion}",
                e_mensaje = error.Descripcion
            };
            var result = await _unitOfWork.StoreProcedure<LogResponseDto>().ExecuteFunctionNonQueryAsync("SELECT aire.PKG_P_GENERALES.fnc_registrar_log(:e_nombre_up, :e_tipo_mensaje, :e_mensaje) FROM DUAL", logRequestDto);
            var response = new ResponseAction()
            {
                estado = false,
                mensaje = logRequestDto.e_nombre_up,
                error = logRequestDto.e_mensaje,
                codigo_error = (decimal)result
            };

            return response;
        }
    }
}
