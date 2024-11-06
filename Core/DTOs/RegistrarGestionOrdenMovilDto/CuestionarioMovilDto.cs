namespace Core.DTOs.RegistrarGestionOrdenMovilDto
{
    using System.Collections.Generic;
    using Core.DTOs.CuestionarioInstanciasMovilDto;

    public class CuestionarioMovilDto : CuestionarioInstanciasMovilDto
    {
        public string observaciones { get; set; }

        /// <summary>
        /// Mapeo del dto.
        /// </summary>
        /// <param name="idCuestionario">Identificador del cuestionario.</param>
        /// <param name="idOrden">Identificador de orden</param>
        /// <param name="idContratistaPersona">Identificador de contratista.</param>
        /// <param name="idUsuario">Identificador de usuario.</param>
        /// <param name="respuestas">Respuestas.</param>
        /// <param name="soportes">Soportes.</param>
        /// <param name="observaciones">Observaciones.</param>
        public CuestionarioMovilDto(int idCuestionario, int idOrden, int idContratistaPersona, int idUsuario, IList<RespuestaMovilDto> respuestas, List<SoporteMovilDto> soportes, string observaciones)
            : base(idCuestionario, idOrden, idContratistaPersona, idUsuario, respuestas, soportes)
        {
            this.observaciones = observaciones;
        }
    }
}

