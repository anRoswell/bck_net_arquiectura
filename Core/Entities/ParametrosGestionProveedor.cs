
namespace Core.Entities
{
    using Core.DTOs;
    using System.Collections.Generic;

    public partial class ParametrosGestionProveedor
    {
        public List<PrvProdServ> PrvProdServ { get; set; }
        public List<ProveedorDto> Proveedores { get; set; }
        public List<Estados> EstadosProveedor { get; set; }
    }
}