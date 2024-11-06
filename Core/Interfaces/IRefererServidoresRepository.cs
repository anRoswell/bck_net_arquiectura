using Core.Entities;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    /// <summary>
    /// Interfaz para Heredar el CRUD y adicionar otros métodos
    /// </summary>
    public interface IRefererServidoresRepository : IRepository<RefererServidore>
    {
        Task<bool> GetPermisoAccesoPorRefererServidores(string Referer, string Grupo);        
    }
}
