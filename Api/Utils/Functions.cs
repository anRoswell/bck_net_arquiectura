using Core.DTOs.FilesDto;
using Core.ModelProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Utils
{
    public static class Functions
    {
        public static class Validations 
        {
            /*Predicates*/
            public static bool IsFileByFilename(FileResponse file, string filename)
            {
                return $"{file.NombreOriginal}{file.Extension}".Equals(filename);
            }
        }
    }
}
