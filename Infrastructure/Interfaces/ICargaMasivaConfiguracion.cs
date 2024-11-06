namespace Infrastructure.Interfaces
{
    using System.Threading.Tasks;
    using Core.DTOs;
    using Core.DTOs.CargaMasiva;
    using Core.Enumerations;

    public interface ICargaMasivaConfiguracion
	{
        /// <summary>
        /// Carga inicial.
        /// </summary>
        CargaInicial CargaInicial { get; }

        /// <summary>
        /// Procesa el excel.
        /// </summary>
        /// <param name="fileBase64">Excel en base 64.</param>
        /// <param name="myUser">Usuario.</param>
        /// <returns>Tiempo de procesamiento.</returns>
        Task<ResponseDto<MedirTiempo>> Procesar(object parameter, int myUser);
    }
}