using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Clase Repositorio, encargada de ejecutar otros Querys, diferente del CRUD, a la Base de Datos
    /// </summary>
    public class RefererServidoresRepositoryOracle : IRefererServidoresRepositoryOracle
    {
        protected readonly DbAireContext _contextaire;

        public RefererServidoresRepositoryOracle(DbAireContext contextaire) {
            _contextaire = contextaire;
        }        

        public async Task<bool> GetPermisoAccesoPorRefererServidoresCore(string Referer, string Grupo)
        {
            // Ejecutamos la funcion para consultar si el Host tiene permitido el acceso.
            string sql = $"SELECT COUNT(*) as NUMEROREGISTROS FROM aire.gnl_peticiones_url_origen where encabezado_peticion_origen = '{Referer}' and permitir_acceso = 1";
            gnl_peticiones_url_origen tmp = await _contextaire.gnl_peticiones_url_origen.FromSqlRaw(sql).FirstOrDefaultAsync();            
            return tmp.NUMEROREGISTROS > 0;
        }
    }
}
