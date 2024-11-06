using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class ReqCriterosEvaluacion
    {
        public int RcriIdReqCriteriosEvaluacion { get; set; }
        public int RcriCodRequerimiento { get; set; }
        public int RcriTipoCriterio { get; set; }
        public string RcriTituloCriterio { get; set; }
        public int? RcriValorCriterio { get; set; }
        public int? RcriVlCriNumDesde { get; set; }
        public int? RcriVlCriNumHasta { get; set; }
        public bool? RcriEstado { get; set; }
        public int? Respuesta { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
