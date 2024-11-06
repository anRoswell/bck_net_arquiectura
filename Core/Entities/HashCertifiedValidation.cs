using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public partial class HashCertifiedValidation
    {
        public int IdHashCertifiedValidation { get; set; }
        public string CodigoHash { get; set; }
        public string CertifiedHtml { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
