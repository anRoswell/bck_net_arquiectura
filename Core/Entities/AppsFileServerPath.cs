using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public partial class AppsFileServerPath : BaseEntity
    {
        public int CodAplicaciones { get; set; }
        public string PathWeb { get; set; }
        public string PathRed { get; set; }
        public string PathRedArchivo { get; set; }
        public string PathWebArchivo { get; set; }
        public string Observacion { get; set; }
        public bool Estado { get; set; }
    }
}
