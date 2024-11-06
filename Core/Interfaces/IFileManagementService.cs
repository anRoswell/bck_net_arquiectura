namespace Core.Interfaces
{
    using Core.DTOs.FilesDto;
    using System.Threading.Tasks;

    public interface IFileManagementService
	{
        /// <summary>
        /// Convierte el base64 a un archivo y lo copia en la ruta asignada.
        /// </summary>
        /// <param name="fileByBase64Dto">Parametros de entrada.</param>
        /// <returns>Datos del archivo.</returns>
        Task<FileResponse> CreateFileByBase64(FileByBase64Dto fileByBase64Dto);

        /// <summary>
        /// Convierte el base64 a un archivo y lo copia en la ruta asignada.
        /// </summary>
        /// <param name="fileByBase64Dto">Parametros de entrada.</param>
        /// <returns>Datos del archivo.</returns>
        Task<FileResponse> CreateFileByBase64(NewFileByBase64Dto fileByBase64Dto);

        /// <summary>
        /// Convierte el archivo de la ruta en un base64
        /// </summary>
        /// <param name="url">Url que aloja el archivo.</param>
        /// <returns>base64</returns>
        Task<string> CreateBase64ByUrl(string url);
    }
}

