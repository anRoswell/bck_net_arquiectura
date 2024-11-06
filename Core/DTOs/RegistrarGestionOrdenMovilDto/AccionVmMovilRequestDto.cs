namespace Core.DTOs.RegistrarGestionOrdenMovilDto
{
    using Core.DTOs.CuestionarioInstanciasMovilDto;
    using System.Collections.Generic;

    public class AccionVmMovilRequestDto
	{
        public int? accion_vm { get; set; }
        public string observacion { get; set; }
        public IList<SoporteRequestMovilDto> fotos { get; set; }
    }
}

