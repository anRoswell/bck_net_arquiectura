using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.FiltrosDto
{
    public class Op360_ListarFiltrosResponseDto
    {
        public Columnas_Filtro[] columnas_filtro { get; set; }
        public Operador[] operador { get; set; }
        public Contratista[] contratistas { get; set; }
        public Filtro_Ordenes[] filtros_globales { get; set; }
    }

    public class Columnas_Filtro
    {
        public int codigo { get; set; }
        public string descripcion { get; set; }
        public string operadores { get; set; }
        public string tipo_dato { get; set; }
    }

    public class Operador
    {
        public int codigo { get; set; }
        public string descripcion { get; set; }
    }

    public class Contratista
    {
        public int codigo { get; set; }
        public string  identificacion { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public Filtro_Ordenes[] filtros { get; set; }

        
    }

    public class Filtro_Ordenes
    {
        public int id_filtro { get; set; }
        public int id_columna_filtro { get; set; }
        public int id_operador { get; set; }    
        public string valor { get; set; }
        public object id_contratista { get; set; }
    }


}
