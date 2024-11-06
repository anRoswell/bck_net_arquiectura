using Newtonsoft.Json;

namespace Core.QueryFilters
{
    /*
    * fecha: 12/01/2024    
    * carlos vargas
    */
    public class QueryOp360GestionarContratistas
    {   
        //[JsonProperty("id_contratista")]
        //public int? Id_Contratista { get; set; }

        [JsonProperty("identificacion")]
        public string Identificacion { get; set; }

        [JsonIgnore]
        public int?[] Id_Ordenes { get; set; }

        [JsonProperty("id_orden_string")]
        public string Id_Ordenes_String
        {
            get
            {
                return Id_Ordenes != null ? string.Join(",", Id_Ordenes) : "";
            }
        }

        
        [JsonIgnore]
        public bool Asignar_Contratista { get; set; }

        [JsonProperty("contratista_asignar")]
        public string Contratista_Asignar { 
            get
            {
                return Asignar_Contratista ? "S" : "N";
            }
        }
    }
}