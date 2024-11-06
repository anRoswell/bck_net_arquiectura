
using System;

#nullable disable

namespace Core.Entities
{
    public partial class ReqArtSerRequeridosDoc
    {
        public int RasrdIdReqArtSerRequeridosDoc { get; set; }
        public int RasrdCodReqArtSerRequeridos { get; set; }
        public int RasrdCodTipoDocArtSerRequeridos { get; set; }
        public string RasrdNameDocument { get; set; }
        public string RasrdExtDocument { get; set; }
        public int? RasrdSizeDocument { get; set; }
        public string RasrdUrlDocument { get; set; }
        public string RasrdUrlRelDocument { get; set; }
        public string RasrdOriginalNameDocument { get; set; }
        public int RasrdEstadoDocumento { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }

    public partial class ReqArtSerRequeridosDoc
    {
        public int ItemArticulo { get; set; }
        public string RasrEmpresaApot { get; set; }
        public int RasrSolicitudApot { get; set; }
        public string RasrCodigoArticuloApot { get; set; }
        public int RasrLineaApot { get; set; }
    }
}
