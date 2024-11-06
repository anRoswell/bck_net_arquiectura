using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IMenuService
    {
        Task<List<Menu>> GetMenus(int TipoUsuario);
    }
}
