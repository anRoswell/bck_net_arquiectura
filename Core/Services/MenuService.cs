using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MenuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

      

        public async Task<List<Menu>> GetMenus(int TipoUsuario)
        {
            return await _unitOfWork.MenusRepository.GetMenus(TipoUsuario);
        }
    }
}
