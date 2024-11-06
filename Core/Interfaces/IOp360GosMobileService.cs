namespace Core.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.DTOs;
    using Core.DTOs.Gos.Mobile.ComprometerGestion;
    using Core.DTOs.Gos.Mobile.CrearGestion;
    using Core.DTOs.Gos.Mobile.ObtenerGestionesByGestorDto;
    using Core.DTOs.ObtenerGestionesByGestorDto;
    using Core.DTOs.ProcesarGestionDto;

    public interface IOp360GosMobileService
    {
        /// <summary>
        /// Obtiene las gestiones por gestor asignado.
        /// </summary>
        /// <param name="obtenerGestionesByGestorRequestDto">Parametro de entrada.</param>
        /// <returns>Lista de gestiones</returns>
        public Task<ResponseDto<IList<GestionesByGestorDto>>> ObtenerGestionesByGestorAsync(ObtenerGestionesByGestorRequestDto obtenerGestionesByGestorRequestDto);

        /// <summary>
        /// Procesa la gestion
        /// </summary>
        /// <param name="procesarGestionRequestDto">Parametros de entrada.</param>
        /// <returns>Respuesta base</returns>
        public Task<ResponseDto> ProcesarGestionAsync(ProcesarGestionRequestDto procesarGestionRequestDto);

        /// <summary>
        /// Crea la gestion
        /// </summary>
        /// <param name="crearGestionRequestDto">Parametros de entrada.</param>
        /// <returns>Respuesta base</returns>
        public Task<ResponseDto<CrearGestionResponseDto>> CrearGestionAsync(CrearGestionRequestDto crearGestionRequestDto);

        /// <summary>
        /// Compromete la gestion
        /// </summary>
        /// <param name="comprometerGestionRequestDto">Parametros de entrada.</param>
        /// <returns>Respuesta base</returns>
        public Task<ResponseDto> ComprometerGestionAsync(ComprometerGestionRequestDto comprometerGestionRequestDto);
    }
}