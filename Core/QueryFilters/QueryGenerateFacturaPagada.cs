namespace Core.QueryFilters
{
    public class QueryGenerateFacturaPagada
    {
        public string Empresa { get; set; }
        public string NitProveedor { get; set; }
        public string Fecha_Inicial { get; set; }
        public string Fecha_Final { get; set; }
        public string Tipo_Documento_Causacion { get; set; }
        public string Numero_Documento_Causacion { get; set; }
        public string Tipo_Documento_Alterno { get; set; }
        public string Numero_Documento_Alterno { get; set; }
    }
}
