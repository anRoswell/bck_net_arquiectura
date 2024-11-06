using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Clase Repositorio, encargada de ejecutar otros Querys, diferente del CRUD, a la Base de Datos
    /// </summary>
    public class RefererServidoresRepository : BaseRepository<RefererServidore>, IRefererServidoresRepository
    {
        public RefererServidoresRepository(DbModelContext context) : base(context) { }

        public async Task<bool> GetPermisoAccesoPorRefererServidores(string Referer, string Grupo)
        {
            SqlParameter[] @params =
             {
                new SqlParameter("@returnVal", SqlDbType.Bit) {Direction = ParameterDirection.Output}
               //new SqlParameter("@returnVal2", SqlDbType.Int) {Direction = ParameterDirection.ReturnValue}
             };

            // Ejecutamos la funcion para consultar si el Host tiene permitido el acceso.
            await _context.Database.ExecuteSqlRawAsync($"SET @returnVal = (SELECT dbo.GetPermisoAccesoPorRefererServidores('{Referer}'))", @params);
            var result = (bool)@params[0].Value;
            return result;
        }
    }
}
