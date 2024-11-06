using Core.Entities;
using Core.Interfaces;
using Core.QueryFilters;
using System.Collections.Generic;

namespace Core.CustomEntities
{
    public class GenerateFactPag
    {
        public QueryGenerateFacturaPagada parameters { get; set; }
        public List<Empresa> empresasActivas { get; set; }
        public IApoteosysService apoteosysService { get; set; }
    }
}
