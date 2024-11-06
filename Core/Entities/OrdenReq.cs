using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class OrdenReq
    {
        public int Id { get; set; }
        public int CodOrden { get; set; }
        public int CodRequerimiento { get; set; }
        public string CodProveedor { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
