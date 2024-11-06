namespace Core.DTOs.Gos.GetOrdenById
{
	public class ComentariosDto
	{
        public int id_orden_comentario { get; set; }
        public int id_orden { get; set; }
        public string comentario { get; set; }
        public int id_usuario_registra { get; set; }
        public string usuario { get; set; }
        public string fecha_registra { get; set; }
    }
}