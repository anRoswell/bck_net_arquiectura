using Core.Entities;
using Core.Enumerations;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.QueryFilters
{
    public class QueryOp360ConfigExcel
    {
        public gnl_rutas_archivo_servidor rootFileServer { get; set; }
        public string TypeMime { get; set; }
        public string TituloReporteExcel { get; set; }
        public string NombreArchivo { get; set; }
        //public byte[] ArchivoByte { get; set; }
    }    
}
