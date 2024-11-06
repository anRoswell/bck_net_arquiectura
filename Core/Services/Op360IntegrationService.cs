namespace Core.Services
{
    using Core.DTOs;
    using Core.DTOs.Integration;
    using Core.DTOs.Integration.Authentication;
    using Core.Interfaces;
    using Newtonsoft.Json;
    using System.Threading.Tasks;

    public class Op360IntegrationService : IOp360IntegrationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public Op360IntegrationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IntegrationResponseDto> GuardarOrdenIntegrationLog(dynamic _integration)
        {
            var param = JsonConvert.SerializeObject(_integration);
            return await _unitOfWork.StoreProcedure<IntegrationResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_daniel_gonzalez_test.registrar_log", param);
        }

        public async Task<IntegrationResponseDto> GuardarOrdenIntegrationBodyPrueba(Body _integration)
        {
            //var bodyPart = _integration;

            var param = JsonConvert.SerializeObject(_integration);
            return await _unitOfWork.StoreProcedure<IntegrationResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_scr.prc_resgistrar_ordenes_ws_osf", param);
        }

        public async Task<ResponseDto<AuthenticationResponseDto>> AuthenticationAsync(AuthenticationRequestDto authenticationRequestDto)
        { 
            var param = JsonConvert.SerializeObject(authenticationRequestDto);
            return await _unitOfWork.StoreProcedure<ResponseDto<AuthenticationResponseDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_auth_tokens.prc_generar_auth_token", param);
        }
    }
}
