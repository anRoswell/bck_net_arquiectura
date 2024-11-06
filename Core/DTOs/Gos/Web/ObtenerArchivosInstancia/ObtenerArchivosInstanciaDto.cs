namespace Core.DTOs.Gos.ObtenerArchivosInstancia
{
	public class ObtenerArchivosInstanciaDto
	{
        public int id_archivo_instancia { get; set; }
        public int id_archivo { get; set; }
        public string nombre_archivo { get; set; }
        public int numero_registros_archivo { get; set; }
        public int numero_registros_procesados { get; set; }
        public int numero_errores { get; set; }
        public string fecha_inicio_cargue { get; set; }
        public string fecha_fin_cargue { get; set; }
        public string duracion { get; set; }
        public int id_usuario_registro { get; set; }
        public string fecha_registro { get; set; }
        public int id_estado_intancia { get; set; }
        public string observaciones { get; set; }
        public int id_soporte { get; set; }
        public string pathwebdescarga { get; set; }
        public string nombre_usuario_registro { get; set; }
    }
}