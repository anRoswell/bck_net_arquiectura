using Core.DTOs.FilesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.GDMovilDto
{
    public class Op360_SoportesMovil2GDDto
    {
        public int id_regla_oro { get; set; }
        public string nombre { get; set; }
        public double peso { get; set; }
        public string formato { get; set; }
        public string url { get; set; }
        public string codigo_tipo_soporte { get; set; }

        /// <summary>
        /// Mapeo del dto.
        /// </summary>
        /// <param name="requestDto">Request.</param>
        /// <param name="fileResponse">Respuesta del archivo.</param>
        /// <param name="uriString">Ruta final del archivo.</param>
        /// 
        public Op360_SoportesMovil2GDDto(Op360_Reglas_De_OroDto requestDto, FileResponse fileResponse, string uriString)
        {
            id_regla_oro = requestDto.id_regla_oro;
            codigo_tipo_soporte = requestDto.codigo_tipo_soporte;
            formato = fileResponse.Extension;
            nombre = fileResponse.NombreInterno;
            peso = fileResponse.Size;
            url = uriString;
        }
    }
}
