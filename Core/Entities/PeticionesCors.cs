using System;

#nullable disable

namespace Core.Entities
{
    public partial class PeticionesCors : BaseEntity
    {
        public string RequestHeadersReferer { get; set; }
        public string Token { get; set; }
        public string Grupo { get; set; }
        public string ControllerName { get; set; }
        public string ActionMethod { get; set; }
        public string MethodType { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
