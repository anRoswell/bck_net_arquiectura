using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public partial class Ciudade
    {
        public string CodigoCiudad { get; set; }
        public string CodDepartamento { get; set; }
        public string Ciudad { get; set; }
        public bool Habilitado { get; set; }
    }
}
