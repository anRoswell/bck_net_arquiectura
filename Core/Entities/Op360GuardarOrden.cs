using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Op360GuardarOrden
    {
        [Key]
        public string nic { get; set; }
        public string nis { get; set; }
        public string id_tipo_orden { get; set; }
        public string id_tipo_suspencion { get; set; }
        public string id_estado_servicio { get; set; }
    }
}
