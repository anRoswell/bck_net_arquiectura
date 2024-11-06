namespace Core.DTOs.Gos.GetOrdenById
{
    public class DatosDesarrolloDto
	{
        public string nombre_proyecto { get; set; }
        public string nombre_contacto { get; set; }
        public string nombre_barrio { get; set; }
        public int? cantidad_asistentes { get; set; }
        public int? id_tema { get; set; }
        public string desc_tema { get; set; }
        public int? id_subtema { get; set; }
        public string desc_subtema { get; set; }
        public string observaciones { get; set; }   
        public AdjuntosDto Adjuntos { get; set; }
    }
}