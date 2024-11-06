using Core.Entities;
using Core.Enumerations;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelResponse;
using Core.QueryFilters;
using Core.Tools;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(DbModelContext context) : base(context) { }

        public async Task<List<Usuario>> GetListarUsuarios()
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","1")
                };

                string sql = $"[usr].[SpUsuario] @Operacion = @Operacion";

                var response = await _context.Usuarios.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<Usuario>> GetUsuarioXCedula(string cedula)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","2"),
                    new SqlParameter("@Cedula", cedula)
                };

                string sql = $"[usr].[SpUsuario] @Operacion = @Operacion, @Cedula = @Cedula";

                var response = await _context.Usuarios.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<Usuario>> GetUsuarioPorId(int id)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","3"),
                    new SqlParameter("@Id", id)
                };

                string sql = $"[usr].[SpUsuario] @Operacion = @Operacion, @Id = @Id";

                var response = await _context.Usuarios.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PostCrearUsuario(Usuario usuario)
        {
            try
            {
                string token = Funciones.GetSHA256(Guid.NewGuid().ToString());

                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","4"),
                    new SqlParameter("@Cedula", usuario.UsrCedula),
                    new SqlParameter("@TipoUsuario", usuario.UsrTusrCodTipoUsuario),
                    new SqlParameter("@Nombres", usuario.UsrNombres),
                    new SqlParameter("@Apellidos", usuario.UsrApellidos),
                    new SqlParameter("@Email", usuario.UsrEmail),
                    new SqlParameter("@Password", usuario.UsrPassword),
                    new SqlParameter("@EmpresaProceso", usuario.UsrEmpresaProceso),
                    new SqlParameter("@Token", token),
                    new SqlParameter("@Estado", usuario.UsrEstado),
                    new SqlParameter("@CodArchivo", usuario.CodArchivo is null ? "0" : usuario.CodArchivo),
                    new SqlParameter("@CodUser", usuario.CodUser ?? "")
                };

                string sql = $"[usr].[SpUsuario] @Operacion = @Operacion, @Cedula = @Cedula, @TipoUsuario = @TipoUsuario, @Nombres = @Nombres, @Apellidos = @Apellidos, @Email = @Email, " +
                    $"@Password = @Password, @Token = @Token, @EmpresaProceso = @EmpresaProceso, @Estado = @Estado, @CodArchivo = @CodArchivo, @CodUser = @CodUser";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PutActualizarUsuario(Usuario usuario)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","5"),
                    new SqlParameter("@Id", usuario.Id),
                    new SqlParameter("@Cedula", usuario.UsrCedula),
                    new SqlParameter("@TipoUsuario", usuario.UsrTusrCodTipoUsuario),
                    new SqlParameter("@Nombres", usuario.UsrNombres),
                    new SqlParameter("@Apellidos", usuario.UsrApellidos),
                    new SqlParameter("@Email", usuario.UsrEmail),
                    new SqlParameter("@EmpresaProceso", usuario.UsrEmpresaProceso),
                    new SqlParameter("@Estado", usuario.UsrEstado),
                    new SqlParameter("@CodArchivo", usuario.CodArchivo is null ? "0" : usuario.CodArchivo),
                    new SqlParameter("@CodUserUpdate", usuario.CodUserUpdate ?? "")
                };

                string sql = $"[usr].[SpUsuario] @Operacion = @Operacion, @Id = @Id, @Cedula = @Cedula, @TipoUsuario = @TipoUsuario, @Nombres = @Nombres, @Apellidos = @Apellidos, @Email = @Email, " +
                    $"@EmpresaProceso = @EmpresaProceso, @Estado = @Estado, @CodArchivo = @CodArchivo, @CodUserUpdate = @CodUserUpdate";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> DeleteUsuario(Usuario usuario)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","6"),
                    new SqlParameter("@Id", usuario.Id),
                    new SqlParameter("@CodUserUpdate", usuario.CodUserUpdate ?? "")
                };

                string sql = $"[usr].[SpUsuario] @Operacion = @Operacion, @Id = @Id, @CodUserUpdate = @CodUserUpdate";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// claves_eguimiento_token: seguimientotokenasdf644654sdaf6
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<List<Usuario>> GetLoginByCredentials(UserLogin login)
        {
            try
            {
                //cfv: la deshabilito con fines de pruebas.
                //SqlParameter[] parameters = new[] {
                //    new SqlParameter("@Operacion","7"),
                //    new SqlParameter("@Email", login.Email)
                //};

                //string sql = $"[usr].[SpUsuario] @Operacion = @Operacion, @Email = @Email";

                //var response = await _context.Usuarios.FromSqlRaw(sql, parameters: parameters).ToListAsync();

                List <Usuario> response = new List<Usuario>() { 
                    new Usuario()
                    {
                        UsrCedula = "1234",
                        Id = 12345,
                        UsrNombreCompleto = "Usuario Prueba",
                        UsrEmail = login.Email,
                        UsrEstado = 1,
                        UsrNombreTipoUsuario = RoleType.DashboardUser.ToString()
                    }
                };
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> CambiarClaveUsuario(Usuario usuario)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","8"),
                    new SqlParameter("@Id", usuario.Id),
                    new SqlParameter("@Password", usuario.UsrPassword),
                    new SqlParameter("@CodUserUpdate", usuario.CodUserUpdate ?? "")
                };

                string sql = $"[usr].[SpUsuario] @Operacion = @Operacion, @Id = @Id, @Password = @Password, @CodUserUpdate = @CodUserUpdate";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> ResetearClaveUsuario(Usuario usuario)
        {
            try
            {
                string token = Funciones.GetSHA256(Guid.NewGuid().ToString());

                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","9"),
                    new SqlParameter("@Id", usuario.Id),
                    new SqlParameter("@Token", token),
                    new SqlParameter("@Password", usuario.UsrPassword), // Es la cedula del usuario
                    new SqlParameter("@CodUserUpdate", usuario.CodUserUpdate ?? "")
                };

                string sql = $"[usr].[SpUsuario] @Operacion = @Operacion, @Id = @Id, @Token = @Token, @Password = @Password, @CodUserUpdate = @CodUserUpdate";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PutActualizarEmpresaUsuario(Usuario usuario)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","10"),
                    new SqlParameter("@Id", usuario.Id),
                    new SqlParameter("@EmpresaProceso", usuario.UsrEmpresaProceso),
                    new SqlParameter("@CodUserUpdate", usuario.CodUserUpdate ?? "")
                };

                string sql = $"[usr].[SpUsuario] @Operacion = @Operacion, @Id = @Id, @EmpresaProceso = @EmpresaProceso, @CodUserUpdate = @CodUserUpdate";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> ForgottenPassword(Usuario usuario)
        {
            try
            {
                string token = Funciones.GetSHA256(Guid.NewGuid().ToString());

                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","11"),
                    new SqlParameter("@Cedula", usuario.UsrCedula),
                    new SqlParameter("@Email", usuario.UsrEmail),
                    new SqlParameter("@Token", token),
                };

                string sql = $"[usr].[SpUsuario] @Operacion = @Operacion, @Cedula = @Cedula, @Email = @Email, @Token = @Token";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> RecoveryPassword(RecoveryParams recoveryParams)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","12"),
                    new SqlParameter("@Token", recoveryParams.Token),
                    new SqlParameter("@Password", recoveryParams.NewPassword)
                };

                string sql = $"[usr].[SpUsuario] @Operacion = @Operacion, @Token = @Token, @Password = @Password";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> PutTmpSuspendidoUsuario(QueryToken param)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","13"),
                    new SqlParameter("@Token", param.Token)
                };

                string sql = $"[usr].[SpUsuario] @Operacion = @Operacion, @Token = @Token";

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
