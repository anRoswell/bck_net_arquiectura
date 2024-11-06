namespace Core.DTOs.Gos.CrearComentario
{
    public class CrearComentarioSpDto
	{
		public int id_orden { get; set; }
		public string comentario { get; set; }
        public int usuario_registra { get; set; }
    }
}