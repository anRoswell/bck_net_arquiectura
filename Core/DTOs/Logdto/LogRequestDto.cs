namespace Core.DTOs.Logdto
{
	public class LogRequestDto
	{
		public string e_nombre_up { get; set; }
        public string e_tipo_mensaje { get; set; } = "ERROR";
        public string e_mensaje { get; set; }
    }
}