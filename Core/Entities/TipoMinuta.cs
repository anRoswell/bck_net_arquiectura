﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class TipoMinuta : BaseEntity
    {
        public string Nombre { get; set; }
        public string Url { get; set; }
        public string NameDocument { get; set; }
        public string ExtDocument { get; set; }
        public int? SizeDocument { get; set; }
        public string UrlDocument { get; set; }
        public string UrlRelDocument { get; set; }
        public string OriginalNameDocument { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}