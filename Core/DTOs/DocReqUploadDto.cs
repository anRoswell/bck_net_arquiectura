using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs
{
    public class DocReqUploadDto
    {
        public int? Id { get; set; }
        public string DrpuUrl { get; set; }
        public string DrcoNameDocument { get; set; }
        public string DrcoExtDocument { get; set; }
        public int? DrcoSizeDocument { get; set; }
        public string DrcoUrlDocument { get; set; }
        public string DrcoUrlRelDocument { get; set; }
        public string DrcoOriginalNameDocument { get; set; }
        public int KeyFile { get; set; }
    }
}
