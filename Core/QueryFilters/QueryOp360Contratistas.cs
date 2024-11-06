using Newtonsoft.Json;

namespace Core.QueryFilters
{
    /*
    * fecha: 12/01/2024    
    * carlos vargas
    */
    public class QueryOp360Contratistas
    {   
        [JsonProperty("identificacion")]
        public string Identificacion { get; set; }
    }
}