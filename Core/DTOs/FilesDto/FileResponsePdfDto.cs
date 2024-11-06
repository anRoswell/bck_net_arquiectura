using System.IO;
using System.Net;
using System.Net.Http;

namespace Core.DTOs.FilesDto
{
    public class FileResponsePdfDto
    {
        public Stream File { get; set; }
        public HttpResponseMessage Response { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Mensaje { get; set; }
        public string UrlReporteJasper { get; set; }
    }
}

