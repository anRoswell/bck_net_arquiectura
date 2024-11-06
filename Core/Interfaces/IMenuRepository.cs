using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface  IMenuRepository
    {
        Task<List<Menu>> GetMenus(int TipoUsuario);
    }
}
