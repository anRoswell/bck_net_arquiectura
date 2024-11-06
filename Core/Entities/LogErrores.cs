using System;

#nullable disable

namespace Core.Entities
{
    public partial class LogErrores : BaseEntity
    {
        public string Origen { get; set; }
        public string Controlador { get; set; }
        public string Funcion { get; set; }
        public string Descripcion { get; set; }
        public string Usuario { get; set; }
        public bool Estado { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
