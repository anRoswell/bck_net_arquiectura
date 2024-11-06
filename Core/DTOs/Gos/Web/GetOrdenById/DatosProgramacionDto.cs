namespace Core.DTOs.Gos.GetOrdenById
{
    public class DatosProgramacionDto
	{
        public int? id_actividad { get; set; }
        public string desc_actividad { get; set; }
        public int? id_gestion { get; set; }
        public string desc_gestion { get; set; }
        public int? id_accion { get; set; }
        public string desc_accion { get; set; }
        public int? id_departamento { get; set; }
        public string desc_departamento { get; set; }
        public int? id_municipio { get; set; }
        public string desc_municipio { get; set; }
        public int? id_barrio { get; set; }
        public string desc_barrio { get; set; }
        public int? id_publico_tipo { get; set; }
        public string desc_publico_tipo { get; set; }
        public int? id_actor_tipo { get; set; }
        public string desc_actor_tipo { get; set; }
        public int? id_mercado_tipo { get; set; }
        public string desc_mercado_tipo { get; set; }
		public string cuenta { get; set; }
		public string cliente { get; set; }
		public string otro_barrio { get; set; }
		public string desc_otro_barrio { get; set; }
        public int? id_corregimiento { get; set; }
        public string direccion { get; set; }
        public int? id_causal { get; set; }
        public int? id_anomalia { get; set; }
    }
}