namespace Core.DTOs.ObtenerOrdenesTrabajoOficinaCentralDto
{
    using Core.QueryFilters;

    public class ObtenerOrdenesTrabajoOficinaCentralSpDto
	{
        public FiltrosDto filtros { get; set; }
        public QueryOp360ServerSide ServerSideJson { get; set; }
    }
}