namespace Core.Entities
{
    public class gnl_rutas_archivo_servidor : BaseEntityOracle
    {
        public int ID_APLICACION { get; set; }
        public string RUTA_WEB { get; set; }
        public string RUTA_RED { get; set; }
        public string RUTA_RED_ARCHIVO { get; set; }
        public string RUTA_WEB_ARCHIVO { get; set; }
        public string OBSERVACION { get; set; }
        public string ESTADO { get; set; }        
    }
}