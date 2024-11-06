using Core.DTOs.OrdenesAsignadasTecnicoMovilDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.GDMovilDto
{
    public class Op360GD_OrdenesAsignadasTecnicoMovilDto
    {
        public int id_orden { get; set; }
        public string nic { get; set; }
        public int numero_orden { get; set; }
        public int numero_aviso { get; set; }
        public int tipo_aviso { get; set; }
        public string cliente { get; set; }
        public string telefono_cliente { get; set; }
        public string sector { get; set; }
        public string zona { get; set; }
        public string ct { get; set; }
        public string transformador { get; set; }
        public int sub_estacion { get; set; }
        public int circuito { get; set; }
        public string descripcion_incidencia { get; set; }
        public string observacion_telegestion { get; set; }
        public string referencia { get; set; }
        public string barrio { get; set; }
        public string ciudad { get; set; }
        public string direccion { get; set; }
        public int latitud { get; set; }
        public int longitud { get; set; }
    }

}
