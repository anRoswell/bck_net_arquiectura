namespace Core.DTOs.FilesDto
{
    public class FileResponse
    {
        public string Extension { get; set; }
        public string NombreInterno { get; set; }
        public string NombreOriginal { get; set; }
        public string PathWebAbsolute { get; set; }
        public string PathWebRelative { get; set; }
        public double Size { get; set; }
    }
}

