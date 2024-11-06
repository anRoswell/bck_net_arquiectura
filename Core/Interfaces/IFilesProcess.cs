using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DTOs;
using Core.DTOs.FilesDto;
using Core.Entities;
using Core.ModelProcess;
using Core.Options;
using Core.QueryFilters;

namespace Core.Interfaces
{
    public interface IFilesProcess
    {        
        Task<ResponseDto<List<FileResponseOracle>>> SaveFilesOracle(QueryOp360Files parameters);      
        
        Task<ResponseDto<FileResponseOracle>> SaveFileOracleBase64_Scr(QueryOp360FileBase64 parameters);        

        Task<FileResponseOracle> SaveFileOracleBase64_Gos(Op360FileBase64 data);
        
        Task<ResponseDto<FileResponseOracleBase64>> GetFileBase64(QueryOp360GetFiles parameters);
        
        Task<ResponseDto<FileResponsePlantillasOracleBase64Dto>> GetFilePlantillasBase64(QueryOp360ObtenerGnlPlantilla codigo);

        Task<ResponseDto<FileResponseOracleBase64>> GetFilePlantillasCMBase64(QueryOp360ObtenerGnlPlantilla codigo);

        Task<ResponseDto<FileResponseOracleBase64>> GetFileActaOrdenSCRBase64(FileResponsePdfDto ms);

        Task<ResponseDto<FileResponseReportesJasper>> GetFilePlantillas(QueryOp360ObtenerGnlPlantilla codigo);
        
        Task<ResponseDto<FileResponseReportesJasper>> GetFileGnlSoporte(QueryOp360ObtenerGnlPlantilla codigo);

        Task<FileResponseBytes> GetFileBytes(QueryOp360GetFiles parameters);

        Task<FileResponseBytes> ObtenerLogo();
    }
}