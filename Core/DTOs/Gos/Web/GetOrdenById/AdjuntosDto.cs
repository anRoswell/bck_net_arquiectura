namespace Core.DTOs.Gos.GetOrdenById
{
    using System.Collections.Generic;

    public class AdjuntosDto
	{
        public IList<AdjuntoDto> Evidencia { get; set; }
        public IList<AdjuntoDto> Acta { get; set; }
        public IList<AdjuntoDto> Asistencia { get; set; }
        public IList<AdjuntoDto> Anomalia { get; set; }
        public IList<AdjuntoDto> Causal { get; set; }
        public IList<AdjuntoDto> Pdf { get; set; }
    }
}