namespace Core.QueryFilters
{
    public class QuerySearchEstadoCuentasPagadas : BaseQuery
    {
        public string Empresa { get; set; }
        public string NitProveedor { get; set; }
        public string Numero_Documento { get; set; } // Numero Factura
        public string Fecha_Inicial { get; set; }
        public string Fecha_Final { get; set; }
        public string TipoUsuario { get; set; }
    }
}
