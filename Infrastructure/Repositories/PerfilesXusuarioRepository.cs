using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelResponse;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PerfilesXusuarioRepository : BaseRepository<PerfilesXusuario>, IPerfilesXusuarioRepository
    {
        public PerfilesXusuarioRepository(DbModelContext context) : base(context) { }

        public async Task<List<PerfilesXusuarioView>> GetListado()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1")
                };

                string sql = $"[usr].[SpPerfilesXusuario] @Operacion = @Operacion";

                var response = await _context.PerfilesXusuariosView.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<PerfilesXusuario>> GetPorId(int id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","2"),
                    new SqlParameter("@IdPerfilXusuario", id)
                };

                string sql = $"[usr].[SpPerfilesXusuario] @Operacion = @Operacion, @IdPerfilXusuario = @IdPerfilXusuario";

                var response = await _context.PerfilesXusuarios.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<PerfilesXusuario>> GetPorIdUsuario(int idUsuario)
        {
            try
            {
                var response = await _context.PerfilesXusuarios.Where((e)=>e.PxuUsrCodUsuario == idUsuario).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PostCrear(PerfilesXusuario perfilesXusuario)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","3"),
                    new SqlParameter("@CodUsuario", perfilesXusuario.PxuUsrCodUsuario),
                    new SqlParameter("@CodAplicacion", perfilesXusuario.PxuAplCodAplicacion),
                    new SqlParameter("@CodPerfil", perfilesXusuario.PxuPrfCodPerfil),
                    new SqlParameter("@Estado", perfilesXusuario.PxuEstado),
                    new SqlParameter("@CodArchivo", perfilesXusuario.CodArchivo is null ? "0" : perfilesXusuario.CodArchivo),
                    new SqlParameter("@CodUser", perfilesXusuario.CodUser)
                };

                string sql = $"[usr].[SpPerfilesXusuario] @Operacion = @Operacion, @CodUsuario = @CodUsuario, @CodAplicacion = @CodAplicacion, @CodPerfil = @CodPerfil, @Estado = @Estado, " +
                    $"@CodArchivo = @CodArchivo, @CodUser = @CodUser";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PutActualizar(PerfilesXusuario perfilesXusuario)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","4"),
                    new SqlParameter("@IdPerfilXusuario", perfilesXusuario.Id),
                    new SqlParameter("@CodUsuario", perfilesXusuario.PxuUsrCodUsuario),
                    new SqlParameter("@CodAplicacion", perfilesXusuario.PxuAplCodAplicacion),
                    new SqlParameter("@CodPerfil", perfilesXusuario.PxuPrfCodPerfil),
                    new SqlParameter("@Estado", perfilesXusuario.PxuEstado),
                    new SqlParameter("@CodArchivo", perfilesXusuario.CodArchivo is null ? "0" : perfilesXusuario.CodArchivo),
                    new SqlParameter("@CodUserUpdate", perfilesXusuario.CodUserUpdate ?? ""),
                };

                string sql = $"[usr].[SpPerfilesXusuario] @Operacion = @Operacion, @IdPerfilXusuario = @IdPerfilXusuario, @CodUsuario = @CodUsuario, @CodAplicacion = @CodAplicacion, " +
                    $"@CodPerfil = @CodPerfil, @Estado = @Estado, @CodArchivo = @CodArchivo, @CodUserUpdate = @CodUserUpdate";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> DeleteRegistro(PerfilesXusuario perfilesXusuario)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","5"),
                    new SqlParameter("@IdPerfilXusuario", perfilesXusuario.Id),
                    new SqlParameter("@CodUserUpdate", perfilesXusuario.CodUserUpdate ?? "")
                };

                string sql = $"[usr].[SpPerfilesXusuario] @Operacion = @Operacion, @IdPerfilXusuario = @IdPerfilXusuario, @CodUserUpdate = @CodUserUpdate";

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
