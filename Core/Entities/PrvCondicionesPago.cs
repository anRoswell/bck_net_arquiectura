using System;

namespace Core.Entities
{
    public class PrvCondicionesPago : BaseEntity
    {
        public string CondDescripcion { get; set; }
        public bool? CondPagoEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
