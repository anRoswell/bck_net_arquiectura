namespace Core.DTOs.Gos.Web.Dashboard
{
    using System.Collections.Generic;

    public class ReporteDto
	{
        public List<OrdenesAgrupadaDto> ordenes_agrupadas { get; set; }
        public List<GraficaAsignacionDto> grafica_asignacion { get; set; }
    }
}

