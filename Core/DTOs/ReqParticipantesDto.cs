using Core.Entities;
using System;
using System.Collections.Generic;

namespace Core.DTOs
{
    public class ReqParticipantesDto
    {
        public int IdReqParticipantes { get; set; }
        public int CodRequerimento { get; set; }
        public int CodProveedor { get; set; }
        public DateTime FecOfePre { get; set; }
        public string CodUser { get; set; }
        public string Observacion { get; set; }
        public string UrlPdfParticipacion { get; set; }
        public bool IsEnGuardadoTemporal { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
        public List<ReqRtaParticipante> Criterios { get; set; }
        public List<ReqArtSerParticipante> ArtSers { get; set; }
    }
}
