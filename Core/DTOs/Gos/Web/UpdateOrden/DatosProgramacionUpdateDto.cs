namespace Core.DTOs.Gos.Web.UpdateOrden
{
	public class DatosProgramacionUpdateDto
	{
        public int? id_actividad { get; set; }
        public int? id_gestion { get; set; }
        public int? id_accion { get; set; }
        public int? id_departamento { get; set; }
        public int? id_municipio { get; set; }
        public int? id_barrio { get; set; }
        public string? otro_barrio { get; set; } = null;
        public string? direccion { get; set; } = null;
        public int? id_publico_tipo { get; set; }
        public int? id_actor_tipo { get; set; }
        public int? id_mercado_tipo { get; set; }
        public int? id_causal { get; set; }
        public int? id_cliente { get; set; }
        public int? id_corregimiento { get; set; }
    }
}