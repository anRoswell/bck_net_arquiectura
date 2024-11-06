using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class ReqParticipantes
    {
        public int IdReqParticipantes { get; set; }
        public int CodRequerimento { get; set; }
        public int CodProveedor { get; set; }
        public DateTime FecOfePre { get; set; }
        public string Observacion { get; set; }
        public string UrlPdfParticipacion { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
