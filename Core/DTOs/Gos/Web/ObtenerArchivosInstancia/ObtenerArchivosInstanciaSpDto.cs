namespace Core.DTOs.Gos.Web.ObtenerArchivosInstancia
{
    using Core.DTOs.ObtenerOrdenesTrabajoOficinaCentralDto;

    public class ObtenerArchivosInstanciaSpDto : PaginationDto
	{
        public int id_ruta_archivo_servidor { get; set; }
    }
}