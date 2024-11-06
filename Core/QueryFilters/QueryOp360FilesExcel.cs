using Core.Enumerations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.QueryFilters
{
    public class QueryOp360FilesExcel
    {
        public List<IFormFile> Files { get; set; }
        public Enum_RutasArchivos Id_Ruta { get; set; }
    }
}
