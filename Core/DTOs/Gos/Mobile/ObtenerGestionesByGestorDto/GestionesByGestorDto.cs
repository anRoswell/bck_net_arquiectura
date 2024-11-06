namespace Core.DTOs.Gos.Mobile.ObtenerGestionesByGestorDto
{
    using System;

    public class GestionesByGestorDto
    {
        public int? id_orden { get; set; }
        public string numero_orden { get; set; }
        public int? id_contratista_persona { get; set; }
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
        public string ind_ambiente_registra { get; set; }
        public DateTime fecha_programacion { get; set; }
        public DateTime fecha_registra { get; set; }
        public string nombre_cliente { get; set; }
        public int? cuenta { get; set; }
        public int? id_cliente { get; set; }
        public int? id_estado_orden { get; set; }
        public string descripcion_estado { get; set; }
        public string codigo_estado { get; set; }
        public string direccion { get; set; } //Agregue este campo
    }
}