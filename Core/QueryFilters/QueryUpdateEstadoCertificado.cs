namespace Core.QueryFilters
{
    public class QueryUpdateEstadoCertificado : BaseQuery
    {
        public int CodCertificado { get; set; }
        public int Estado { get; set; }
        public string Observaciones { get; set; }
        public string CerHtmlPdf { get; set; }
    }
}
