using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public partial class Usuario : BaseEntity
    {
        public string UsrCedula { get; set; }
        public int UsrTusrCodTipoUsuario { get; set; }
        public string UsrNombres { get; set; }
        public string UsrNombreCompleto { get; set; }
        public string UsrNombreTipoUsuario { get; set; }
        public string UsrApellidos { get; set; }
        public string UsrEmail { get; set; }
        public string UsrPassword { get; set; }
        public int UsrEmpresaProceso { get; set; }
        public bool UsrTmpSuspendido { get; set; }
        public string UsrToken { get; set; }
        public bool? UsrForcePasswordChange { get; set; }
        public int UsrEstado { get; set; }
        public DateTime? UsrUltimaFechaPassword { get; set; }
        public string CodArchivo { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }

    public partial class Usuario : BaseEntity
    {
        public string Estado { get; set; }
    }
}
