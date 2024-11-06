using Newtonsoft.Json;

namespace Core.QueryFilters
{
    /*
    * fecha: 12/01/2024    
    * carlos vargas
    */
    public class QueryOp360ObtenerBrigadas
    {
        //[JsonProperty("id_contratista")]
        //public int? Id_Contratista { get; set; }

        //[JsonProperty("identificacion")]
        //public string Identificacion { get; set; }

        public string id_persona { get; set; }
        public string id_usuario { get; set; }
    }
}