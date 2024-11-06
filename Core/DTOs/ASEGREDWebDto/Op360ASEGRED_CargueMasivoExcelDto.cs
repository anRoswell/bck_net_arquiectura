using Core.DTOs.ASEGREDWebDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ASEGREDWebDto
{
    public class Op360ASEGRED_CargueMasivoExcelDto
    {
        public int item { get; set; }
        public int codigo_uucc { get; set; }
        public string descripcion { get; set; }
        public int cantidad_total { get; set; }
        public int? pp001 { get; set; }
        public int? pp002 { get; set; }
        public int? pp003 { get; set; }
        public int? pp004 { get; set; }
    }
}
