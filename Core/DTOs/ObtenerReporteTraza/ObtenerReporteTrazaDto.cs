using System;

namespace Core.DTOs.ObtenerReporteTraza
{
    public class ObtenerReporteTrazaDto
    {
        public ReporteTraza[] reporte_traza { get; set; }
        public int? RegistrosTotales { get; set; }
    }

    public class ReporteTraza
    {
        //public int? id_orden_historico { get; set; }
        //public DateTime? fecha_registro_historico { get; set; }
        //public string? estado_historico { get; set; }
        public int? id_orden { get; set; }
        public string orden { get; set; }
        //public int? id_cliente { get; set; }
        public string tipo { get; set; }
        public string nombre_contratista_persona { get; set; }
        public string nombre_barrio { get; set; }
        public string estado { get; set; }
        public string contratista { get; set; }
        public string nombre_territorial { get; set; }
        public string tipo_suspension { get; set; }
        //public string nombre_zona { get; set; }
        //public string direcion { get; set; }
        public DateTime? fecha_generacion { get; set; }
        //public DateTime? fecha_cierre { get; set; }
        //public string usuario_cierre { get; set; }
        public string descripcion_de_tipo { get; set; }
        public string comentarios { get; set; }
        //public string acta { get; set; }
        //public string nombre_actividd { get; set; }
        //public string tipo_trabajo { get; set; }
        //public string actividad_orden { get; set; }
        //public DateTime? fecha_estimada_respuesta { get; set; }
        //public string numero_camp { get; set; }
        //public string comentario_orden_servicio_num1 { get; set; }
        //public string comentario_orden_servicio_num2 { get; set; }
        //public string observacion_rechazo { get; set; }
        //public DateTime? fecha_rechazo { get; set; }
        public string origen { get; set; }
        public DateTime? fecha_ingreso_op360 { get; set; }
        //public DateTime? fecha_asigna_contratista { get; set; }
        //public DateTime? fecha_asigna_tecnico { get; set; }
        public int? nic { get; set; }
        public double? deuda { get; set; }
        public string tecnico { get; set; }
        public string departamento { get; set; }
        public string municipio { get; set; }
        public int? numero_factura { get; set; }
        public string tipo_brigada { get; set; }
        public int? antiguedad { get; set; }
        //campo calculado
        public DateTime? fecha_consulta { get; set; }
    }
}