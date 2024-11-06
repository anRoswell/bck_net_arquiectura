using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class gnl_peticiones_cors : BaseEntityOracle
    {
        //public int ID_PETICION_CORS { get; set; }
        public string encabezado_peticion_origen { get; set; }
        public string token { get; set; }
        public string grupo { get; set; }
        public string nombre_controlador { get; set; }
        public string metodo_accion { get; set; }
        public string typo_metodo { get; set; }
        public string usuario_registra { get; set; }
        public DateTime fecha_registra { get; set; }
    }
}
