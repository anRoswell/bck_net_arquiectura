using Microsoft.AspNetCore.Http;

namespace Core.QueryFilters
{
    public class QueryHabilitarContrato : BaseQuery
    {
        public IFormFile File { get; set; }
        public string Contrato { get; set; }
        public QueryHabilitarContratoInfo Info { get; set; }
    }

    public class QueryHabilitarContratoInfo 
    {
        public int CodContrato { get; set; }
        public string PathTmp_Contrato { get; set; }
        public string PathRel_Contrato { get; set; }
        public string PathWebAbsolute_Contrato { get; set; }
        public string PathRoot_FS { get; set; }
        public string Path_Contrato { get; set; }
        public string Email_Proveedor { get; set; }
        public string Email_Rte_Legal { get; set; }
        public string NombreArchivo { get; set; }
    }
}
