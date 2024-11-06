using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.GDMovilDto
{
    public class Op360_PreguntaDto
    {
        public int id_pregunta { get; set; }
        public int id_contratista_persona { get; set; }
        public int id_orden { get; set; }
        public int id_usuario { get; set; }
    }
}
