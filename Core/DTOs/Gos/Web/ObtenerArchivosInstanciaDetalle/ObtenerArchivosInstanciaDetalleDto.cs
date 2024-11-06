namespace Core.DTOs.Gos.ObtenerArchivosInstanciaDetalle
{
	public class ObtenerArchivosInstanciaDetalleDto
	{
        public int id_archivo_instancia_detalle { get; set; }
        public int id_archivo_instancia { get; set; }
        public int numero_fila { get; set; }
        public string estado { get; set; }
        public string observaciones { get; set; }
    }
}