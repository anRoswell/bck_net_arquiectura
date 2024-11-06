using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.FiltrosDto
{
    public class Op360_EditarFiltrosDto
    {
        public Filtro[] filtros { get; set; }
        public Columnas_Filtro2[] columnas_filtro { get; set; }
        public Contratista2[] contratistas { get; set; }
        public Dominios_Valor[] dominios_valor { get; set; }
    }

    public class Filtro
    {
        public int id_columna_filtro { get; set; }
        public int id_operador { get; set; }
        public string valor { get; set; }
        public int id_contratista { get; set; }
    }

    public class Columnas_Filtro2
    {
        public string nombre_columna { get; set; }
        public string tipo_dato { get; set; }
        public string tipo_elemento { get; set; }
        public string operadores { get; set; }
        public string origen { get; set; }
        public string ind_requerido { get; set; }
        public string ind_activo { get; set; }
        public string descripcion_columna { get; set; }
    }

    public class Contratista2
    {
        public int id_persona { get; set; }
        public string ind_activo { get; set; }
        public int codigo { get; set; }
    }

    public class Dominios_Valor
    {
        public int id_dominio { get; set; }
        public string descripcion { get; set; }
        public string valor { get; set; }
        public string ind_activo { get; set; }
        public string codigo { get; set; }
    }

}
