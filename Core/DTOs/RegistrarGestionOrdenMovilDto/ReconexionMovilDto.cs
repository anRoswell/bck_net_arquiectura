namespace Core.DTOs.RegistrarGestionOrdenMovilDto
{
    using System.Collections.Generic;
    using Core.DTOs.CuestionarioInstanciasMovilDto;

    public class ReconexionMovilDto
	{
        public int? accion_rcs { get; set; }
        public int? subactividad { get; set; }
        public string observaciones { get; set; }
        public IList<SoporteRequestMovilDto> fotos { get; set; }
    }
}

