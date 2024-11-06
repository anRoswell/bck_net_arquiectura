using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Core.QueryFilters
{
    public class CommandCreateComentarioContrato
    {
        public string Comentario { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
