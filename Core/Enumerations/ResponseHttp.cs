using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enumerations
{
    public enum ResponseHttp
    {
        /// <summary>
        /// Respuesta Exitosa
        /// </summary>
        Ok = 200,
        /// <summary>
        /// Se ha presentado un error
        /// </summary>
        Error = 400,
        /// <summary>
        /// Usuario sin permisos de acceso
        /// </summary>
        PermisoDenegado = 401,
        /// <summary>
        /// Error Interno del sistema
        /// </summary>
        InternalError = 500
    }
}
