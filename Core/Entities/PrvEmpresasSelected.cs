using System;

namespace Core.Entities
{
    public class PrvEmpresasSelected : BaseEntity
    {
        
        public int CodEmpresas { get; set; }
        public string NombreEmpresa { get; set; }
        public int CodProveedor { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
