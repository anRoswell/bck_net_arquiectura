namespace Core.DTOs.Gos.Web.UpdateOrden
{
	public class SoporteUpdateDto
	{
        public string? base64 { get; set; } = null;
        public string? codigo_tipo_soporte { get; set; } = null;
        public int? id_adjunto { get; set; }
        public string? accion { get; set; } = null;
        public int? id_ruta { get; set; }
    }
}