using Core.Entities;
using Core.Exceptions;
using Core.ModelResponse;
using Core.QueryFilters;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public class TokenService : ITokenService
    {
        DbModelContext _context;

        public TokenService(DbModelContext context)
        {
            _context = context;
        }

        public async Task<List<ResponseActionUrl>> GenerateToken()
        {
            try
            {
                SqlParameter[] paramts = new[] {
                    new SqlParameter("@Operacion","1")
                };

                string sql = $"[conf].[SpUtilities] @Operacion = @Operacion";

                var response = await _context.ResponseActionUrls.FromSqlRaw(sql, parameters: paramts).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> UpdateToken(string html, int id)
        {
            try
            {
                SqlParameter[] paramts = new[] {
                    new SqlParameter("@Operacion", "2"),
                    new SqlParameter("@IdHashCertifiedValidation", id),
                    new SqlParameter("@CertifiedHtml", html)
                };

                string sql = $"[conf].[SpUtilities] @Operacion = @Operacion, @IdHashCertifiedValidation = @IdHashCertifiedValidation, @CertifiedHtml = @CertifiedHtml";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: paramts).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<HashCertifiedValidation>> ValidateTokenCertificado(QueryToken query)
        {
            try
            {
                SqlParameter[] paramts = new[] {
                    new SqlParameter("@Operacion", "3"),
                    new SqlParameter("@CodigoHash", query.Token)
                };

                string sql = $"[conf].[SpUtilities] @Operacion = @Operacion, @CodigoHash = @CodigoHash";

                var response = await _context.HashCertifiedValidations.FromSqlRaw(sql, parameters: paramts).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
    }
}
