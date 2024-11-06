namespace Core.CustomEntities
{
    public class ArticulosRequerimiento
    {
        public int Id { get; set; } // Id Autoincrementable
        public string Scm_Status { get; set; } // Etado de Solicitud
        public string Scm_Dep_Departamento { get; set; } // Departamento de la Solicitud
        public string Scm_Solicitud_Compra { get; set; } // Numero de Solicitud
        public string Scm_Observaciones { get; set; } // Observaciones de la Solicitud
        public string Scm_Fecha_Creacion { get; set; } // Fecha de la Solicitud
        public string Scm_Emp_Empresa { get; set; } // Empresa de la Solicitud
        public string Dsc_Linea { get; set; } // Línea(item) del Articulo en la Solicitud
        public string Dsc_Ato_Articulo { get; set; } // Codigo del Articulo
        public string Art_Unidad_Medida { get; set; } // Unidad de Medida del Articulo
        public string Dsc_Descripcion { get; set; } // Descripción del articulo
        public string Dsc_Descripcion_Alterna { get; set; } // Descripción alterna del articulo
        public string Dsc_Cantidad { get; set; } // Cantidad Solicitada del Articulo
        public string Dsc_Aprobada { get; set; } // Indica si la Solictud es Aprobada
        public string Dsc_Observaciones { get; set; } // Observaciones del articulo

        public string Tipo_Solicitud { get; set; } // Tipo de Solicitud (Servicio, Inventario)
        public string Valor_Solicitud { get; set; } // Valor de la Solicitud
        public string Cantidad_Aprobada { get; set; } // Cantidad de articulos aprobados
    }
}
