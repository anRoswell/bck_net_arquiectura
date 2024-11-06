namespace Core.DTOs.Gos.ObtenerArchivosInstanciaDetalle
{
    using System.Collections.Generic;

    public class ObtenerArchivosInstanciaDetalleResponseDto
	{
		public IList<ObtenerArchivosInstanciaDetalleDto> archivos_instancia_detalle { get; set; }
    }
}