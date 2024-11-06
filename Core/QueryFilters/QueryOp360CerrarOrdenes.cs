using Newtonsoft.Json;

namespace Core.QueryFilters
{
    /*
    * fecha: 14/02/2024    
    * carlos vargas
    */
    public class QueryOp360CerrarOrdenes
    {   
        public int id_anomalia { get; set; }
        public int id_subanomalia { get; set; }
        public string observacion_cierre { get; set; }

        [JsonIgnore]
        public int?[] id_ordenes { get; set; }

        public string id_orden_string
        {
            get
            {
                return id_ordenes != null ? string.Join(",", id_ordenes) : "";
            }
        }        

        public int id_usuario_cierre { get; set; }
    }
}