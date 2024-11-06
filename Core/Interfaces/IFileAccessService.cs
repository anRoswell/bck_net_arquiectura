namespace Core.Interfaces
{
    using Core.DTOs.FilesDto;
    using Core.Entities;
    using Core.Enumerations;
    using System.Threading.Tasks;

    public interface IFileAccessService
	{
        /// <summary>
        /// Obtiene la ruta de destino para el archivo.
        /// </summary>
        /// <param name="modulo">Modulo al que pertenece el archivo.</param>
        /// <returns>Registro con la ruta.</returns>
        Task<gnl_rutas_archivo_servidor> GetRoot(Enum_RutasArchivos modulo);
                
        /// <summary>
        /// Guarda el archivo de soporte en bd con su ruta especifica.
        /// </summary>
        /// <param name="fileResponse">Parametro con el archivo fisico.</param>
        /// <param name="userId">Usuario que guarda el archivo en la bd.</param>
        /// <returns></returns>
        Task<gnl_soportes> SaveFileSoportes(FileResponse fileResponse, int userId);

        /// <summary>
        /// Guarda el archivo de soporte en bd con su ruta especifica.
        /// </summary>
        /// <param name="fileResponse">Parametro con el archivo fisico.</param>
        /// <param name="userId">Usuario que guarda el archivo en la bd.</param>
        /// <returns></returns>
        Task<gnl_soportes> SaveFileScrSoportes(FileResponse fileResponse, int userId, TipoSoporteEnum tipoSoporte);

        /// <summary>
        /// Guarda el archivo de soporte en bd con su ruta especifica.
        /// </summary>
        /// <param name="fileResponse">Parametro con el archivo fisico.</param>
        /// <param name="userId">Usuario que guarda el archivo en la bd.</param>
        /// <returns></returns>
        Task<gos_soporte> SaveFileGosSoportes(FileResponse fileResponse, int userId, TipoSoporteEnum tipoSoporte);
    }
}

