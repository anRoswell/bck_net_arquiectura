using Core.Entities;
using System.Collections.Generic;

namespace Core.CustomEntities
{
    public class ParamsInicialesReqPrv
    {
        public List<IdentidadesLocale> ListIva { get; set; }
        public List<TipoCriterio> TipoCriterios { get; set; }
        public List<TipoRequerimiento> TipoRequerimientos { get; set; }
    }
}
