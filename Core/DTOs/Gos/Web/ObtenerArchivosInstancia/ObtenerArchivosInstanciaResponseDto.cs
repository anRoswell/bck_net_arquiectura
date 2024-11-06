namespace Core.DTOs.Gos.ObtenerArchivosInstancia
{
    using System.Collections.Generic;

    public class ObtenerArchivosInstanciaResponseDto
	{
		public IList<ObtenerArchivosInstanciaDto> archivos_instancia { get; set; }
	}
}