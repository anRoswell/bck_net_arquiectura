namespace Core.DTOs.Gos.Web.ConsultarOrdenesGestion
{
    using Core.DTOs.ObtenerOrdenesTrabajoOficinaCentralDto;

    public class ConsultaOrdenesGestionDto
    {
        public string Format { get; set; }
        public FiltrosDto filtros { get; set; }
    }

    public class ConsultaOrdenesGestionSpDto
    {
        public FiltrosDto filtros { get; set; }
    }
}