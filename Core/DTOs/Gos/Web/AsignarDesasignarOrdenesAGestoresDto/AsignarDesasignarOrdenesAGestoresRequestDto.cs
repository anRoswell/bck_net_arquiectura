namespace Core.DTOs.AsignarDesasignarOrdenesAGestoresDto
{
    using System.Collections.Generic;

    public class AsignarDesasignarOrdenesAGestoresRequestDto
    {
        public int id_gestor { get; set; }
        public IList<int> id_orden { get; set; }
        public bool asignar { get; set; }
    }
}