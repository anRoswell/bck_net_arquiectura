using Core.Entities;
using System.Collections.Generic;

namespace Core.CustomEntities
{
    public class RequerimientoDetalle
    {
        public List<Requerimientos> Requerimientos { get; set; }
        public List<ReqArtSerRequeridos> ReqArtSerRequeridos { get; set; }
        public List<ReqCriterosEvaluacion> ReqCriteriosEvaluacion { get; set; }
        public List<ReqRtaCriteriosEvaluacion> ReqRtaCriteriosEvaluacion { get; set; }
        public List<ReqCriterosEvaluacionRangoRta> ReqCriterosEvaluacionRangoRta { get; set; }
        public List<ReqPolizasSeguro> ReqPolizasSeguro { get; set; }
        public List<ReqListDocumentos> ReqListDocumentos { get; set; }
        public List<Proveedores> ReqListProveedoresParticipantes { get; set; }
    }
}
