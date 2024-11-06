using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public partial class PrvAdobeSign : BaseEntity
    {
        public int CodProveedor { get; set; }
        public int CodAdobeSignEstado { get; set; }
        public string AdsgnIdAdobeSign { get; set; }
        public string AdsgnJson { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
