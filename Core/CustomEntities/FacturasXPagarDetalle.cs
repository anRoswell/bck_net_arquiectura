using Core.Entities;
using System.Collections.Generic;

namespace Core.CustomEntities
{
    public class FacturasXPagarDetalle
    {
        public List<EstadoCuentasXPorPagar> Factura { get; set; }
        public List<EstadoCuentasXPagarDetalle> DetallesFactura { get; set; }
    }
}
