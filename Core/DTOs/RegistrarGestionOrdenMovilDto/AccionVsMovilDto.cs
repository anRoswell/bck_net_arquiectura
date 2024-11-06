namespace Core.DTOs.RegistrarGestionOrdenMovilDto
{
    using System.Collections.Generic;
    using Core.DTOs.CuestionarioInstanciasMovilDto;

    public class AccionVsMovilDto
	{
        public int accion_vs { get; set; }
        public string observacion { get; set; }
        public IList<SoporteMovilDto> fotos { get; set; }
    }
}

