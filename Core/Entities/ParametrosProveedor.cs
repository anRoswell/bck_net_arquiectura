using System.Collections.Generic;

namespace Core.Entities
{
    public class ParametrosProveedor
    {
        public List<Ciudade> Ciudades { get; set; }
        public List<Departamento> Departamentos { get; set; }
        public List<Empresa> Empresas { get; set; }
        public List<PrvCondicionesPago> CondicionesPagos { get; set; }
        public List<PrvProdServ> ProductoServicios { get; set; }
        public List<TipoProveedor> TipoProveedores { get; set; }
        public List<Documento> Documentos { get; set; }
        public List<Banco> Bancos { get; set; }
        public List<PrvListaRestrictiva> ListaRestrictivas { get; set; }
        public List<Estados> EstadosProveedor { get; set; }
    }
}
