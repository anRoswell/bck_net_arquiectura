using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelResponse;
using Core.QueryFilters;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class NoticiasDocRepository : BaseRepository<NoticiasDoc>, INoticiasDocRepository
    {
        public NoticiasDocRepository(DbModelContext context) : base (context){ }

        public async Task<List<NoticiasDoc>> GetDocumentosNoticia(int idNoticia)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1"),
                    new SqlParameter("@notdCodNoticias", idNoticia)
                };

                string sql = $"[noti].[SpNoticiasDoc] @Operacion = @Operacion, @notdCodNoticias = @notdCodNoticias";

                var response = await _context.NoticiasDocs.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseActionUrl>> AgregarImagenNoticia(NoticiasDoc entity)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","2"),
                    new SqlParameter("@notdCodNoticias", entity.NotdCodNoticias),
                    new SqlParameter("@notdNameDocument", entity.NotdNameDocument),
                    new SqlParameter("@notdExtDocument", entity.NotdExtDocument),
                    new SqlParameter("@notdSizeDocument", entity.NotdSizeDocument),
                    new SqlParameter("@notdUrlDocument", entity.NotdUrlDocument),
                    new SqlParameter("@notdUrlRelDocument", entity.NotdUrlRelDocument),
                    new SqlParameter("@notdOriginalNameDocument", entity.NotdOriginalNameDocument),
                    new SqlParameter("@CodUser", entity.CodUser)
                };

                string sql = $"[noti].[SpNoticiasDoc] @Operacion = @Operacion, @notdCodNoticias = @notdCodNoticias, @notdNameDocument = @notdNameDocument, @notdExtDocument = @notdExtDocument, @notdSizeDocument = @notdSizeDocument, " +
                    $"@notdUrlDocument = @notdUrlDocument, @notdUrlRelDocument = @notdUrlRelDocument, @notdOriginalNameDocument = @notdOriginalNameDocument, @CodUser = @CodUser";

                var response = await _context.ResponseActionUrls.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> DeleteImagenNoticia(QueryDeleteImagenNoticia query)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","3"),
                    new SqlParameter("@notdIdNoticiasDoc", query.CodNoticasDoc),
                    new SqlParameter("@CodUserUpdate", query.CodUserUpdate)
                };

                string sql = $"[noti].[SpNoticiasDoc] @Operacion = @Operacion, @notdIdNoticiasDoc = @notdIdNoticiasDoc, @CodUserUpdate = @CodUserUpdate";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
    }
}
