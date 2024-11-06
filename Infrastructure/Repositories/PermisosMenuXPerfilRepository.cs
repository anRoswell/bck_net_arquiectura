using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelResponse;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PermisosMenuXPerfilRepository : BaseRepository<PermisosMenuXperfil>, IPermisosMenuXPerfilRepository
    {
        public PermisosMenuXPerfilRepository(DbModelContext context) : base(context) { }

        public async Task<List<PermisosMenuXperfil>> GetListado()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1")
                };

                string sql = $"[usr].[SpPermisosMenuXperfil] @Operacion = @Operacion";

                var response = await _context.PermisosMenuXperfils.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<PermisosMenuXperfil>> GetPorId(int id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","2"),
                    new SqlParameter("@IdPermiso", id)
                };

                string sql = $"[usr].[SpPermisosMenuXperfil] @Operacion = @Operacion, @IdPermiso = @IdPermiso";

                var response = await _context.PermisosMenuXperfils.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PostCrear(PermisosMenuXperfil permisosMenuXperfil)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","3"),
                    new SqlParameter("@CodPerfil", permisosMenuXperfil.PmpPrfCodPerfil),
                    new SqlParameter("@CodAplicacion", permisosMenuXperfil.PmpAplCodAplicacion),
                    new SqlParameter("@CodMenu", permisosMenuXperfil.PmpMenCodMenu),
                    new SqlParameter("@Ejecutar", permisosMenuXperfil.PmpEjecutar),
                    new SqlParameter("@Leer", permisosMenuXperfil.PmpLeer),
                    new SqlParameter("@Editar", permisosMenuXperfil.PmpEditar),
                    new SqlParameter("@Grabar", permisosMenuXperfil.PmpGrabar),
                    new SqlParameter("@Borrar", permisosMenuXperfil.PmpBorrar),
                    new SqlParameter("@Consultar", permisosMenuXperfil.PmpConsultar),
                    new SqlParameter("@CodArchivo", permisosMenuXperfil.CodArchivo is null ? "0" : permisosMenuXperfil.CodArchivo),
                    new SqlParameter("@CodUser", permisosMenuXperfil.CodUser)
                };

                string sql = $"[usr].[SpPermisosMenuXperfil] @Operacion = @Operacion, @CodPerfil = @CodPerfil, @CodAplicacion = @CodAplicacion, @CodMenu = @CodMenu, @Ejecutar = @Ejecutar, " +
                    $"@Leer = @Leer, @Editar = @Editar, @Grabar = @Grabar, @Borrar = @Borrar, @Consultar = @Consultar, @CodArchivo = @CodArchivo, @CodUser = @CodUser";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PutActualizar(PermisosMenuXperfil permisosMenuXperfil)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","4"),
                    new SqlParameter("@IdPermiso", permisosMenuXperfil.Id),
                    new SqlParameter("@CodPerfil", permisosMenuXperfil.PmpPrfCodPerfil),
                    new SqlParameter("@CodAplicacion", permisosMenuXperfil.PmpAplCodAplicacion),
                    new SqlParameter("@CodMenu", permisosMenuXperfil.PmpMenCodMenu),
                    new SqlParameter("@Ejecutar", permisosMenuXperfil.PmpEjecutar),
                    new SqlParameter("@Leer", permisosMenuXperfil.PmpLeer),
                    new SqlParameter("@Editar", permisosMenuXperfil.PmpEditar),
                    new SqlParameter("@Grabar", permisosMenuXperfil.PmpGrabar),
                    new SqlParameter("@Borrar", permisosMenuXperfil.PmpBorrar),
                    new SqlParameter("@Consultar", permisosMenuXperfil.PmpConsultar),
                    new SqlParameter("@CodArchivo", permisosMenuXperfil.CodArchivo is null ? "0" : permisosMenuXperfil.CodArchivo),
                    new SqlParameter("@CodUserUpdate", permisosMenuXperfil.CodUserUpdate ?? "")
                };

                string sql = $"[usr].[SpPermisosMenuXperfil] @Operacion = @Operacion, @IdPermiso = @IdPermiso, @CodPerfil = @CodPerfil, @CodAplicacion = @CodAplicacion, @CodMenu = @CodMenu, @Ejecutar = @Ejecutar, " +
                    $"@Leer = @Leer, @Editar = @Editar, @Grabar = @Grabar, @Borrar = @Borrar, @Consultar = @Consultar, @CodArchivo = @CodArchivo, @CodUserUpdate = @CodUserUpdate";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> DeleteRegistro(PermisosMenuXperfil permisosMenuXperfil)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","5"),
                    new SqlParameter("@IdPermiso", permisosMenuXperfil.Id),
                    new SqlParameter("@CodUserUpdate", permisosMenuXperfil.CodUserUpdate ?? "")
                };

                string sql = $"[usr].[SpPermisosMenuXperfil] @Operacion = @Operacion, @IdPermiso = @IdPermiso, @CodUserUpdate = @CodUserUpdate";

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
