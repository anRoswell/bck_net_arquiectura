namespace Core.QueryFilters
{
    public class QueryAprobacionContrato : BaseQuery
    {
        public int IdAprobadoresContrato { get; set; }
        public bool Aprobacion { get; set; }
        public string Justificacion { get; set; }
    }
}
