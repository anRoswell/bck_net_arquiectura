using Core.DTOs.CuestionarioInstanciasMovilDto;
using Core.DTOs.FilesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.GDMovilDto
{
    public class Op360_SoportesMovilGDDto
    {
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
        public Op360_SoportesMovilGDDto(Op360_SoporteDTO requestDto, FileResponse fileResponse, string uriString)
        {
            codigo_tipo_soporte = requestDto.codigo_tipo_soporte;
            formato = fileResponse.Extension;
            nombre = fileResponse.NombreInterno;
            peso = fileResponse.Size;
            url = uriString;
        }
    }
}
