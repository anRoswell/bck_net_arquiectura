namespace Infrastructure.Repositories
{
    using System;
    using System.Data;
    using System.Text.RegularExpressions;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Oracle.ManagedDataAccess.Client;
    using Infrastructure.Extensions;
    using Oracle.ManagedDataAccess.Types;
    using System.Data.Common;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

    public class StoreProcedureRepository<TEntity> : IStoreProcedureRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;

        public StoreProcedureRepository(DbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
            _dbContext.ChangeTracker.LazyLoadingEnabled = false;
            _dbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<TEntity> ExecuteStoreProcedureAsync(string storeProcedure, string parameters = null)
        {
            try
            {
                var command = CreateCommand(storeProcedure, parameters);
                AddOutputParameter(command);
                await command.ExecuteScalarAsync();
                return await ReadOutputParameterAsync(command);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing stored procedure. {ex.Message}", ex.InnerException);
            }
        }

        public async Task<TEntity> ExecuteStoreProcedureNonQueryAsync(string storeProcedure, string parameters = null)
        {
            try
            {
                using var command = CreateCommand(storeProcedure, parameters);
                AddOutputParameter(command);
                await command.ExecuteNonQueryAsync();
                return await ReadOutputParameterAsync(command);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing stored procedure non-query. {ex.Message}", ex.InnerException);
            }
        }

        public async Task<TEntity> ExecuteFunctionNonQueryAsync(string query, string parameters = null)
        {
            try
            {
                using var command = CreateCommand(query, parameters, CommandType.Text);
                AddOutputParameter(command);
                await command.ExecuteNonQueryAsync();
                return await ReadOutputParameterAsync(command);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing function non-query. {ex.Message}", ex.InnerException);
            }
        }

        public async Task<object> ExecuteFunctionNonQueryAsync<T>(string storeProcedure, T entity)
        {
            try
            {
                var parameters = MapEntityToParameters(entity);
                using var command = CreateCommand(storeProcedure, parameters, CommandType.Text);
                var result = await command.ExecuteScalarAsync();
                return result == null || result == DBNull.Value ? default : result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing function non-query with entity parameter. {ex.Message}", ex.InnerException);
            }
        }

        #region Private Methods

        private OracleCommand CreateCommand(string commandText, object parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            try
            {
                var command = _dbContext.Database.GetDbConnection().CreateCommand() as OracleCommand;
                command.CommandType = commandType;
                command.CommandText = commandText;

                if (parameters != null)
                {
                    if (parameters is string jsonInput)
                    {
                        var inputParameter = new OracleParameter("e_json", OracleDbType.Clob)
                        {
                            Direction = ParameterDirection.Input,
                            Value = jsonInput
                        };
                        command.Parameters.Add(inputParameter);
                    }
                    else if (parameters is StoredProcedureParameters storedProcParams)
                    {
                        foreach (var param in storedProcParams.Parameters)
                        {
                            command.Parameters.Add(new OracleParameter(param.Key, param.Value));
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Invalid parameters type.");
                    }
                }

                OpenConnection(command.Connection);
                return command;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating command. {ex.Message}", ex.InnerException);
            }
        }

        private static void OpenConnection(DbConnection connection)
        {
            if (connection.State == ConnectionState.Closed) connection.Open();
        }

        private static void AddOutputParameter(OracleCommand command)
        {
            var outputParameter = new OracleParameter("s_json", OracleDbType.Clob)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(outputParameter);
        }

        private static async Task<TEntity> ReadOutputParameterAsync(OracleCommand command)
        {
            var outputClob = (OracleClob)command.Parameters["s_json"].Value;
            var outputValue = outputClob.Value;
            var outputUnescape = Unescape(outputValue);
            return await DeserializarAsync(outputUnescape);
        }

        private static async Task<TEntity> DeserializarAsync(string jsonString)
        {
            return await Task.Run(() => JsonConvert.DeserializeObject<TEntity>(jsonString));
        }

        private static string Unescape(string input)
        {
            string textoConSecuenciaDeEscape = input;
            string textoDecodificado = Regex.Unescape(textoConSecuenciaDeEscape);
            byte[] bytesUtf8 = Encoding.UTF8.GetBytes(textoDecodificado);
            string textoOriginal = Encoding.UTF8.GetString(bytesUtf8);
            return textoOriginal;
        }

        private static StoredProcedureParameters MapEntityToParameters<T>(T entity)
        {
            var parameters = new StoredProcedureParameters();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(entity);
                parameters.AddParameter(propertyName, propertyValue);
            }
            return parameters;
        }






        #endregion
    }
}
