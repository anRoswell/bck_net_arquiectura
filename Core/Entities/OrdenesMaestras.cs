using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public partial class OrdenesMaestras : BaseEntity
    {
        public string empresa { get; set; }
        public string Numero { get; set; }
        public string Fecha { get; set; }
        public string Nit { get; set; }
        public string Moneda { get; set; }
        public string Tipo_Documento { get; set; }
        public string Numero_Documento { get; set; }
        public string Tipo_Orden { get; set; }
        public string Estado_Orden { get; set; }
        public string Condicion_Pago { get; set; }
        public double Sub_Total { get; set; }
        public double Descuento_Global { get; set; }
        public double Descuento { get; set; }
        public double Otro_Impuesto { get; set; }
        public double Iva { get; set; }
        public double Total { get; set; }
        public string Observaciones { get; set; }
        public string Departamento { get; set; }
    }

    public partial class OrdenesMaestras
    {
        public int? OrdReq { get; set; }
    }
}
