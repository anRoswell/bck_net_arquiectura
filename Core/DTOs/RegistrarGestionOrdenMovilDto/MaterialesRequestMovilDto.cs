namespace Core.DTOs.RegistrarGestionOrdenMovilDto
{
    using System.Collections.Generic;

    public class MaterialesRequestMovilDto : MaterialesMovilDto
    {
		public IList<SeriesMovilDto> series { get; set; }
	}
}

