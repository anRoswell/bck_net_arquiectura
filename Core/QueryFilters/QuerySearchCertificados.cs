namespace Core.QueryFilters
{
    public class QuerySearchCertificados : BaseQuery
    {
        public int Year { get; set; }
        public string Empresa { get; set; }
        public int Month { get; set; }
        public string NitProveedor { get; set; }
        public int IdEmpresa { get; set; }
        public string UrlToken { get; set; }
        public string TipoUsuario { get; set; }
        public int Periodicidad { get; set; }
    }

    public class QuerySearchCertificadosICA : QuerySearchCertificados
    {
        public bool IsBimensual { get; set; }
        public string Cuenta { get; set; }
    }
}
