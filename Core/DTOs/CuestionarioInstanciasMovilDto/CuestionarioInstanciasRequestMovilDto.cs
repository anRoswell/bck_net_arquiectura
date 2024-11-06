namespace Core.DTOs.CuestionarioInstanciasMovilDto
{
    using System.Collections.Generic;

    public class CuestionarioInstanciasRequestMovilDto
	{
        public int id_cuestionario { get; set; }
        public int id_orden { get; set; }
        public int id_contratista_persona { get; set; }
        public int id_usuario { get; set; }
        public IList<RespuestaMovilDto> respuestas { get; set; }
        public IList<SoporteRequestMovilDto> soportes { get; set; }
    }
}

