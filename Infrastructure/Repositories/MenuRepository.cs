using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        public MenuRepository(DbModelContext context) : base(context) { }
        public async Task<List<Menu>> GetMenus(int TipoUsuario)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1"),
                      new SqlParameter("@Men_Tusr_CodTipoUsuario",TipoUsuario)
                };

                string sql = $"[usr].[SpGetMenu] @Operacion = @Operacion,@Men_Tusr_CodTipoUsuario=@Men_Tusr_CodTipoUsuario";

                var response = await _context.Menus.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
    }
}
