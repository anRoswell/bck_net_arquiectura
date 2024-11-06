using System;

#nullable disable

namespace Core.Entities
{
    public partial class PrvSocio : BaseEntity
    {
        public int CodProveedor { get; set; }
        public string SocNombre { get; set; }
        public string SocCodCiudad { get; set; }
        public string NombreCiudad { get; set; }
        public string SocIdentificacion { get; set; }
        public string IdentSocCompleta { get; set; }
        public string SocDigVerificacion { get; set; }
        public string SocDireccion { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
