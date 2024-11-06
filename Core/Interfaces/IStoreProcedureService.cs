namespace Core.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.DTOs;
    using Core.DTOs.FilesDto;
    using Core.Entities;
    using Core.QueryFilters;
    using ImageMagick;
    using static QRCoder.PayloadGenerator.ShadowSocksConfig;

    public interface IStoreProcedureService
	{
        Task<ResponseDto<DatosDto>> Get();
        Task<ResponseDto<Aire_Scr_Ord_OrdenDto>> UpdateOrden(Aire_Scr_Ord_OrdenDto aire_ord_ordenDto);
        Task<ResponseDto<Datos_Dummi>> Obtener_Datos_Dummi(string StoreProcedure, QueryOp360Dummi parameters);        
    }
}