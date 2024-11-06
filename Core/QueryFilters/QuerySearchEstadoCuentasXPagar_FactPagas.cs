namespace Core.QueryFilters
{
    public class QuerySearchEstadoCuentasXPagar_FactPagas
    {
        public string Empresa { get; set; }
        public string NitProveedor { get; set; }
        public string Numero_Documento_Alterno { get; set; } // Numero Factura
        public string Tipo_Documento_Causacion { get; set; }
        public string Numero_Documento_Causacion { get; set; }
    }
}
