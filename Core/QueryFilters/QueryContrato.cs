namespace Core.QueryFilters
{
    public class QueryContrato : BaseQuery
    {
        public int IdContrato { get; set; }
        public int ContEstado { get; set; }
        public string ContConsecutivoAlterno { get; set; }
    }
}
