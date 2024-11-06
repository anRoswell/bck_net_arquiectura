using Core.DTOs;
using System;
using System.Collections.Generic;

namespace Core.QueryFilters
{
    public class QuerySolicitudModificacion : BaseQuery
    {
        public int IdContrato { get; set; }
        public string ContObjetoContrato { get; set; }
        public string ContCarateristicasEspecificas { get; set; }
        public string? ContDuracionContrato { get; set; }
        public DateTime? ContVigenciaDesde { get; set; }
        public DateTime? ContVigenciaHasta { get; set; }
        public decimal? ContValorContrato { get; set; }
        public string? ContCodFormaPago { get; set; }
        public string Observaciones { get; set; }
        public string ConsecutivoAlternoOtroSi { get; set; }
        public int ContCodRepresentanteLegal {  get; set; }
        public List<AprobadoresContratoDto> Aprobadores { get; set; }
    }
}
