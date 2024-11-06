namespace Core.CustomEntities
{
    public class AdjudicacionOrdenes
    {
        public int RasrIdReqArtSerRequeridos { get; set; }
        public int CodProveedor { get; set; }
        public string RasrEmpresaApot { get; set; }
        public int RasrSolicitudApot { get; set; }
        public int RasrLineaApot { get; set; }
        public string RasrCodigoArticuloApot { get; set; }
        public int Num_Orden { get; set; }
        public string Tipo_Orden { get; set; }
        public string Proveedor_Orden { get; set; }
        public float Total_Orden { get; set; }
        public string Fecha_Orden { get; set; }
        public float Total_Linea_Dte { get; set; }
        public string NombreOrden { get; set; }
    }
}
