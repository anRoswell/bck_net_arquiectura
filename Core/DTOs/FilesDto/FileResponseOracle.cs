namespace Core.DTOs.FilesDto
{
    using System;

    public class FileResponseOracle
    {
        public string Extension { get; set; }
        public string NombreInterno { get; set; }
        public string NombreOriginal { get; set; }
        public Uri PathWebDescarga { get; set; }
        public int Id_Soporte { get; set; }
        public long Size { get; set; }
    }

    public class FileResponseBytes
    {
        public byte[] _file { get; set; }
        public string TypeMime { get; set; }
        public string Nombre { get; set; }
    }
}

