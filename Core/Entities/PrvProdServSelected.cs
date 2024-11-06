using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class PrvProdServSelected:BaseEntity
    {
    
        public int CodPrvProdServ { get; set; }
        public string NombreProducto { get; set; }
        public int CodProveedor { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
