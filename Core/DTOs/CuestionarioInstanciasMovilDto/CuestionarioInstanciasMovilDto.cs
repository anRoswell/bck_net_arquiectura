namespace Core.DTOs.CuestionarioInstanciasMovilDto
{
    using System.Collections.Generic;

    public class CuestionarioInstanciasMovilDto
	{
        public int id_cuestionario { get; set; }
        public int id_orden { get; set; }
        public int id_contratista_persona { get; set; }
        public int id_usuario { get; set; }
        public IList<RespuestaMovilDto> respuestas { get; set; }
        public IList<SoporteMovilDto> soportes { get; set; }

        /// <summary>
        /// Mapeo del dto.
        /// </summary>
        /// <param name="idCuestionario">Identificador del cuestionario.</param>
        /// <param name="idOrden">Identificador de orden</param>
        /// <param name="idContratistaPersona">Identificador de contratista.</param>
        /// <param name="idUsuario">Identificador de usuario.</param>
        /// <param name="respuestas">Respuestas.</param>
        /// <param name="soportes">Soportes.</param>
        public CuestionarioInstanciasMovilDto(int idCuestionario, int idOrden, int idContratistaPersona, int idUsuario, IList<RespuestaMovilDto> respuestas, List<SoporteMovilDto> soportes)
        {
            id_contratista_persona = idContratistaPersona;
            id_cuestionario = idCuestionario;
            id_orden = idOrden;
            id_usuario = idUsuario;
            this.respuestas = respuestas;
            this.soportes = soportes;
        }
    }
}
