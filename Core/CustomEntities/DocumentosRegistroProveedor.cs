using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CustomEntities
{
    public class DocumentosRegistroProveedor
    {
        public int PrvdIdPrvDocumentos { get; set; }
        public string PrvdUrlDocument { get; set; }
        public int PrvdCodDocumento { get; set; }
        public string PrvdNameDocument { get; set; }
        public string PrvdExtDocument { get; set; }
        public int PrvdSizeDocument { get; set; }
        public string PrvdUrlRelDocument { get; set; }
        public string PrvdOriginalNameDocument { get; set; }
        public int PrvdEstadoDocumento { get; set; }
        public int CocIdDocumentos { get; set; }
        public string CocNombreDocumento { get; set; }
        public bool Cocrequiered { get; set; }
        public bool CocVigencia { get; set; }
        public int CocVigenciaMaxima { get; set; }
        public int Vencido { get; set; }
        public DateTime FechaVencimiento { get; set; }
    }
}
