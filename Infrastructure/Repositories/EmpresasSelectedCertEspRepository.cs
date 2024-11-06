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
    public class EmpresasSelectedCertEspRepository : BaseRepository<EmpresasSelectedCertEsp>, IEmpresasSelectedCertEspRepository
    {
        public EmpresasSelectedCertEspRepository(DbModelContext context) : base(context) { }

        public async Task<List<EmpresasSelectedCertEsp>> GetEmpresasSelectedCertificado(int id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1"),
                    new SqlParameter("@escCodCertificadosEspeciales",id)
                };

                string sql = $"[cer].[SpEmpresasSelectedCertEsp] @Operacion = @Operacion, @escCodCertificadosEspeciales = @escCodCertificadosEspeciales";

                var response = await _context.EmpresasSelectedCertEsps.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
    }
}
