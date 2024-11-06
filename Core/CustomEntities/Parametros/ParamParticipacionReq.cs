using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Options;
using System.Collections.Generic;

namespace Core.CustomEntities.Parametros
{
    public class ParamParticipacionReq
    {
        public ReqParticipantesDto ReqParticipantesDto { get; set; }
        public List<PrvDocumento> PrvDocumentos { get; set; }
        public List<ReqDocumentosRequerido> ReqDocumentosRequeridos { get; set; }
        public PathOptions PathOptions { get; set; }
        public IFilesProcess FilesProcess  { get; set; }
    }
}
