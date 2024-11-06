using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs
{
    public class ReqComentarioDto
    {
        public int Id { get; set; }
        public int CodRequerimiento { get; set; }
        public string RcComentario { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
