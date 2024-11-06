namespace Core.DTOs.Gos.ObtenerOrdenesTrabajoOficinaCentralDto
{
    using System;

    public class ObtenerOrdenesTrabajoOficinaCentralDto
	{
        public int? id_orden { get; set; }
        public string numero_orden { get; set; }
        public DateTime fecha_programacion { get; set; }
        public int? id_actividad { get; set; }
        public string desc_actividad { get; set; }
        public int? id_cliente { get; set; }
        public string desc_cliente { get; set; }
        public int? id_contratista_persona_gestor { get; set; }
        public string desc_contr_persona { get; set; }
        public int? id_estado_orden { get; set; }
        public string desc_esta_orden { get; set; }
        public string ind_ambiente_registra { get; set; }
        public string codigo_estado_orden { get; set; }
        public DateTime? fecha_gestion { get; set; }
        public int? id_gestion { get; set; }
        public int? id_accion { get; set; }
        public int? id_mercado_tipo { get; set; }
        public string desc_gestion { get; set; }
        public string desc_accion { get; set; }
        public string desc_mercado_tipo { get; set; }
    }
}