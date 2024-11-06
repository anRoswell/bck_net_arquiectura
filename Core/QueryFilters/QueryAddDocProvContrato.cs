using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Core.QueryFilters
{
    public class QueryAddDocProvContrato : BaseQuery
    {
        public List<IFormFile> Files { get; set; }
        public string Documentos { get; set; }
    }
}