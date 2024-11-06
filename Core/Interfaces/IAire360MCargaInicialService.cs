namespace Core.Interfaces
{
    using Core.DTOs;
    using System.Threading.Tasks;
    using Core.DTOs.CargaInicialMovilDto;
    using Core.DTOs.EncuestaInicialMovilDto;
    using Core.DTOs.OrdenesAsignadasTecnicoMovilDto;
    using System.Collections.Generic;
    using Core.DTOs.CuestionarioInstanciasMovilDto;
    using Core.DTOs.RegistrarGestionOrdenMovilDto;
    using Core.Dtos.RechazarOrdenDto;
    using Core.DTOs.ComprometerOrdenDto;

    public interface IAire360MCargaInicialService
    {
        /// <summary>
        /// Obtiene los registros de carga inicial.
        /// </summary>
        /// <returns>Json de respuesta con los registros.</returns>
        Task<ResponseDto<DataCargaInicialMovilDto>> GetCargaInicialAsync();

        /// <summary>
        /// Obtiene la encuesta inicial.
        /// </summary>
        /// <param name="encuestaInicialRequestMovilDto">Parametro de entrada.</param>
        /// <returns>Json de respuesta con los registros.</returns>
        Task<ResponseDto<EncuestaInicialMovilDto>> GetEncuestaInicialAsync(EncuestaInicialRequestMovilDto encuestaInicialRequestMovilDto);

        /// <summary>
        /// Obtiene las ordenes asignadas por tecnico.
        /// </summary>
        /// <param name="ordenesAsignadasTecnicoMovilRequestDto">Parametro de entrada.</param>
        /// <returns>Json de respuesta con los registros.</returns>
        Task<ResponseDto<IList<OrdenesAsignadasTecnicoMovilDto>>> GetOrdenesAsignadasTecnicoAsync(OrdenesAsignadasTecnicoMovilRequestDto ordenesAsignadasTecnicoMovilRequestDto);

        /// <summary>
        /// Crea el cuestionario.
        /// </summary>
        /// <param name="cuestionarioInstanciasRequestMovilDto">Parametro de entrada.</param>
        /// <returns>Id del cuestionario creado.</returns>
        Task<ResponseDto<CuestionarioInstanciasResponseMovilDto>> CreateCuestionarioInstanciaAsync(CuestionarioInstanciasRequestMovilDto cuestionarioInstanciasRequestMovilDto);

        /// <summary>
        /// Registrar gestion de orden.
        /// </summary>
        /// <param name="registrarGestionOrdenMovilRequestDto">Parametro de entrada.</param>
        /// <returns>Id de la gestion de orden creada.</returns>
        Task<ResponseDto> RegisterGestionOrdenAsync(RegistrarGestionOrdenMovilRequestDto registrarGestionOrdenMovilRequestDto);

        /// <summary>
        /// Rechazar la orden.
        /// </summary>
        /// <param name="rechazarOrdenRequestDto">Parametro de entrada.</param>
        /// <returns>Response.</returns>
        Task<ResponseDto> RechazarOrdenAsync(RechazarOrdenRequestDto rechazarOrdenRequestDto);

        /// <summary>
        /// Compromete la orden
        /// </summary>
        /// <param name="comprometerOrdenRequestDto">Parametros de entrada.</param>
        /// <returns>Reponse.</returns>
        Task<ResponseDto> ComprometerOrdenAsync(ComprometerOrdenRequestDto comprometerOrdenRequestDto);
    }
}

