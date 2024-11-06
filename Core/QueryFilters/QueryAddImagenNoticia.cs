using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Core.QueryFilters
{
    public class QueryAddImagenNoticia : BaseQuery
    {
        public List<IFormFile> Files { get; set; }
        public int CodNoticia { get; set; }
    }
}
