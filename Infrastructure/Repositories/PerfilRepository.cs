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
using System.Data;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PerfilRepository : BaseRepository<Perfil>, IPerfilRepository
    {
        public PerfilRepository(DbModelContext context) : base(context) { }

        public async Task<List<Perfil>> GetPerfiles()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1")
                };

                string sql = $"[usr].[SpPerfiles] @Operacion = @Operacion";

                var response = await _context.Perfiles.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

       

        public async Task<List<Perfil>> GetPerfilesSSR(QuerySearchRequerimientosSSR data)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","6"),
                    new SqlParameter("@first", data.First),                   
                    new SqlParameter("@ord", data.SortField??"prf_idperfil"),
                    new SqlParameter("@SortOrder",data.SortOrder),
                     new SqlParameter("@Total", data.Rows),
                    
                };

                string sql = $"[usr].[SpPerfiles] @Operacion = @Operacion, @first=@first, @ord=@ord,@SortOrder=@SortOrder, @Total=@Total";

                var response = await _context.Perfiles.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }



        public async Task<List<Perfil>> Getperfil(int id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","2"),
                    new SqlParameter("@IdPerfil", id)
                };

                string sql = $"[usr].[SpPerfiles] @Operacion = @Operacion, @IdPerfil = @IdPerfil";

                var response = await _context.Perfiles.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PostCrear(Perfil perfil)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","3"),
                    new SqlParameter("@NombrePerfil", perfil.PrfNombrePerfil),
                    new SqlParameter("@Administrador", perfil.PrfAdministrador),
                    new SqlParameter("@Estado", perfil.PrfEstado),
                    new SqlParameter("@CodArchivo", perfil.CodArchivo is null ? "0" : perfil.CodArchivo),
                    new SqlParameter("@CodUser", perfil.CodUser)
                };

                string sql = $"[usr].[SpPerfiles] @Operacion = @Operacion, @NombrePerfil = @NombrePerfil, @Administrador = @Administrador, @Estado = @Estado, @CodArchivo = @CodArchivo, @CodUser = @CodUser";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PutActualizar(Perfil perfil)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","4"),
                    new SqlParameter("@IdPerfil", perfil.Id),
                    new SqlParameter("@NombrePerfil", perfil.PrfNombrePerfil),
                    new SqlParameter("@Administrador", perfil.PrfAdministrador),
                    new SqlParameter("@Estado", perfil.PrfEstado),
                    new SqlParameter("@CodArchivo", perfil.CodArchivo is null ? "0" : perfil.CodArchivo),
                    new SqlParameter("@CodUserUpdate", perfil.CodUserUpdate ?? "")
                };

                string sql = $"[usr].[SpPerfiles] @Operacion = @Operacion, @IdPerfil = @IdPerfil, @NombrePerfil = @NombrePerfil, @Administrador = @Administrador, @Estado = @Estado, @CodArchivo = @CodArchivo, @CodUserUpdate = @CodUserUpdate";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> DeletePerfil(Perfil perfil)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","5"),
                    new SqlParameter("@IdPerfil", perfil.Id),
                    new SqlParameter("@CodUserUpdate", perfil.CodUserUpdate ?? "")
                };

                string sql = $"[usr].[SpPerfiles] @Operacion = @Operacion, @IdPerfil = @IdPerfil, @CodUserUpdate = @CodUserUpdate";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<int> GetTotalPerfilesSSR()
        {
            try
            {
                SqlParameter[] @params =
                {
                new SqlParameter("@returnVal", SqlDbType.Int) {Direction = ParameterDirection.Output}
                //new SqlParameter("@returnVal2", SqlDbType.Int) {Direction = ParameterDirection.ReturnValue}
                };

                await _context.Database.ExecuteSqlRawAsync($"exec [usr].[SpPerfiles] @Operacion = '7', @contPerfiles = @returnVal output", @params);
                var result = (int)@params[0].Value;
                return result;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
    }
}
