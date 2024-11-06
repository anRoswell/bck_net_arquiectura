using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.GDMovilDto
{
    public class Op360_SoporteResponseDTO
    {
        public string nombre { get; set; }
        public double peso { get; set; }
        public string formato { get; set; }
        public string url { get; set; }
        public string codigo_tipo_soporte { get; set; }

        public Op360_SoporteResponseDTO(string nombre, double peso, string formato, string url, string codigo_tipo_soporte)
        {
            this.nombre = nombre;
            this.peso = peso;
            this.formato = formato;
            this.url = url;
            this.codigo_tipo_soporte = codigo_tipo_soporte;
        }
    }
}
