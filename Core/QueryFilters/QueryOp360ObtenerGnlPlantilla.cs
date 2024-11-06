using Newtonsoft.Json;

namespace Core.QueryFilters
{
    /*
    * fecha: 14/02/2024    
    * carlos vargas
    */
    public class QueryOp360ObtenerGnlPlantilla
    {   
        public string codigo { get; set; }
        public string usuario { get; set; }
        public string contrasena { get; set; }
        public int? id_soporte { get; set; }
    }
}