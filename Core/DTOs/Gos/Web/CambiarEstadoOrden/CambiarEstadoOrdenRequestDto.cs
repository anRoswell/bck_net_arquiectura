namespace Core.DTOs.Gos.CambiarEstadoOrden
{
    public class CambiarEstadoOrdenRequestDto
	{
		public int id_orden { get; set; }
        public string estado { get; set; }
    }
}