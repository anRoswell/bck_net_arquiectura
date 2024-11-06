using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class ObtenerReporteLogLegalizacionDto
    {
        public Reportes_Legalizacion[] reportes_legalizacion { get; set; }
        public int? RegistrosTotales { get; set; }
    }
    public class Reportes_Legalizacion
    {
        public string numero_orden { get; set; }
        public string estado { get; set; }
        public string error { get; set; }
        public DateTime fecha_movimiento { get; set; }
        public Uri UrlDescargaActa { get; set; }
    }
}
