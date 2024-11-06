namespace Core.DTOs.ObtenerGestionesByGestorDto
{
    using System.Collections.Generic;
    using Core.DTOs.Gos.Mobile.ObtenerGestionesByGestorDto;

    public class ObtenerGestionesByGestorResponseDto
	{
        public IList<GestionesByGestorDto> Gestiones { get; set; }
    }
}