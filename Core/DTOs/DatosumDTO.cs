using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class DatosumDTO
    {
        public int NIS { get; set; }
        public string TIP_SERV { get; set; }
        public string COD_TAR { get; set; }
        public int TIP_CONEXION { get; set; }
        public string TIP_TENSION { get; set; }
        public int POT { get; set; }
        public int MUNICIPIO { get; set; }
        public string LOCALIDAD { get; set; }
        public int BARRIO_ID { get; set; }
        public string OPERATING_SECTOR_ID { get; set; }
        public int ZONA_ID { get; set; }
        public int TIP_VIA { get; set; }
        public string CALLE { get; set; }
        public int? NUM_PUERTA { get; set; }
        public string DUPLICADOR { get; set; }
        public string CGV_SUM { get; set; }
        public string NOM_FINCA { get; set; }
        public string REF_DIR { get; set; }
        public string ACC_FINCA { get; set; }
        public string APE1_CLI { get; set; }
        public string APE2_CLI { get; set; }
        public string NOM_CLI { get; set; }
        public int TIP_CLI { get; set; }
        public string TIPO_MEDIDA { get; set; }
        public int UNICON { get; set; }
        public string CIRCUITO { get; set; }
        public string TRAFO { get; set; }
        public int FACTOR_MULT { get; set; }
        public string ESTADO_SERVICIO { get; set; }
        public int PREMISE_ID { get; set; }
        public double EXPIRED_BALANCE { get; set; }//lo cambie
        public int EXPIRED_PERIODOS { get; set; }
        public int ADDRESS_ID { get; set; }
    }
}
