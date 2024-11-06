using System.Collections.Generic;

namespace Core.Entities
{
    public class Plantilla: BaseEntity
    {
    }

    public class PruebaDataTable
    {
        public List<FormasPago> FormasPagos { get; set; }
        public List<TipoUsuario> TipoUsuarios { get; set; }
    }
}
