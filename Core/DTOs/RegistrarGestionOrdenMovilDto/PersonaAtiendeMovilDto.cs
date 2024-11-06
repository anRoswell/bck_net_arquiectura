namespace Core.DTOs.RegistrarGestionOrdenMovilDto
{
	public class PersonaAtiendeMovilDto
	{
        public string nombre_completo { get; set; }
        public string identificacion { get; set; }
        public string telefono { get; set; }
        public int relacion_con_titular { get; set; }
        public bool solicita_asesoria { get; set; }
    }
}

