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
    public class SinoRepository : BaseRepository<Sino>, ISinoRepository
    {
        public SinoRepository(DbModelContext context) : base(context) { }

        public async Task<List<Sino>> SearchAll()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","9")
                };

                string sql = $"[cont].[SpContrato] @Operacion = @Operacion";

                var response = await _context.Sinos.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
    }
}
