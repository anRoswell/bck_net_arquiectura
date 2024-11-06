namespace Core.DTOs.RegistrarGestionOrdenMovilDto
{
    using System.Collections.Generic;

    public class MaterialesInstaladoMovilDto
	{
        public IList<MaterialesRequestMovilDto> materiales { get; set; }
        public string observaciones { get; set; }
    }
}

