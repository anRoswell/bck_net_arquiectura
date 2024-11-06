namespace Core.DTOs.FilesDto
{
    using System;

    public class FileResponsePlantillasOracleBase64Dto
    {
        public int id_plantilla { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public string nombre_archivo { get; set; }
        public string typemime { get; set; }
        public string archivobase64 { get; set; }
        public byte[] archivobyte { 
            get
            {
                if (archivobase64 != null)
                {
                    return Convert.FromBase64String(archivobase64);
                }
                return null;
            }
        }

        public string mensaje { get; set; }
        public string extension
        {
            get
            {
                if (nombre_archivo != null)
                {
                    return string.Concat(".",nombre_archivo.Substring(nombre_archivo.LastIndexOf('.') + 1));
                }
                return null;
            }
        }
    }
}

