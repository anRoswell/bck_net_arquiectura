using Core.Entities;
using System;
using System.Collections.Generic;

namespace Core.CustomEntities
{
    public class ParticipantesReqAdjudicacion
    {
        public int PrvIdProveedores { get; set; }
        public string PrvNit { get; set; }
        public string PrvNombreProveedor { get; set; }
        public int PrvTotalPuntaje { get; set; }
        public DateTime PrvFechaOfertaPresentada { get; set; }
        public string UrlPdfParticipacion { get; set; }
        public string PrvCodUsuario { get; set; }
        public List<RespuestaEvaluacionCriterio> RespuestaEvaluacionCriterios { get; set; }
        public List<ReqArtSerParticipanteCopy> ReqArtSerParticipantes { get; set; }
    }

    public class RespuestaEvaluacionCriterio
    {
        public Nullable<int> RcriIdReqCriteriosEvaluacion { get; set; }
        public Nullable<int> RcriTipoCriterio { get; set; }
        public string RcriTituloCriterio { get; set; }
        public RespuestaCriterio RespuestaCriterio { get; set; }
    }

    public class RespuestaCriterio
    {
        public int IdReqRtaParticipante { get; set; }
        public int CodCriterio { get; set; }
        public int CodProveedor { get; set; }
        public int Respuesta { get; set; }
        public int CodReqRangoCriteriosEvaluacion { get; set; }
        public int CodReqRtaCriteriosEvaluacion { get; set; }
        public int ValorCriterioSiNo { get; set; }
        public int ValorCriterioRango { get; set; }
        public int ValorCriterioSelUnica { get; set; }
        public string ValorRtaSiNo { get; set; }
        public string ValorRtaRango { get; set; }
        public string ValorRtaUnica { get; set; }
    }

}
