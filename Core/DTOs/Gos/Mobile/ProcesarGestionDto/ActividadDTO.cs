namespace Core.DTOs.ProcesarGestionDto
{
    public class ActividadDto
    {
        public string nombre_contacto { get; set; }
        public string nombre_proyecto { get; set; }
        public string direccion { get; set; }
        public int? nic_usuario { get; set; }
        public int? id_tema { get; set; }
        public int? id_subtema { get; set; }
        public int? cantidad_asistentes { get; set; }
        public string observaciones { get; set; }
    }
}