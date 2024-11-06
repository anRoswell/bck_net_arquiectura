using Core.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class Op360ServerSideTestDto
    {
        public PruebaServerSideDatosDto[] clientes { get; set; }
        public int? RegistrosTotales { get; set; }
    }

    public class PruebaServerSideDatosDto
    {
        public int id_cliente { get; set; }
        public int id_zona { get; set; }
        public int nic { get; set; }
        public string nombre_cliente { get; set; }
        public DateTime? fecha_ultima_factura { get; set; }
        public int RegistrosTotales { get; set; }
    }
}
