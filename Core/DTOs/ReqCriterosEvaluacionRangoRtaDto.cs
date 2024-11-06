using System;

namespace Core.DTOs
{
    public class ReqCriterosEvaluacionRangoRtaDto
    {
        public int Id { get; set; }
        public int CodReqCriterosEvaluacionRango { get; set; }
        public int RcRtaValorCriterioDesde { get; set; }
        public int RcRtaValorCriterioHasta { get; set; }
        public int RcRtaValorCriterio { get; set; }
        public bool? RcEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
        public int RcRtaLlaveCriterio { get; set; }
    }
}
