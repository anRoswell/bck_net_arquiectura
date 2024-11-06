using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Core.QueryFilters
{
    public class QuerySendMailMasive
    {
        public string To { get; set; }
        public string CCO { get; set; }
        public string Asunto { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
        public int OpcionAEjecutar { get; set; }
    }
}
