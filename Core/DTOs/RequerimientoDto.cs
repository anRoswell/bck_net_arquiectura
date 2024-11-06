
using Core.Entities;
using System;
using System.Collections.Generic;

namespace Core.DTOs
{
    public class RequerimientoDto
    {
        public int Id { get; set; }
        public string ReqNombreCentroCosto { get; set; }
        public int ReqCodEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public int ReqCodAgencia { get; set; }
        public string ReqDescription { get; set; }
        public string ReqLugarEntrega { get; set; }
        public DateTime ReqCierraOferta { get; set; }
        public DateTime ReqCompraPrevista { get; set; }
        public DateTime ReqFechaEntrega { get; set; }
        public string ReqGarantiasExigidas { get; set; }
        public DateTime? ReqfinOfertaPresentada { get; set; }
        public int ReqReqType { get; set; }
        public int ReqCodGestorCompras { get; set; }
        public bool ReqRequiereContrato { get; set; }                                                    
        public int? ReqCodGestorContrato { get; set; }
        public int ReqEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
        //public List<ReqArtSerRequeridos> ReqArtSerRequeridos { get; set; }
        public List<ReqArtSerRequeridosDto> ReqArtSerRequeridos { get; set; }
        public List<ReqCriterosEvaluacionDto> ReqCriteriosEvaluacion { get; set; }
        public List<ReqPolizasSeguro> ReqPolizasSeguro { get; set; }
        public List<ReqListDocumentos> ReqListDocumentos { get; set; }
    }
}
