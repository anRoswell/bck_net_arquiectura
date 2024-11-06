using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs
{
    public class TimelineDto
    {
        public string Nombre { get; set; }
        public List<Paso> Pasos { get; set; }
    }

    public class Paso
    {
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<User> Usuario { get; set; }
    }

    public class User
    {
        public string Nombre { get; set; }
    }
}
