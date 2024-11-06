using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelResponse;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReqQuestionAnswerRepository : BaseRepository<ReqQuestionAnswer>, IReqQuestionAnswerRepository
    {
        public ReqQuestionAnswerRepository(DbModelContext context) : base(context) { }

        public async Task<List<ReqQuestionAnswer>> GetMensajes(int id, int idRequerimientos, int esProveedor)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1"),
                    new SqlParameter("@id",id),
                    new SqlParameter("@rqaCodRequerimiento",idRequerimientos),
                    new SqlParameter("@esProveedor", esProveedor),
                };

                string sql = $"[req].[SpGetReqQuestionAnswer] @Operacion=@Operacion,@id = @id, @rqaCodRequerimiento = @rqaCodRequerimiento, @esProveedor = @esProveedor";

                var response = await _context.ReqQuestionAnswers.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PostCrear(ReqQuestionAnswer reqQuestionAnswer)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","2"),
                    new SqlParameter("@rqaCodRequerimiento",reqQuestionAnswer.RqaCodRequerimiento),
                    new SqlParameter("@rqaCodUsuario",reqQuestionAnswer.RqaCodUsuario),
                    new SqlParameter("@rqaComentario",reqQuestionAnswer.RqaComentario),
                    new SqlParameter("@rqahasUploadFile",reqQuestionAnswer.RqahasUploadFile),
                    new SqlParameter("@rqaisPrivate",reqQuestionAnswer.RqahasUploadFile),
                    new SqlParameter("@rqaEstado",reqQuestionAnswer.RqaEstado),
                    new SqlParameter("@CodUser",reqQuestionAnswer.CodUser)
                    
                };

                string sql = $"[req].[SpGetReqQuestionAnswer] @Operacion=@Operacion, @rqaCodRequerimiento=@rqaCodRequerimiento, @rqaCodProveedor=@rqaCodProveedor, @rqaCodUsuario=@rqaCodUsuario, @rqaComentario=@rqaComentario, @rqahasUploadFile=@rqahasUploadFile, @rqaisPrivate=@rqaisPrivate, @rqaEstado=@rqaEstado, @CodUser=@CodUser";

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
