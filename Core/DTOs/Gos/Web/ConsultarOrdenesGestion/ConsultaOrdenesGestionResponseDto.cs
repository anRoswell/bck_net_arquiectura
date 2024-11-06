namespace Core.DTOs.Gos.Web.ConsultarOrdenesGestion
{
    using System.Collections.Generic;

    public class ConsultaOrdenesGestionResponseDto
	{
        public IList<Orden> ordenes { get; set; }
    }
}