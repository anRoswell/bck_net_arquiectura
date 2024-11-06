using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ASEGREDWebDto
{
    public class Op360ASEGRED_ListadoUnidadesConstructivasCerradasResponseDto
    {
        public int item { get; set; }
        public string estructura { get; set; }
        public int uc { get; set; }
        public string descripcion_uc { get; set; }
        public decimal valor_material_aire { get; set; }
        public decimal valor_material_contrata { get; set; }
        public decimal valor_mano_obra { get; set; }
        public decimal valor_total { get; set; }
        public int cantidad_instalada { get; set; }
        public string estado { get; set; }
    }
}
