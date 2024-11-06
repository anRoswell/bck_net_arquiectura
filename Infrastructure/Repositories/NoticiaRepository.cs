
namespace Infrastructure.Repositories
{
    using Core.CustomEntities;
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

    public class NoticiaRepository : BaseRepository<Noticia>, INoticiaRepository
    {
        public NoticiaRepository(DbModelContext context) : base(context) { }

        public async Task<List<Noticia>> GetNoticias(string Operacion, string CodUser)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion", Operacion),
                    new SqlParameter("@CodUser", CodUser),
                };

                string sql = $"[noti].[SpNoticias] @Operacion = @Operacion, @CodUser = @CodUser";

                List<Noticia> response = await _context.Noticias.FromSqlRaw(sql, parameters: parameters).ToListAsync();

                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e}");
            }
        }

        public async Task<List<Empresa>> GetEmpresas()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","3")
                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion";

                var response = await _context.Empresas.FromSqlRaw(sql, parameters: parameters).ToListAsync();

                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.ToString()}");
            }
        }

        public async Task<List<ResponseAction>> PostNoticia(Noticia noticia, List<NoticiasDoc> listDocuments)
        {
            try
            {
                StringBuilder NoticiasDoc = new StringBuilder();

                foreach (NoticiasDoc item in listDocuments)
                {
                    NoticiasDoc.Append("INSERT INTO #noticias_doc (notdNameDocument, notdExtDocument, notdSizeDocument, notdUrlDocument, notdUrlRelDocument, notdOriginalNameDocument) ");
                    NoticiasDoc.Append($"VALUES('{item.NotdNameDocument}','{item.NotdExtDocument}',{item.NotdSizeDocument}, '{item.NotdUrlDocument}','{item.NotdUrlRelDocument}','{item.NotdOriginalNameDocument}') ");
                }

                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","6"),
                    new SqlParameter("@Id", noticia.Id),
                    new SqlParameter("@CodEmpresa", noticia.CodEmpresa),
                    new SqlParameter("@CodTipoNoticia", noticia.CodTipoNoticia),
                    new SqlParameter("@CodAlcance", noticia.CodAlcance),
                    new SqlParameter("@notTitle", noticia.NotTitle),
                    new SqlParameter("@notDescripcion", noticia.NotDescripcion),
                    new SqlParameter("@notEstado", noticia.NotEstado),
                    new SqlParameter("@CodUser", noticia.CodUser),
                    new SqlParameter("@InsertNoticiasDoc", NoticiasDoc.ToString())
                };

                string sql = $"[noti].[SpNoticias] @Operacion = @Operacion, @Id = @Id, @CodEmpresa = @CodEmpresa, @CodTipoNoticia = @CodTipoNoticia, @CodAlcance = @CodAlcance, @notTitle = @notTitle, " +
                    $"@notDescripcion = @notDescripcion, @notEstado = @notEstado, @CodUser = @CodUser, @InsertNoticiasDoc = @InsertNoticiasDoc";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PutNoticia(Noticia noticia, List<NoticiasDoc> listDocuments)
        {
            try
            {
                SqlParameter[] parameters;
                string sql = string.Empty;

                if (listDocuments.Count > 0)
                {
                    StringBuilder NoticiasDoc = new StringBuilder();

                    foreach (NoticiasDoc item in listDocuments)
                    {
                        NoticiasDoc.Append("INSERT INTO #noticias_doc_update (notdNameDocument, notdExtDocument, notdSizeDocument, notdUrlDocument, notdUrlRelDocument, notdOriginalNameDocument) ");
                        NoticiasDoc.Append($"VALUES('{item.NotdNameDocument}','{item.NotdExtDocument}',{item.NotdSizeDocument}, '{item.NotdUrlDocument}','{item.NotdUrlRelDocument}','{item.NotdOriginalNameDocument}') ");
                    }

                    parameters = new[] {
                        new SqlParameter("@Operacion","2"),
                        new SqlParameter("@notTitle", noticia.NotTitle),
                        new SqlParameter("@CodTipoNoticia", noticia.CodTipoNoticia),
                        new SqlParameter("@notDescripcion", noticia.NotDescripcion),
                        new SqlParameter("@notEstado", noticia.NotEstado),
                        new SqlParameter("@Id", noticia.Id),
                        new SqlParameter("@CodAlcance", noticia.CodAlcance),
                        new SqlParameter("@CodEmpresa", noticia.CodEmpresa),
                        new SqlParameter("@CodUserUpdate", noticia.CodUserUpdate),
                        new SqlParameter("@InsertNoticiasDoc", NoticiasDoc.ToString())
                    };

                    sql = $"[noti].[SpNoticias] @Operacion = @Operacion, @notTitle = @notTitle, @CodTipoNoticia = @CodTipoNoticia, @notDescripcion = @notDescripcion, @notEstado = @notEstado, @Id = @Id, " +
                        $"@CodAlcance = @CodAlcance, @CodEmpresa = @CodEmpresa, @CodUserUpdate = @CodUserUpdate, @InsertNoticiasDoc = @InsertNoticiasDoc";
                }
                else
                {
                    parameters = new[] {
                        new SqlParameter("@Operacion","2"),
                        new SqlParameter("@notTitle", noticia.NotTitle),
                        new SqlParameter("@CodTipoNoticia", noticia.CodTipoNoticia),
                        new SqlParameter("@notDescripcion", noticia.NotDescripcion),
                        new SqlParameter("@notEstado", noticia.NotEstado),
                        new SqlParameter("@Id", noticia.Id),
                        new SqlParameter("@CodAlcance", noticia.CodAlcance),
                        new SqlParameter("@CodEmpresa", noticia.CodEmpresa),
                        new SqlParameter("@CodUserUpdate", noticia.CodUserUpdate)
                    };

                    sql = $"[noti].[SpNoticias] @Operacion = @Operacion, @notTitle = @notTitle, @CodTipoNoticia = @CodTipoNoticia, @notDescripcion = @notDescripcion, @notEstado = @notEstado, @Id = @Id, " +
                        $"@CodAlcance = @CodAlcance, @CodEmpresa = @CodEmpresa, @CodUserUpdate = @CodUserUpdate";
                }

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> DeleteNoticia(Noticia noticia)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","3"),
                    new SqlParameter("@Id", noticia.Id),
                };

                string sql = $"[noti].[SpNoticias] @Operacion = @Operacion, @Id = @Id";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<TiposPlantilla>> GetTiposPlantillas()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","4")
                };

                string sql = $"[noti].[SpNoticias] @Operacion = @Operacion";

                List<TiposPlantilla> response = await _context.TiposPlantillas.FromSqlRaw(sql, parameters: parameters).ToListAsync();

                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.ToString()}");
            }
        }

        public async Task<List<TipoNoticia>> GetTipoNoticia()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","5")
                };

                string sql = $"[noti].[SpNoticias] @Operacion = @Operacion";

                List<TipoNoticia> response = await _context.TipoNoticias.FromSqlRaw(sql, parameters: parameters).ToListAsync();

                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.ToString()}");
            }
        }

        public async Task<List<Alcance>> GetAlcance()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","7")
                };

                string sql = $"[noti].[SpNoticias] @Operacion = @Operacion";

                List<Alcance> response = await _context.Alcances.FromSqlRaw(sql, parameters: parameters).ToListAsync();

                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.ToString()}");
            }
        }

        public async Task<List<Noticia>> GetNoticiaPorID(int id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","8"), //Cambiar
                    new SqlParameter("@Id", id)
                };

                string sql = $"[noti].[SpNoticias] @Operacion = @Operacion, @Id = @Id";

                var response = await _context.Noticias.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public Task<NoticiaDetalle> GetNoticiaDetallePorID(int id)
        {
            throw new NotImplementedException();
        }
    }
}
