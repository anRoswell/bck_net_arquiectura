using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Core.QueryFilters
{
    public class QueryCreateRequerimiento
    {
        public List<IFormFile> Files { get; set; }
        public string Requerimiento { get; set; }
    }
}
