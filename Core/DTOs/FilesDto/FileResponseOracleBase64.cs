namespace Core.DTOs.FilesDto
{
    using System;

    public class FileResponseOracleBase64
    {
        public int Id_Soporte { get; set; }
        public string NombreArchivo { get; set; }
        public string Extension { get; set; }
        public string TypeMime { get; set; }
        public long Size { get; set; }
        public string ArchivoBase64 { get; set; }        
    }
}

