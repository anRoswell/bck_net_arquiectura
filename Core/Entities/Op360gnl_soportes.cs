using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class gnl_soportes : BaseEntityOracle
    {
        //public int ID_SOPORTE { get; set; }
        public int ID_ACTIVIDAD { get; set; }
        public int ID_TIPO_SOPORTE { get; set; }
        //public string BFILE { get; set; }
        public string NOMBRE { get; set; }
        public string PESO { get; set; }
        public int ID_USUARIO_REGISTRO { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
        public string FORMATO { get; set; }
        public string IND_ARCHIVO_EXTERNO { get; set; }
        public string URL_EXTERNA { get; set; }
    }
}
