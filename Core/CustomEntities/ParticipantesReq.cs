using System.Collections.Generic;

namespace Core.CustomEntities
{
    public class ParticipantesReq
    {
        public int PrvIdProveedores { get; set; }
        public string PrvNit { get; set; }
        public string PrvNombreProveedor { get; set; }
        public string UrlPdfParticipacion { get; set; }
        public string PrvCodUsuario { get; set; }
        public List<ReqArtSerParticipanteCopy> ReqArtSerParticipantes { get; set; }
    }
}
