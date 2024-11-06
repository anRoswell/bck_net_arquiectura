namespace Core.DTOs.Gos.GetOrdenById
{
    using System.Collections.Generic;

    public class OrdenDto
	{
        public DatosBasicosDto datos_basicos { get; set; }
        public IList<CambioEstadoDto> cambios_estado { get; set; }
        public DatosGestionDto datos_gestion { get; set; }
        public IList<AdjuntoDto> Adjuntos { get; set; }
        public IList<ComentariosDto> comentarios { get; set; }
    }
}