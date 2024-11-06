using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Core.QueryFilters
{
    public class QueryParticipacion
    {
        public List<IFormFile> FilesRequired { get; set; }
        public string DocumentosRequeridos { get; set; }
        public string Participacion { get; set; }
    }
}
