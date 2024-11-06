namespace Core.DTOs.Gos.GetOrdenById
{
    using System;

    public class AdjuntoDto
    {
        public DateTime fecha_registra { get; set; }
        public string formato { get; set; }
        public int id_soporte { get; set; }
        public int id_tipo_soporte { get; set; }
        public string desc_tipo_soporte { get; set; }
        public int id_usuario_registra { get; set; }
        public string ind_url_externo { get; set; }
        public string nombre { get; set; }
        public string peso { get; set; }
    }
}