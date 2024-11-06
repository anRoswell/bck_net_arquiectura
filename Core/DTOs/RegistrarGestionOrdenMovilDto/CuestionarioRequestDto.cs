namespace Core.DTOs.RegistrarGestionOrdenMovilDto
{
    using System.Collections.Generic;
    using Core.DTOs.CuestionarioInstanciasMovilDto;

    public class CuestionarioRequestDto
	{
        public int id_cuestionario { get; set; }
        public int id_orden { get; set; }
        public int id_contratista_persona { get; set; }
        public IList<RespuestaMovilDto> respuestas { get; set; }
        public IList<SoporteRequestMovilDto> soportes { get; set; }
        public string observaciones { get; set; }
    }
}

