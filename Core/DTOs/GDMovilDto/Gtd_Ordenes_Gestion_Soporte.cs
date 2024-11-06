using Core.Entities;
using Core.Enumerations;
using ImageMagick;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCoder.PayloadGenerator;

namespace Core.DTOs.GDMovilDto
{
    public class gtd_ordenes_gestion_soporte : BaseEntityOracle
    {
        public int id_regla_oro { get; set; }
        public int id_soporte_tipo { get; set; }
        //public Enum_RutasArchivos? Id_Ruta { get; set; }
        public string nombre { get; set; }
        public int peso { get; set; }
        public string formato { get; set; }
        public string ind_url_externo { get; set; }
        public string url { get; set; }
        public string bfile { get; set; }
        public int id_usuario_registra { get; set; }
        public Date fecha_registra { get; set; }
    }
}
