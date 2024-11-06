using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Core.QueryFilters
{
    public class QueryCreateNoticia
    {
        public List<IFormFile> FilePrincipal { get; set; }
        //public List<IFormFile> FilesSecondary { get; set; }
        public string Noticia { get; set; }
    }
}
