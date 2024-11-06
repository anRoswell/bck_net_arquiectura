using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.SCRWebEntities
{
    public class gnl_actividades : BaseEntityOracle2
    {
        //public int ID_ACTIVIDAD { get; set; }
        public string NOMBRE { get; set; }
        public string DESCRIPCION { get; set; }
        //public string PREFIJO { get; set; }
        public string IND_ACTIVO { get; set; }
    }
}
