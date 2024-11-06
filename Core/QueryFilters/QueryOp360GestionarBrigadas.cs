using Newtonsoft.Json;

namespace Core.QueryFilters
{
    /*
    * fecha: 12/01/2024    
    * carlos vargas
    */
    public class QueryOp360GestionarBrigadas
    {
        public string ind_areacentral { get; set; }

        [JsonProperty("identificacion_contratista")]
        public string Identificacion_Contratista { get; set; }

        [JsonProperty("identificacion_brigada")]
        public string Identificacion_Brigada { get; set; }

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
        public bool Asignar_Brigada { get; set; }

        [JsonProperty("brigada_asignar")]
        public string Brigada_Asignar { 
            get
            {
                return Asignar_Brigada ? "S" : "N";
            }
        }
    }
}