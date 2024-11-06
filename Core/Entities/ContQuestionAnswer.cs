using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public class ContQuestionAnswer : BaseEntity
    {
        public ContQuestionAnswer()
        {
            InverseIdQuestionAnswerNavigation = new HashSet<ContQuestionAnswer>();
        }
        public int? IdQuestionAnswer { get; set; }
        public int? IdContrato { get; set; }
        public int? IdUsuario { get; set; }
        public bool? EsGestor { get; set; }
        public string Comentario { get; set; }
        public bool? ContieneAdjunto { get; set; }
        public bool? EsPrivado { get; set; }
        public bool? Estado { get; set; }
        public string FilePath { get; set; }
        public string FileRelativo { get; set; }
        public int? FileSize { get; set; }
        public string FileExt { get; set; }
        public string CodUser { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime? FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }

        public virtual ContQuestionAnswer IdQuestionAnswerNavigation { get; set; }
        public virtual ICollection<ContQuestionAnswer> InverseIdQuestionAnswerNavigation { get; set; }
    }
}
