using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.QueryFilters
{
    public class QuerySeguimiento : BaseQuery
    {
        public string Seguimiento { get; set; }
        public IFormFile File { get; set; }
    }
}
