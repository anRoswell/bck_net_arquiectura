namespace Core.QueryFilters
{
    public class QueryUpdateEstadoPrv : BaseQuery
    {
        public string PrvNit { get; set; }
        public int Estado { get; set; }
        public string Observaciones { get; set; }
    }
}
