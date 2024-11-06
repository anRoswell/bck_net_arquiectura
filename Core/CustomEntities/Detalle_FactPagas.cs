using Core.Entities;
using System.Collections.Generic;

namespace Core.CustomEntities
{
    public class Detalle_FactPagas
    {
        public List<EstadoCuentasXPagar_FactPagas_Maestro> Factura { get; set; }
        public List<EstadoCuentasXPagar_FactPagas_Detalle> DetallesFactura { get; set; }
    }
}
