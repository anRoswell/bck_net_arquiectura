namespace Core.Interfaces
{
    using Core.DTOs;
    using Core.DTOs.Integration;
    using Core.DTOs.Integration.Authentication;
    using System.Threading.Tasks;

    public interface IOp360IntegrationService
    {
        Task<IntegrationResponseDto> GuardarOrdenIntegrationLog(dynamic _integration);
        Task<IntegrationResponseDto> GuardarOrdenIntegrationBodyPrueba(Body _integration);

        /// <summary>
        /// Genera un token de 20 caracteres.
        /// </summary>
        /// <param name="authenticationRequestDto">Dto con usuario y contraseña.</param>
        /// <returns>Token.</returns>
        public Task<ResponseDto<AuthenticationResponseDto>> AuthenticationAsync(AuthenticationRequestDto authenticationRequestDto);
    }
}
