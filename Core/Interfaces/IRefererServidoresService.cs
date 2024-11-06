using Core.Entities;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    /// <summary>
    /// Interfaz para definir los métodos que debe tener el Servicio
    /// </summary>
    public interface IRefererServidoresService
    {
        Task<bool> GetPermisoAccesoPorRefererServidores(string Referer, string Grupo);
        Task<bool> GetPermisoAccesoPorRefererServidoresOracle(string Referer, string Grupo);
    }
}
