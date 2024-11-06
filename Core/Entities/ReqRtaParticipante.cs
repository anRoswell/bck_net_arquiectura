using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class ReqRtaParticipante
    {
        public int IdReqRtaParticipante { get; set; }
        public int CodCriterio { get; set; }
        public int CodProveedor { get; set; }
        public int Respuesta { get; set; }
        public int CodReqRangoCriteriosEvaluacion { get; set; }
        public int CodReqRtaCriteriosEvaluacion { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
