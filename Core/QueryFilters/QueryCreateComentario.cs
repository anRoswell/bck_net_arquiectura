using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.QueryFilters
{
    public class QueryCreateComentario
    {
        public string Comentario { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
