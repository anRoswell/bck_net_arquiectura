using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PrvSocioRepository : BaseRepository<PrvSocio>, IPrvSocioRepository
    {
        public PrvSocioRepository(DbModelContext context) : base(context) { }

        public async Task<List<PrvSocio>> GetSocio(int id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1"),
                    new SqlParameter("@CodProveedor",id)
                };

                string sql = $"[prv].[SpPrvSocios] @Operacion = @Operacion, @CodProveedor=@CodProveedor";

                var response = await _context.PrvSocios.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
    }
}
