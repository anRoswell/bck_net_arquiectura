namespace Core.DTOs.ObtenerOrdenesTrabajoOficinaCentralDto
{
    public class ObtenerOrdenesTrabajoOficinaCentralRequestDto
    {
        public FiltrosDto filtros { get; set; }
        public string? ServerSide { get; set; } = null;
    }
}