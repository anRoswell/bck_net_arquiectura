namespace Core.DTOs.RegistrarGestionOrdenMovilDto
{
    using System.Collections.Generic;
    using Core.DTOs.CuestionarioInstanciasMovilDto;

    public class MaterialRetiradoMovilDto
	{
        public int? accion_mdv { get; set; }
        public IList<MaterialesMovilDto> materiales { get; set; }
        public string observaciones { get; set; }
        public IList<SoporteRequestMovilDto> fotos { get; set; }
    }
}