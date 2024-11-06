using System;

namespace Core.DTOs
{
    public class RepresentantesLegalEmpresaDto
    {
        public int Id { get; set; }

        public int RleCodEmpresa { get; set; }

        public string RleIdentificacionRteLegal { get; set; }

        public string RleNombreRteLegal { get; set; }

        public string RleEmailRteLegal { get; set; }

        public bool? RleEstado { get; set; }

        public string CodUser { get; set; }

        public DateTime FechaRegistro { get; set; }

        public string CodUserUpdate { get; set; }

        public DateTime FechaRegistroUpdate { get; set; }

        public string Info { get; set; }

        public string InfoUpdate { get; set; }
    }
}
