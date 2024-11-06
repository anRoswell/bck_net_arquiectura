using Core.DTOs;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Core.QueryFilters
{
    public class QueryCreateProveedores
    {
        public List<IFormFile> FilesRequired { get; set; }
        public List<IFormFile> OthersFiles { get; set; }
        public string ListDocuments { get; set; }
        public string Proveedor { get; set; }
    }
}
