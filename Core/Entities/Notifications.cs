using System;

namespace Core.Entities
{
    public class Notifications : BaseEntity
    {
        public string TituloNotificacion { get; set; }
        public string ObsNotificacion { get; set; }
        public string UrlNotificacion { get; set; }
        public bool EstadoNotificacion { get; set; }
        public DateTime FechaNotificacion { get; set; }
    }
}
