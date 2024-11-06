using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class Aire_ScrOrdConsultaClienteNicNis
    {
        public GetCliente_gnl_clientes[] clientes { get; set; }
    }

    public class GetCliente_gnl_clientes
    {
        [Key]
        public int id_cliente { get; set; }
        public int id_territorial { get; set; }
        public string nombre_territorial { get; set; }
        public int id_zona { get; set; }
        public string nombre_zona { get; set; }
        public int id_departamento { get; set; }
        public string nombre_departamento { get; set; }
        public int id_municipio { get; set; }
        public string nombre_municipio { get; set; }
        public string direccion { get; set; }
        public string nic { get; set; }
        public string nis { get; set; }
        public string nombre_cliente { get; set; }

        public string deuda { get; set; }
        public string tarifa { get; set; }
        public string cantidad_facturas { get; set; }
        public string telefono_cliente { get; set; }

        public string tipo_servicio { get; set; }
        public string tipo_conexion { get; set; }
        public string tipo_tension { get; set; }
        public string tipo_cliente { get; set; }
        public string nombre_finca { get; set; }
        public string circuito { get; set; }
        public string estado_servicio { get; set; }
        public string unicon { get; set; }
        public string trafo { get; set; }
        //public string apellidos_cliente { get; set; }
        //public string nombres_cliente { get; set; }
        public string nombre_barrio { get; set; }
        public string codigo_utl_susp { get; set; }
        public string numero_medidor { get; set; }
    }
}
