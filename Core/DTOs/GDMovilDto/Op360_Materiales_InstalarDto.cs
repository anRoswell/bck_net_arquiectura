using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.GDMovilDto
{
    public class Op360_Materiales_InstalarDto
    {
        public IList<Op360_SeriesDto> series { get; set; }
        public int id_articulo { get; set; }
        public int cantidad { get; set; }
    }
}
