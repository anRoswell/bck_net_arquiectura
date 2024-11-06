
namespace Infrastructure.Repositories
{
    using Core.Entities;
    using Core.Exceptions;
    using Core.Interfaces;
    using Infrastructure.Data;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ClaseContratoRepository : BaseRepository<ClaseContrato>, IClaseContratoRepository
    {
        public ClaseContratoRepository(DbModelContext context) : base(context) { }

        public async Task<List<ClaseContrato>> SearchAll()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","7")
                };

                string sql = $"[cont].[SpContrato] @Operacion = @Operacion";

                var response = await _context.ClaseContratos.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

    }
}
