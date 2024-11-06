namespace Core.DTOs.Gos.Web.Dashboard
{
    using System.Collections.Generic;

    public class OrdenesAgrupadaDto
	{
        public string contratista { get; set; }
        public string identificacion { get; set; }
        public List<ZonaDto> zonas { get; set; }
        public int NoRegistros { get; set; }
    }
}