namespace Core.DTOs.Gos.Web.UpdateOrden
{
    public class AdjuntosUpdateDto
	{
        public int? id_adjunto { get; set; }
        public string? accion { get; set; } = null;
        public string? codigo_tipo_soporte { get; set; } = null;
        public string? nombre { get; set; } = null;
        public string? url { get; set; } = null;
        public string? formato { get; set; } = null;
        public double? peso { get; set; } = null;

        public AdjuntosUpdateDto(int? idAdjunto, string accion, string codigoTipoSoporte, string nombre, string url, string formato, double peso)
        {
            id_adjunto = idAdjunto;
            this.accion = accion;
            codigo_tipo_soporte = codigoTipoSoporte;
            this.nombre = nombre;
            this.url = url;
            this.formato = formato;
            this.peso = peso;
        }
    }
}