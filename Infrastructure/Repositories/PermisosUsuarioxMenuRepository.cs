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
    public class PermisosUsuarioxMenuRepository : BaseRepository<PermisosUsuarioxMenu>, IPermisosUsuarioxMenuRepository
    {
        public PermisosUsuarioxMenuRepository(DbModelContext context) : base(context) { }

        public async Task<List<PermisosUsuarioxMenu>> GetListado()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1")
                };

                string sql = $"[usr].[SpPermisosUsuarioXmenu] @Operacion = @Operacion";

                var response = await _context.PermisosUsuarioxMenus.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<PermisosUsuarioxMenu>> GetPorId(int id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","2"),
                    new SqlParameter("@IdPermiso", id)
                };

                string sql = $"[usr].[SpPermisosUsuarioXmenu] @Operacion = @Operacion, @IdPermiso = @IdPermiso";

                var response = await _context.PermisosUsuarioxMenus.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PostCrear(PermisosUsuarioxMenu permisosUsuarioxMenu)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","3"),
                    new SqlParameter("@CodUsuario", permisosUsuarioxMenu.PumUsrCodUsuario),
                    new SqlParameter("@CodAplicacion", permisosUsuarioxMenu.PumAplCodAplicacion),
                    new SqlParameter("@CodMenu", permisosUsuarioxMenu.PumMenCodMenu),
                    new SqlParameter("@Ejecutar", permisosUsuarioxMenu.PumEjecutar),
                    new SqlParameter("@Leer", permisosUsuarioxMenu.PumLeer),
                    new SqlParameter("@Editar", permisosUsuarioxMenu.PumEditar),
                    new SqlParameter("@Grabar", permisosUsuarioxMenu.PumGrabar),
                    new SqlParameter("@Borrar", permisosUsuarioxMenu.PumBorrar),
                    new SqlParameter("@Consultar", permisosUsuarioxMenu.PumConsultar),
                    new SqlParameter("@CodArchivo", permisosUsuarioxMenu.CodArchivo is null ? "0" : permisosUsuarioxMenu.CodArchivo),
                    new SqlParameter("@CodUser", permisosUsuarioxMenu.CodUser)
                };

                string sql = $"[usr].[SpPermisosUsuarioXmenu] @Operacion = @Operacion, @CodUsuario = @CodUsuario, @CodAplicacion = @CodAplicacion, @CodMenu = @CodMenu, @Ejecutar = @Ejecutar, " +
                    $"@Leer = @Leer, @Editar = @Editar, @Grabar = @Grabar, @Borrar = @Borrar, @Consultar = @Consultar, @CodArchivo = @CodArchivo, @CodUser = @CodUser";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PutActualizar(PermisosUsuarioxMenu permisosUsuarioxMenu)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","4"),
                    new SqlParameter("@IdPermiso", permisosUsuarioxMenu.Id),
                    new SqlParameter("@CodUsuario", permisosUsuarioxMenu.PumUsrCodUsuario),
                    new SqlParameter("@CodAplicacion", permisosUsuarioxMenu.PumAplCodAplicacion),
                    new SqlParameter("@CodMenu", permisosUsuarioxMenu.PumMenCodMenu),
                    new SqlParameter("@Ejecutar", permisosUsuarioxMenu.PumEjecutar),
                    new SqlParameter("@Leer", permisosUsuarioxMenu.PumLeer),
                    new SqlParameter("@Editar", permisosUsuarioxMenu.PumEditar),
                    new SqlParameter("@Grabar", permisosUsuarioxMenu.PumGrabar),
                    new SqlParameter("@Borrar", permisosUsuarioxMenu.PumBorrar),
                    new SqlParameter("@Consultar", permisosUsuarioxMenu.PumConsultar),
                    new SqlParameter("@CodArchivo", permisosUsuarioxMenu.CodArchivo is null ? "0" : permisosUsuarioxMenu.CodArchivo),
                    new SqlParameter("@CodUserUpdate", permisosUsuarioxMenu.CodUserUpdate),
                };

                string sql = $"[usr].[SpPermisosUsuarioXmenu] @Operacion = @Operacion, @IdPermiso = @IdPermiso, @CodUsuario = @CodUsuario, @CodAplicacion = @CodAplicacion, @CodMenu = @CodMenu, @Ejecutar = @Ejecutar, " +
                    $"@Leer = @Leer, @Editar = @Editar, @Grabar = @Grabar, @Borrar = @Borrar, @Consultar = @Consultar, @CodArchivo = @CodArchivo, @CodUserUpdate = @CodUserUpdate";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> DeleteRegistro(PermisosUsuarioxMenu permisosUsuarioxMenu)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","5"),
                    new SqlParameter("@IdPermiso", permisosUsuarioxMenu.Id),
                    new SqlParameter("@CodUserUpdate", permisosUsuarioxMenu.CodUserUpdate)
                };

                string sql = $"[usr].[SpPermisosUsuarioXmenu] @Operacion = @Operacion, @IdPermiso = @IdPermiso, @CodUserUpdate = @CodUserUpdate";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<PermisosXUsuario>> GetPermisosXUsuario(int codUser)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1"),
                    new SqlParameter("@CodUser", codUser)
                };

                string sql = $"[usr].[SpPermisosXUsuario] @Operacion = @Operacion, @CodUser = @CodUser";

                var response = await _context.PermisosXUsuario.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<PermisosXUsuario>> GetPermisosXUsuarioController(int codUser, string controlador)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","2"),
                    new SqlParameter("@CodUser", codUser),
                    new SqlParameter("@Men_Controlador",controlador)
                };

                string sql = $"[usr].[SpPermisosXUsuario] @Operacion = @Operacion, @CodUser = @CodUser,@Men_Controlador=@Men_Controlador";

                var response = await _context.PermisosXUsuario.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
    }
}
