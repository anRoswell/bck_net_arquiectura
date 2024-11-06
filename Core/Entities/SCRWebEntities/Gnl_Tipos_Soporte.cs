using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.SCRWebEntities
{
    public class gnl_tipos_soporte : BaseEntityOracle2
    {
        //public int ID_TIPO_SOPORTE { get; set; }
        public string NOMBRE { get; set; }
        public string TIPOS_ARCHIVO { get; set; }
        public int TAMAÑO_MAXIMO { get; set; }
        public string IND_ACTIVO { get; set; }
        //public string CODIGO { get; set; }
    }
}
