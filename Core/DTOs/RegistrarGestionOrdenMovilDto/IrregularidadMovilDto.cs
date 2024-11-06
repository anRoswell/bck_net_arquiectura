namespace Core.DTOs.RegistrarGestionOrdenMovilDto
{
    using System.Collections.Generic;
    using Core.DTOs.CuestionarioInstanciasMovilDto;

    public class IrregularidadMovilDto
	{
        public int? accion_ri { get; set; }
        public string observaciones { get; set; }
        public IList<SoporteRequestMovilDto> fotos { get; set; }
    }
}

