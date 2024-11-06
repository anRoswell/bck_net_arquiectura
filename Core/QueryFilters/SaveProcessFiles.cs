using Core.Options;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Core.QueryFilters
{
    public class SaveProcessFiles
    {
        public string PathCarpeta { get; set; }
        public List<IFormFile> Files { get; set; }
        public PathOptions PathOptions { get; set; }
    }
}
