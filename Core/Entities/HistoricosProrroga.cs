using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public partial class HistoricosProrroga : BaseEntity
    {
     public int CodContrato { get; set; }
     public DateTime? VigenciaHasta { get; set; }
     public DateTime? VigenciaAnterior { get; set; }
     public DateTime FechaRegistro { get; set; }
     public string CodUserUpdate { get; set; }
     public DateTime FechaRegistroUpdate { get; set; }
     public string CodUser { get; set; }
     public string Info { get; set; }
     public string InfoUpdate { get; set; }
    }
}
