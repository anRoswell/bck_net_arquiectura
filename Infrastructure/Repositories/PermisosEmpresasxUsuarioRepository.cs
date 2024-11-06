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
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PermisosEmpresasxUsuarioRepository : BaseRepository<PermisosEmpresasxUsuario>, IPermisosEmpresasxUsuarioRepository
    {
        public PermisosEmpresasxUsuarioRepository(DbModelContext context) : base(context) { }

        public async Task<List<PermisosEmpresasxUsuario>> GetListado()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1")
                };

                string sql = $"[usr].[SpPermisoEmpresasXusuarios] @Operacion = @Operacion";

                var response = await _context.PermisosEmpresasxUsuarios.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<PermisosEmpresasxUsuario>> GetPorId(int id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","2"),
                    new SqlParameter("@IdPermiso", id)
                };

                string sql = $"[usr].[SpPermisoEmpresasXusuarios] @Operacion = @Operacion, @IdPermiso = @IdPermiso";

                var response = await _context.PermisosEmpresasxUsuarios.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PostCrear(QueryCreatePermisoEmpresa permisosEmpresasxUsuario)
        {
            try
            {
                StringBuilder InsertEmp = new StringBuilder();

                foreach (int item in permisosEmpresasxUsuario.PeuEmpCodEmpresa)
                {
                    InsertEmp.Append("INSERT INTO #empresas (CodEmpresa) ");
                    InsertEmp.Append($"VALUES({item}) ");
                }

                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","3"),
                    new SqlParameter("@CodUsuario", permisosEmpresasxUsuario.PeuUsrCodUsuario),
                    new SqlParameter("@cadena",InsertEmp.ToString()),
                    new SqlParameter("@CodArchivo", ""),
                     new SqlParameter("@Estado", 1),
                    new SqlParameter("@CodUser", permisosEmpresasxUsuario.CodUser)
                };

                string sql = $"[usr].[SpPermisoEmpresasXusuarios] @Operacion = @Operacion, @CodUsuario = @CodUsuario, @cadena = @cadena, @Estado = @Estado, " +
                    $" @CodUser = @CodUser, @CodArchivo=@CodArchivo";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PutActualizar(QueryCreatePermisoEmpresa permisosEmpresasxUsuario)
        {
            try
            {
                StringBuilder InsertEmp = new StringBuilder();

                foreach (int item in permisosEmpresasxUsuario.PeuEmpCodEmpresa)
                {
                    InsertEmp.Append("INSERT INTO #empresasedit (CodEmpresa) ");                          
                    InsertEmp.Append($"VALUES({item}) ");
                }

                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","4"),
                    new SqlParameter("@CodUsuario", permisosEmpresasxUsuario.PeuUsrCodUsuario),
                    new SqlParameter("@cadena",InsertEmp.ToString()),
                    new SqlParameter("@CodArchivo", ""),
                     new SqlParameter("@Estado", 1),
                    new SqlParameter("@CodUser", permisosEmpresasxUsuario.CodUser)
                };

                string sql = $"[usr].[SpPermisoEmpresasXusuarios] @Operacion = @Operacion, @CodUsuario = @CodUsuario, @cadena = @cadena, @Estado = @Estado, " +
                    $" @CodUser = @CodUser, @CodArchivo=@CodArchivo";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> DeleteRegistro(PermisosEmpresasxUsuario permisosEmpresasxUsuario)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","5"),
                    new SqlParameter("@IdPermiso", permisosEmpresasxUsuario.Id),
                    new SqlParameter("@CodUserUpdate", permisosEmpresasxUsuario.CodUserUpdate ?? "")
                };

                string sql = $"[usr].[SpPermisoEmpresasXusuarios] @Operacion = @Operacion, @IdPermiso = @IdPermiso, @CodUserUpdate = @CodUserUpdate";

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
