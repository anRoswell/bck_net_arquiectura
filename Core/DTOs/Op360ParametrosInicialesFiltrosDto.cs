using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class Op360ParametrosInicialesFiltrosDto
    {
        public Columna[] columnas { get; set; }
    }

    public class Columna
    {
        public int id_columna { get; set; }
        public string descripcion { get; set; }
        public string tipo_dato { get; set; }
        public string tipo_elemento { get; set; }
        public string ind_requerido { get; set; }
        public Operador[] operadores { get; set; }
    }

    public class Operador
    {
        public int id_operador { get; set; }
        public string descripcion { get; set; }
        public string operador { get; set; }
    }

}
