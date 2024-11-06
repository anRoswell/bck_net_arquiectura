namespace Core.DTOs.RegistrarGestionOrdenMovilDto
{
    using System.Collections.Generic;
    using Core.DTOs.CuestionarioInstanciasMovilDto;

    public class AccionVmMovilDto
	{
        public int accion_vm { get; set; }
        public string observacion { get; set; }
        public IList<SoporteMovilDto> fotos { get; set; }
    }
}

