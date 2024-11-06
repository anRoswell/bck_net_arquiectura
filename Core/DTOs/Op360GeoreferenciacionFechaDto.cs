using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class Op360GeoreferenciacionFechaDto
    {
        public int id_contratista_persona { get; set; }
        public int id_usuario { get; set; }
        public int id_persona { get; set; }
        public string nombre_contratista_persona { get; set; }
        public string identificacion_contratista_persona { get; set; }
        public string ind_activo { get; set; }
        public int id_contratista { get; set; }
        public string identificacion_contratista { get; set; }
        public string nombre_contratista { get; set; }
        public DateTime fecha { get; set; }
        public Georreferencia georreferencia { get; set; }
    }
    
    public class Georreferencia
    {
        public float longitud { get; set; }
        public float latitud { get; set; }
    }

}
