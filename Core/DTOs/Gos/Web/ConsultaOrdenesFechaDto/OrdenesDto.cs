namespace Core.DTOs.Gos.Web.ConsultaOrdenesFechaDto
{
	public class OrdenesDto
	{
		public int? id_orden { get; set; }
		public string numero_orden { get; set; }
		public string fecha_programacion { get; set; }
		public int? id_cliente { get; set; }
		public string desc_cliente { get; set; }
		public int? id_contratista_persona_gestor { get; set; }
		public string desc_contr_persona { get; set; }
		public int? id_estado_orden { get; set; }
		public string desc_esta_orden { get; set; }
		public string codigo_estado_orden { get; set; }
		public string ind_ambiente_registra { get; set; }
    }
}