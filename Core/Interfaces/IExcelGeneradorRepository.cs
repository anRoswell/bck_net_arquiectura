namespace Core.Interfaces
{
    using Core.DTOs;
    using Core.DTOs.FilesDto;
    using Core.QueryFilters;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Threading.Tasks;

    public interface IExcelGeneradorRepository<T> where T : class
    {
        /// <summary>
        /// Genera archivo de excel y lo retorna en un objeto de tipo MemoryStream
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task<ResponseDto<(FileResponseOracleBase64, byte[])>> ExecuteExcelGenerador(IQueryable dataQuery, string format, QueryOp360ConfigExcel ConfigExcel);
    }
}

