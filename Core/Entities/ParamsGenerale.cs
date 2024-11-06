using System;

#nullable disable

namespace Core.Entities
{
    public partial class ParamsGenerale : BaseEntity
    {
        public string PgeKeyParam { get; set; }
        public string PgeValorParam { get; set; }
        public byte[] PgeEncriptedValorParam { get; set; }
        public bool? PgeEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
