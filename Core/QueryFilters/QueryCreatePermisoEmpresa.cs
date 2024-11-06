using System;
using System.Collections.Generic;
using System.Text;

namespace Core.QueryFilters
{
    public class QueryCreatePermisoEmpresa
    {
        public int Id { get; set; }
        public int PeuUsrCodUsuario { get; set; }
        public List<int> PeuEmpCodEmpresa { get; set; }
        public string CodUser { get; set; }
        public string CodUserUpdate { get; set; }
    }
}
