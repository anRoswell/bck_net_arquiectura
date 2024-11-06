namespace Core.DTOs.Gos.UpdateOrden
{
    using Core.DTOs.Gos.Web.UpdateOrden;

    public class UpdateOrdenSpDto
	{
        public int? id_usuario { get; set; }
        public DatosBasicosUpdateDto datos_basicos { get; set; }
        public DatosGestionUpdateDto datos_gestion { get; set; }
    }
}

