namespace Core.DTOs.ObtenerOrdenesTrabajoOficinaCentralDto
{
    using System.Collections.Generic;

    public class FiltrosDto
	{
        public int? id_contratista_persona_gestor { get; set; }
        public int id_estado_orden { get; set; }
        public IList<string> fecha_gestion { get; set; }
    }
}