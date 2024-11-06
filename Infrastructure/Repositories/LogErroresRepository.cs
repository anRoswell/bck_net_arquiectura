using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Messages;
using Core.ModelResponse;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infrastructure.Repositories
{
    public class LogErroresRepository : ILogErroresRepository
    {
        protected DbContext DbContext { get; set; }

        public LogErroresRepository(DbContext context) {
            DbContext = context;
        }

        public async Task<List<ResponseAction>> SaveError(LogErrores error)
        {
            try
            {
                int? result = 0;
                var connectionString = DbContext.Database.GetConnectionString();
                using (var connection = new OracleConnection(connectionString))
                {
                    await connection.OpenAsync();
                    OracleCommand command = connection.CreateCommand();
                    command.CommandText = "BEGIN :result := aire.PKG_P_GENERALES.fnc_registrar_log(:nombreUp, :tipoMensaje, :mensaje); END;";

                    // Parámetro de salida para obtener el ID del log
                    command.Parameters.Add("result", OracleDbType.Int32, ParameterDirection.ReturnValue);

                    // Parámetros de entrada para la función
                    command.Parameters.Add("nombreUp", OracleDbType.Varchar2).Value = $"{error.Controlador} - {error.Funcion}";
                    command.Parameters.Add("tipoMensaje", OracleDbType.Varchar2).Value = "ERROR";
                    command.Parameters.Add("mensaje", OracleDbType.Varchar2).Value = error.Descripcion;

                    var a = await command.ExecuteScalarAsync();

                    // Obtener el valor de retorno de la función
                    result = Convert.ToInt32(a);
                }
                List<ResponseAction> response = new List<ResponseAction>() {
                    new ResponseAction()
                    {
                        estado = true,
                        mensaje = "ERROR",
                        error = error.Descripcion,
                        Id = result
                    }
                };
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }




    }
}