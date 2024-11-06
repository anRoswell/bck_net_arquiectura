using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ASEGREDMovilDto
{
    public class Op360ASEGRED_GestionOrdenDto
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
    }
}
