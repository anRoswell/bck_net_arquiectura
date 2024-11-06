using Core.Entities;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    /// <summary>
    /// Interfaz para Heredar el CRUD y adicionar otros métodos
    /// </summary>
    public interface IRefererServidoresRepositoryOracle
    {
        Task<bool> GetPermisoAccesoPorRefererServidoresCore(string Referer, string Grupo);
    }
}
