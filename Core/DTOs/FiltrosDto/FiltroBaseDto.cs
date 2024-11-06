using Newtonsoft.Json;

namespace Core.Dtos.FiltrosDto
{
	public class FiltroBaseDto
    {
        public int id_columna_filtro { get; set; }
        public int id_operador { get; set; }
        public int? id_contratista { get; set; } 
        public string valor { get; set; }
    }
} 