using System;

namespace Core.DTOs
{
    public class ReqAdjudicacionDetalleDto
    {
        public int Id { get; set; }
        public int RadteCodReqAdjudicacion { get; set; }
        public int RadteCodProveedor { get; set; }
        public int RadteCodArtSerRequerido { get; set; }
        public int RadteCantidadAdjudicada { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
