namespace Core.DTOs.Gos.GetOrdenById
{
    public class CambioEstadoDto
	{
        public int id_orden_trazabilidad { get; set; }
        public int id_orden { get; set; }
        public int id_estado_orden_anterior { get; set; }
		public string desc_orden_anterior { get; set; }
        public int id_estado_orden_nuevo { get; set; }
		public string desc_orden_nuevo { get; set; }
        public int id_usuario_inicial { get; set; }
		public string nombre_usuario_inicial { get; set; }
        public int id_usuario_final { get; set; }
		public string nombre_usuario_final { get; set; }
        public string fecha_registra { get; set; }
    }
}