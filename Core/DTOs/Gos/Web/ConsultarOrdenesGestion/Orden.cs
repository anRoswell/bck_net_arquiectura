namespace Core.DTOs.Gos.Web.ConsultarOrdenesGestion
{
    using System;

    public class Orden
	{
        public string id_orden { get;set; }
        public string numero_orden { get; set; }
        public string territorial { get; set; }
        public string municipio { get; set; }
        public string barrio { get; set; }
        public string fecha_creacion_programacion { get; set; }
        public string gestor_social { get; set; }
        public string editado { get; set; }
        public string actividad { get; set; }
        public string gestion { get; set; }
        public string accion { get; set; }
        public string tipo_publico { get; set; }
        public string actor_tipo { get; set; }
        public string mercado_tipo { get; set; }
        public string observaciones_programacion { get; set; }
        public string fecha_programacion { get; set; }
        public string hora_inicio { get; set; }
        public string hora_fin { get; set; }
        public string estado_actividad { get; set; }
        public string ejecucion_actividad { get; set; }
        public string direccion { get; set; }
        public string nombre_contacto { get; set; }
        public string nombre_entidad { get; set; }
        public string temas { get; set; }
        public string otro_tema { get; set; }
        public string subtemas { get; set; }
        public string otro_subtema { get; set; }
        public int? cantidad_asistentes { get; set; }
        public string observaciones { get; set; }
        public string nueva_fecha { get; set; }
        public string nueva_hora_inicio { get; set; }
        public string nueva_hora_fin { get; set; }
        public string observacion_incumplimiento { get; set; }
        public string fecha_ejecucion_real { get; set; }
        public string hora_incio_real { get; set; }
        public string hora_fin_real { get; set; }
        public string tiempo_ejecucion { get; set; }
        public int? mes { get; set; }
        public int? anio { get; set; }
        public string longitud { get; set; }
        public string latitud { get; set; }
        public int? nic { get; set; }
    }
}

