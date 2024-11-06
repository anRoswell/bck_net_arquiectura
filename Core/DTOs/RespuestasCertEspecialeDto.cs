using System;

namespace Core.DTOs
{
    public class RespuestasCertEspecialeDto
    {
        public int Id { get; set; }
        public int RcesCodCertificadosEspeciales { get; set; }
        public string RcesObservacion { get; set; }
        public bool? RcesEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
