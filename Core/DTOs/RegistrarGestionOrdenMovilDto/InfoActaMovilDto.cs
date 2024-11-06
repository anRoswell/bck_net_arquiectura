namespace Core.DTOs.RegistrarGestionOrdenMovilDto
{
	public class InfoActaMovilDto
	{
        public int? acta { get; set; } = 0;
        public string fecha_ejecucion { get; set; }
        public int id_orden { get; set; }
        public string orden { get; set; }
        public int id_contratista_persona { get; set; }
    }
}