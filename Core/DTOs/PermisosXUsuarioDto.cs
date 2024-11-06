namespace Core.DTOs
{
    public class PermisosXUsuarioDto
    {
        public int Pxu_IdPerfilesXUsuarios { get; set; }
        public int Pxu_Usr_CodUsuario { get; set; }
        public int Pxu_Apl_CodAplicacion { get; set; }
        public int Pxu_Prf_CodPerfil { get; set; }
        public string Prf_NombrePerfil { get; set; }
        public int Men_IdMenu { get; set; }
        public string Men_Controlador { get; set; }
        public string Men_Accion { get; set; }
        public string Men_Modulo_Descripcion { get; set; }
        public int Men_Tusr_CodTipoUsuario { get; set; }
        public string Tusr_Descripcion { get; set; }
        public string Pmp_Leer { get; set; }
        public string Pmp_Editar { get; set; }
        public string Pmp_Grabar { get; set; }
        public string Pmp_Borrar { get; set; }
    }
}
