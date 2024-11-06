namespace Core.DTOs.Gos.ObtenerArchivosInstanciaDetalle
{
    using Core.DTOs.ObtenerOrdenesTrabajoOficinaCentralDto;

    public class ObtenerArchivosInstanciaDetalleRequestDto : PaginationDto
    {
		public int id_archivo_instancia { get; set; }
	}
}