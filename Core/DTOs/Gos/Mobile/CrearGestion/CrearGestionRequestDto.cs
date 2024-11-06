namespace Core.DTOs.Gos.Mobile.CrearGestion
{
    public class CrearGestionRequestDto
	{
        public int? id_departamento { get; set; }
        public int? id_municipio { get; set; }
        public int? id_corregimiento { get; set; }
        public int? id_barrio { get; set; }
        public string otro_barrio { get; set; }
        public int? id_actividad { get; set; }
        public int? id_gestion { get; set; }
        public int? id_accion { get; set; }
        public int? id_publico_tipo { get; set; }
        public int? id_actor_tipo { get; set; }
        public int? id_mercado_tipo { get; set; }
        public int? id_contratista_persona_gestor { get; set; }
        public int id_usuario_registra { get; set; }
        public string fecha_programacion { get; set; }
        public string hora_inicio { get; set; }
        public string hora_fin { get; set; }
    }
}