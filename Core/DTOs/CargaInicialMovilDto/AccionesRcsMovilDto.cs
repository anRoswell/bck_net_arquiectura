namespace Core.DTOs.CargaInicialMovilDto
{
	public class AccionesRcsMovilDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int id_tipo_orden { get; set; }
        public int? id_resultado { get; set; }
    }
}