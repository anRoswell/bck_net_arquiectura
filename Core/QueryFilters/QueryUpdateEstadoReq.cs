namespace Core.QueryFilters
{
    public class QueryUpdateEstadoReq : BaseQuery
    {
        public int CodRequerimiento { get; set; }
        public int EstadoReq { get; set; }
    }
}
