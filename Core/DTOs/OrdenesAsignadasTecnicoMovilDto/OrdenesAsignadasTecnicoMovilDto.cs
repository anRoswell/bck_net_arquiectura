namespace Core.DTOs.OrdenesAsignadasTecnicoMovilDto
{
	public class OrdenesAsignadasTecnicoMovilDto
	{
        public string tipo { get; set; }
        public string nic { get; set; }
        public string orden { get; set; }
        public int id_orden { get; set; }
        public int id_tipo_orden { get; set; }
        public string direccion { get; set; }
        public string estado { get; set; }
        public GeorreferenciaMovilDto georreferencia { get; set; }
        public string numero_orden { get; set; }
        public string acta { get; set; }
        public string descripcion { get; set; }
        public string nombre_cliente { get; set; }
        public string municipio { get; set; }
        public string tarifa { get; set; }
        public string telefono_cliente { get; set; }
        public string carga_contratada { get; set; }
        public string numero_medidor { get; set; }
        public string marca_medidor { get; set; }
        public string fecha_ultima_factura { get; set; }
        public decimal deuda { get; set; }
        public int cantidad_facturas { get; set; }
        public string comentarios { get; set; }
        public decimal ultima_factura { get; set; }
        public string codigo_estado { get; set; }
        public string codigo_tipo_orden { get; set; }
        public int? numero_ruedas { get; set; }
    }
}