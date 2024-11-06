using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ASEGREDMovilDto
{
    public class Op360ASEGRED_RegistarGestionOrdenDto
    {
        public int NUM_OS { get; set; }
        public int NUM_SOLICITUD { get; set; }
        public int TIPO_TRABAJO { get; set; }
        public string TIP_OS { get; set; }
        public string F_GEN { get; set; }
        public string F_ESTM_REST { get; set; }
        public int NIC { get; set; }
        public string NUM_CAMP { get; set; }
        public string TIPO_SUSPENSION { get; set; }
        public string COMENT_OS { get; set; }
        public string COMENT_OS2 { get; set; }
        public string DIRECCION { get; set; }
        //public string F_CREATE { get; set; }
        public DatosumDTO DATOSUM { get; set; }
        public IList<ActividadesDto> ACTIVIDADES { get; set; }
        public IList<PrecintosDto> PRECINTOS { get; set; }
        public IList<RecibosDto> RECIBOS { get; set; }
        public IList<ApacoenDto> APACOEN { get; set; }

        //Mapeo de dto.
        public Op360ASEGRED_RegistarGestionOrdenDto(
            int num_Os, int num_Solicitud, int tipo_Trabajo, string tip_Os, string f_Gen, string f_Estm_rest,
            int Nic, string num_Camp, string tipo_Suspension, string coment_Os, string coment_Os2,
            DatosumDTO Datosum, IList<ActividadesDto> Actividades, IList<PrecintosDto> Precintos, IList<RecibosDto> Recibos,
            IList<ApacoenDto> Apaconen)
        {
            NUM_OS = num_Os;
            NUM_SOLICITUD = num_Solicitud;
            TIPO_TRABAJO = tipo_Trabajo;
            TIP_OS = tip_Os;
            F_GEN = f_Gen;
            F_ESTM_REST = f_Estm_rest;
            NIC = Nic;
            NUM_CAMP = num_Camp;
            TIPO_SUSPENSION = tipo_Suspension;
            COMENT_OS = coment_Os;
            COMENT_OS2 = coment_Os2;
            //f_create = f_Create;
            DATOSUM = Datosum;
            ACTIVIDADES = Actividades;
            PRECINTOS = Precintos;
            RECIBOS = Recibos;
            APACOEN = Apaconen;
        }
    }
}
