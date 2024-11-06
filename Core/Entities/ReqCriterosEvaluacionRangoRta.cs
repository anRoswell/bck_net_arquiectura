using System;

#nullable disable

namespace Core.Entities
{
    public partial class ReqCriterosEvaluacionRangoRta : BaseEntity
    {
        public int CodReqCriterosEvaluacion { get; set; }
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

        //public virtual ReqCriterosEvaluacionRango CodReqCriterosEvaluacionRangoNavigation { get; set; }
    }
}
