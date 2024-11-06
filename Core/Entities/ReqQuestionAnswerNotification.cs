using System;

#nullable disable

namespace Core.Entities
{
    public partial class ReqQuestionAnswerNotification: BaseEntity
    {
        public int RqanCodRequerimiento { get; set; }
        public int RqanCodProveedor { get; set; }
        public int RqanCodUsuario { get; set; }
        public bool RqanEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
