using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Integration
{
    public class Actividade
    {
        public int NRO_ACTIVIDAD { get; set; }
        public int TIPO_ACTIVIDAD { get; set; }
    }

    public class Apaconen
    {
        public string COD_MARCA { get; set; }
        public string NUM_APA { get; set; }
        public string TIP_CSMO { get; set; }
        public string NUM_RUE { get; set; }
        public string CSMO { get; set; }
        public string TIP_APA { get; set; }
        public string TIP_INTENSIDAD { get; set; }
        public string TIP_FASE { get; set; }
        public string LECT { get; set; }
        public string F_LECT { get; set; }
        public string NIVEL_TENSION { get; set; }
        public string PROPIEDAD_ACTIVO { get; set; }
    }

    public class Body
    {
        public int NUM_OS { get; set; }
        public int NUM_SOLICITUD { get; set; }
        public int TIPO_TRABAJO { get; set; }
        public string TIP_OS { get; set; }
        public string F_GEN { get; set; }
        public string F_ESTM_REST { get; set; }
        public int NIC { get; set; }
        public object NUM_CAMP { get; set; }
        public string TIPO_SUSPENSION { get; set; }
        public string COMENT_OS { get; set; }
        public object COMENT_OS2 { get; set; }
        public string DIRECCION { get; set; }
        public string F_CREATE { get; set; }
        public Datosum datosum { get; set; }
        public List<Actividade> actividades { get; set; }
        //public List<Precinto> precintos { get; set; }
        private List<Precinto> _precintos = new List<Precinto>();

        public List<Precinto> precintos
        {
            get { return _precintos; }
            set { _precintos = value ?? new List<Precinto>(); } // Si el valor es null, asigna una lista vacía.
        }

        public List<Recibo> recibos { get; set; }
        public List<Apaconen> apaconen { get; set; }
    }

    public class Datosum
    {
        public int NIS { get; set; }
        public string TIP_SERV { get; set; }
        public string COD_TAR { get; set; }
        public int TIP_CONEXION { get; set; }
        public string TIP_TENSION { get; set; }
        public double? POT { get; set; }
        public int MUNICIPIO { get; set; }
        public string LOCALIDAD { get; set; }
        public int BARRIO_ID { get; set; }
        public string OPERATING_SECTOR_ID { get; set; }
        public int ZONA_ID { get; set; }
        public object TIP_VIA { get; set; }
        public object CALLE { get; set; }
        public object NUM_PUERTA { get; set; }
        public object DUPLICADOR { get; set; }
        public object CGV_SUM { get; set; }
        public object NOM_FINCA { get; set; }
        public object REF_DIR { get; set; }
        public object ACC_FINCA { get; set; }
        public string APE1_CLI { get; set; }
        public object APE2_CLI { get; set; }
        public string NOM_CLI { get; set; }
        public string TFNO_CLI { get; set; }
        public int TIP_CLI { get; set; }
        public string TIPO_MEDIDA { get; set; }
        public int UNICON { get; set; }
        public string CIRCUITO { get; set; }
        public string TRAFO { get; set; }
        public double? FACTOR_MULT { get; set; }
        public string ESTADO_SERVICIO { get; set; }
        public int PREMISE_ID { get; set; }
        public double EXPIRED_BALANCE { get; set; }
        public int EXPIRED_PERIODOS { get; set; }
        public int ADDRESS_ID { get; set; }
    }

    public class ParameterHeader
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Precinto
    {
        public string COD_MARCA { get; set; }
        public string NUM_PRECIN { get; set; }
    }

    public class Recibo
    {
        public int SIMBOLO_VAR { get; set; }
        public string F_FACT { get; set; }
        public string F_VCTO_FACT { get; set; }
        public double IMP_TOT_REC { get; set; }
    }

    public class IntegrationDto
    {
        public List<object> parameterQuery { get; set; }
        public List<ParameterHeader> parameterHeader { get; set; }
        public Body body { get; set; }
    }

    public class IntegrationDtoFinal
    {
        public int p_num_msg { get; set; }
        public Body p_parametros { get; set; } //IntegrationDto
        public string p_programa { get; set; }
        public string p_mens { get; set; }
    }

    public class IntegrationStringDto
    {
        public List<object> parameterQuery { get; set; }
        public List<ParameterHeader> parameterHeader { get; set; }
        public string body { get; set; }
    }

    public class ParseoStringJson
    {
        public string stringJson { get; set; }
    }

}
