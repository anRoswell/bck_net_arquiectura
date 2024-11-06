namespace Core.DTOs.RegistrarGestionOrdenMovilDto
{
    using Core.DTOs.CuestionarioInstanciasMovilDto;
    using System.Collections.Generic;

    public class AccionVsMovilRequestDto
	{
        public int? accion_vs { get; set; }
        public string observacion { get; set; }
        public IList<SoporteRequestMovilDto> fotos { get; set; }
    }
}

