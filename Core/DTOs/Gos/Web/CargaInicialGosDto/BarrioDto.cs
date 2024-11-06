namespace Core.DTOs.CargaInicialGosDto
{
	public class BarrioDto: IdDescripcionDto
    {
        public int id_zona { get; set; }
        public int id_corregimiento { get; set; }
        public string codigo { get; set; }
    }
}