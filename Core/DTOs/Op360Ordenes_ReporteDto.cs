using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class Op360Ordenes_ReporteDto
    {
        public int codigo { get; set; }
        public string mensaje { get; set; }
        public List<Dato> datos { get; set; }
    }

    public class Dato
    {
        public string key { get; set; }
        public string valor { get; set; }
    }

}
