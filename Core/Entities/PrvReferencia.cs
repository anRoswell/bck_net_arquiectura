using System;

#nullable disable

namespace Core.Entities
{
    public partial class PrvReferencia : BaseEntity
    {
        public int CodProveedor { get; set; }
        public string RefEmpresa { get; set; }
        public string RefTelefono { get; set; }
        public string RefContacto { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
