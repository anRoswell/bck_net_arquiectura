namespace Core.DTOs.Gos.UpdateOrden
{
    using Core.DTOs.Gos.Web.UpdateOrden;

    public class UpdateOrdenRequestDto
	{
        public DatosBasicosUpdateDto datos_basicos { get; set; }
        public DatosGestionRequestUpdateDto datos_gestion { get; set; }
    }
}

