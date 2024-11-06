using Core.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.QueryFilters
{
    public class QueryCreateContrato
    {
        public List<IFormFile> Files { get; set; }
        public string Contrato { get; set; }
    }
}
