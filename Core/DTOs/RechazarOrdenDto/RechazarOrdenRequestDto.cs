namespace Core.Dtos.RechazarOrdenDto
{
    using System;

	public class RechazarOrdenRequestDto
	{
		public int id_orden { get; set; }
        public int id_contratista_persona { get; set; }
        public DateTime fecha_rechazo { get; set; }
        public string observacion_rechazo { get; set; }
    }
}