using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs
{
    public class ReqRtaCriteriosEvaluacionDto
    {
        public int rcriIdReqRtaCriteriosEvaluacion { get; set; }
        public int rcriCodRequerimiento { get; set; }
        public int rcriCodCriterio { get; set; }
        public string rcriRtaCriterio { get; set; }
        public int rcriRtaValorCriterio { get; set; }
        public bool rcriEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
        public int rcriRtaLlaveCriterio { get; set; }
    }
}
