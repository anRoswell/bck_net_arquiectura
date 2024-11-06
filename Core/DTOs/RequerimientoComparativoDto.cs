using Core.CustomEntities;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs
{
    public class RequerimientoComparativoDto
    {
        public int Id { get; set; }
        public string ReqNombreCentroCosto { get; set; }
        public int ReqCodEmpresa { get; set; }
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
        public int ReqCodGestorContrato { get; set; }
        public int ReqEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
        public string UrlDocumento { get; set; }
        public List<ReqArtSerRequeridos> Articulos { get; set; }
        public List<ParticipantesReq> Participantes { get; set; }
    }
}
