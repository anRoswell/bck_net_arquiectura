using Newtonsoft.Json;
using System;

namespace Core.QueryFilters
{
    /*
    * fecha: 19/12/2023
    * clave: aaaaORDENESa65sd4f65sdf _02
    * carlos vargas
    */
    public class QueryOp360ReporteEjecutadosContratistas
    {
        public string id_contratista_persona { get; set; }
        public string id_zona { get; set; }
        public string id_persona { get; set; }
        public string id_usuario { get; set; }

        public string fecha { get; set; }
        public DateTime[] fechaJson { get; set; }
        public DateTime? fechaInicial { 
            get
            {
                if (fechaJson != null)
                {
                    return fechaJson[0];
                }
                return null;
            }
        }
        public DateTime? fechaFinal { 
            get
            {
                if (fechaJson != null)
                {
                    if (fechaJson.Length >= 2)
                    {
                        return fechaJson[1];
                    }
                }
                return null;
            }
        }

        public string? ServerSide { get; set; }
        public QueryOp360ServerSide ServerSideJson { get; set; }
    }

}