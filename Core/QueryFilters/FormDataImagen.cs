using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Core.QueryFilters
{
    /// <summary>
    /// Almacenamiento Imagen
    /// </summary>
    public class FormDataImagen
    {
        public List<IFormFile> Files { get; set; }
        public int IdPathFileServer { get; set; }
        public string Carpeta { get; set; }
    }
}
