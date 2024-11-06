using System;

namespace Core.CustomEntities
{
    public class ArticulosOfrecidosParticipanteReq
    {
        public int IdReqArtSerParticipante { get; set; }
        public int CodArtSerRequerido { get; set; }
        public int CodProveedor { get; set; }
        public decimal Descuento { get; set; }
        public decimal Iva { get; set; }
        public decimal Valor { get; set; }
        public bool MarcaSolicitada { get; set; }
        public string Observacion { get; set; }
        public bool? ItemValido { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }

        public int RasrIdReqArtSerRequeridos { get; set; }
        public int RasrCodRequerimiento { get; set; }
        public int RasrCodProdServ { get; set; }
        public int RasrItem { get; set; }
        public int RasrSolicitudApot { get; set; }
        public int RasrLineaApot { get; set; }
        public string RasrDepartamentoApot { get; set; }
        public string RasrObservacionApot { get; set; }
        public string RasrCodigoArticuloApot { get; set; }
        public string RasrDescripcionDteApot { get; set; }
        public string RasrDescripcionDteAlternaApot { get; set; }
        public string RasrUnidadMedidaApot { get; set; }
        public string RasrObservacionDteApot { get; set; }
        public int RasrCantidadApot { get; set; }
        public DateTime RasrFechaCreacionApot { get; set; }
        public int RasrTipoFichaTecnica { get; set; }
        public string RasrFichaTecnica { get; set; }
        public bool RasrAgrupado { get; set; }
        public decimal CantidadTotal { get; set; }
    }
}
