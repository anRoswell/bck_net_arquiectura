namespace Core.DTOs.Gos.Web.UpdateOrden
{
    using System.Collections.Generic;

    public class DatosDesarrolloUpdateDto
	{
        public string? nombre_proyecto { get; set; } = null;
        public string? nombre_contacto { get; set; } = null;
        public int? id_tema { get; set; }
        public int? id_subtema { get; set; }
        public int? cantidad_asistentes { get; set; }
        public string? observaciones { get; set; } = null;
        public IList<AdjuntosUpdateDto> adjuntos { get; set; }
    }
}