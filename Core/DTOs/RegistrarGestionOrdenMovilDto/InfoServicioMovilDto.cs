namespace Core.DTOs.RegistrarGestionOrdenMovilDto
{
	public class InfoServicioMovilDto
	{
        public bool estado_servicio { get; set; }
        public int estado_predio { get; set; }
        public int uso_energia { get; set; }
        public int? tipo_uso_energia { get; set; } = null;
        public string numero_medidor { get; set; }
        public int? lectura { get; set; }
        public bool lectura_visible { get; set; }
        public int? tipo_no_visibilidad { get; set; }
        public string ct { get; set; }
        public string mt { get; set; }
    }
}

