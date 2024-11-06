using System;

namespace Core.Entities
{
    public class ReqArtSerParticipante : BaseEntity
    {
        public int? CodOrdenApot { get; set; }
        public int CodArtSerRequerido { get; set; }
        public int CodProveedor { get; set; }
        public decimal Descuento { get; set; }
        public decimal Iva { get; set; }
        public decimal Valor { get; set; }
        public bool MarcaSolicitada { get; set; }
        public string Observacion { get; set; }
        public bool? ItemValido { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
