using Core.Entities;
using System.Collections.Generic;

namespace Core.CustomEntities
{
    public class FacturasPagadasDetalle
    {
        public List<EstadoCuentasPagadasDte> Factura { get; set; }
        public List<EstadoCuentasPagadasDetalle> DetallesFactura { get; set; }
    }
}
