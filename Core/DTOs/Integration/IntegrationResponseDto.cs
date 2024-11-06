using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Integration
{
    public class IntegrationResponseDto
    {
        public string codigo { get; set; }
        public string mensaje { get; set; }

        public IntegrationResponseDto(string codigo, string mensaje)
        {
            this.codigo = codigo;
            this.mensaje = mensaje;
        }
    }
}
