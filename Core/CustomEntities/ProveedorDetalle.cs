using Core.Entities;
using System.Collections.Generic;

namespace Core.CustomEntities
{
    public class ProveedorDetalle
    {
        public List<Proveedores> Proveedores { get; set; }
        public List<PrvSocio> Socios { get; set; }
        public List<PrvReferencia> Referencias { get; set; }
        public List<PrvEmpresasSelected> prvEmpresasSelecteds { get; set; }
        public List<PrvProdServSelected> prvProdServSelecteds { get; set; }
        public List<PrvDocumento> prvDocumentos { get; set; }
    }
}
