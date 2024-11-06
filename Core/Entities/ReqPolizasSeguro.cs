using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class ReqPolizasSeguro
    {
        public int rpsIdReqPolizasSeguro { get; set; }
        public int rpsCodRequirimiento { get; set; }
        public string rpsRiesgoAsociado { get; set; }
        public string rpsCuentia { get; set; }
        public string rpsVigencia { get; set; }
        public string rpsObservacion { get; set; }
        public bool rpsEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
