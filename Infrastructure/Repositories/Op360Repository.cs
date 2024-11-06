using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.QueryFilters;
using Infrastructure.CustomEntitiesOracle;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infrastructure.Repositories
{
    public class Op360Repository : IOp360Repository
    {
        protected readonly DbAireContext _context;
        
        public Op360Repository(DbAireContext context)
        {
            _context = context;            
        }
        /*
        * fecha: 19/12/2023
        * clave: 5e6ewrt546weasdf _08
        * clave: 5e6ewrtLOGINEXTERNO6weasdf _08
        * carlos vargas
        */
        #region Seguridad
        public async Task<Usuarios_Perfiles> ConsultarUsusariosxPerfiles(QueryOp360Seguridad parameters)
        {
            try
            {
                string connectionString = _context.Database.GetConnectionString();
                Usuarios_Perfiles response = new Usuarios_Perfiles() { };

                /*
                 * Consumir store procedure y obtener info blob
                 */
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    using (OracleCommand cmd = connection.CreateCommand())
                    {
                        // Configura el comando para el procedimiento almacenado
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "aire.pkg_g_seguridad.prc_consultar_usuarios_perfiles";

                        // Parámetros del procedimiento almacenado
                        OracleParameter[] sqlParams = new OracleParameter[] {
                                new OracleParameter{ ParameterName = "e_id_usuario", OracleDbType = OracleDbType.Int32, Value = parameters.e_id_usuario },
                                new OracleParameter
                                {
                                    ParameterName = "s_usuarios", OracleDbType = OracleDbType.Clob,Size = 4000, Direction = ParameterDirection.Output
                                }
                            };
                        cmd.Parameters.AddRange(sqlParams);

                        // Ejecuta el procedimiento almacenado
                        await cmd.ExecuteNonQueryAsync();

                        // Obtiene el valor del parámetro de salida (CLOB)
                        if (cmd.Parameters["s_usuarios"].Value != DBNull.Value)
                        {
                            // Convierte el CLOB a una cadena directamente
                            OracleClob clob = (OracleClob)cmd.Parameters["s_usuarios"].Value;
                            string usuarios = clob.Value;
                            response = JsonConvert.DeserializeObject<Usuarios_Perfiles>(usuarios);
                        }
                    }
                }

                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
        public async Task<Usuarios_Perfiles> ValidarLoginExterno(string Id_Usuario, string Token_Apex)
        {
            try
            {
                string connectionString = _context.Database.GetConnectionString();
                Usuarios_Perfiles response = new Usuarios_Perfiles() { };

                /*
                 * Consumir store procedure y obtener info blob
                 */
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    using (OracleCommand cmd = connection.CreateCommand())
                    {
                        // Configura el comando para el procedimiento almacenado
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "aire.pkg_g_seguridad.prc_validar_token_informacion";

                        // Parámetros del procedimiento almacenado
                        OracleParameter[] sqlParams = new OracleParameter[] {
                                new OracleParameter{ ParameterName = "e_id_token", OracleDbType = OracleDbType.Varchar2, Value = Token_Apex },
                                new OracleParameter{ ParameterName = "e_id_usuario", OracleDbType = OracleDbType.Varchar2, Value = Id_Usuario },
                                new OracleParameter
                                {
                                    ParameterName = "s_json_informacion", OracleDbType = OracleDbType.Clob,Size = 4000, Direction = ParameterDirection.Output
                                },
                                new OracleParameter
                                {
                                    ParameterName = "s_respuesta",
                                    OracleDbType = OracleDbType.Object,
                                    Direction = ParameterDirection.Output,
                                    UdtTypeName = "AIRE.TIP_RESPUESTA"
                                }
                            };
                        cmd.Parameters.AddRange(sqlParams);

                        // Ejecuta el procedimiento almacenado
                        await cmd.ExecuteNonQueryAsync();

                        // Obtiene el valor del parámetro de salida (CLOB)
                        if (cmd.Parameters["s_json_informacion"].Value != DBNull.Value)
                        {
                            // Convierte el CLOB a una cadena directamente
                            OracleClob clob = (OracleClob)cmd.Parameters["s_json_informacion"].Value;
                            var s_respuesta = (CustomTIP_RESPUESTA)cmd.Parameters["s_respuesta"].Value;
                            Usuarios_Perfiles responsedef = new Usuarios_Perfiles()
                            {
                                Estado = 400,
                                Mensaje = s_respuesta.MENSAJE
                            };
                            string usuarios = clob.Length > 0 ? clob.Value : "";
                            response = !string.IsNullOrEmpty(usuarios) ? JsonConvert.DeserializeObject<Usuarios_Perfiles>(usuarios) : responsedef;
                            var nn = response.Mensaje;
                        }
                    }
                }

                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
        #endregion

        #region Ordenes
        public async Task<string> Prueba()
        {
            try
            {
                string connectionString = _context.Database.GetConnectionString();
                string response = "";

                /*
                 * Consumir store procedure y obtener info blob
                 */
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    using (OracleCommand cmd = connection.CreateCommand())
                    {
                        // Configura el comando para el procedimiento almacenado
                        //cmd.CommandType = System.Data.CommandType.Text;
                        //cmd.CommandText = "Insert into AIRE.ORD_ORDENES_CARGUE_TEMPORAL_CIERRE " +
                        //    " (ORDEN,NUMERO_ORDEN,LECTURA,OBSERVACION,CODIGO_TIPO_ORDEN,CODIGO_CAUSAL_CIERRE,ID_SOPORTE,USUARIO_REGISTRA) " +
                        //    " values ('2','3375545','0','Cierre Administrativo','TO501','3443','0','1')";

                        cmd.CommandText = "CREATE TABLE AIRE.ORD_ORDENES_CARGUE_TEMPORAL_CIERRE_6784 " +
                            " (" +
                            "ORDEN NUMBER," +
                            "NUMERO_ORDEN VARCHAR2(100)," +
                            "LECTURA VARCHAR2(100)," +
                            "OBSERVACION VARCHAR2(2000)," +
                            "CODIGO_TIPO_ORDEN VARCHAR2(100)," +
                            "CODIGO_CAUSAL_CIERRE VARCHAR2(100)," +
                            "ID_SOPORTE NUMBER," +
                            "USUARIO_REGISTRA VARCHAR2(50)" +
                            ")";
                        // Ejecuta el procedimiento almacenado
                        cmd.ExecuteNonQuery();                        
                    }
                }

                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public ResponseDto Procesar_Crear_Tabla_Tmp_SCR(QueryOp360CargueMasivoData parameters)
        {
            try
            {
                string connectionString = _context.Database.GetConnectionString();
                ResponseDto response = new() { 
                    Codigo = 200,
                    Mensaje = "Proceso exitoso"
                };

                /*
                 * Consumir store procedure y obtener info blob
                 */
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    using (OracleCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = $"CREATE TABLE AIRE.ORD_ORDENES_CARGUE_TEMPORAL_CIERRE_{parameters.id_soporte} " +
                            " (" +
                            "ORDEN NUMBER," +
                            "NUMERO_ORDEN VARCHAR2(100)," +
                            "LECTURA VARCHAR2(100)," +
                            "OBSERVACION VARCHAR2(2000)," +
                            "CODIGO_TIPO_ORDEN VARCHAR2(100)," +
                            "CODIGO_CAUSAL_CIERRE VARCHAR2(100)," +
                            "ID_SOPORTE NUMBER," +
                            "USUARIO_REGISTRA VARCHAR2(50)" +
                            ")";
                        // Ejecuta el procedimiento almacenado
                        cmd.ExecuteNonQuery();
                    }
                }

                return response;
            }
            catch (Exception e)
            {
                ResponseDto response = new()
                {
                    Codigo = 200,
                    Mensaje = $"error: {e.Message}, {e.InnerException?.Message ?? ""}"
                };
                return response;
            }
        }
        #endregion

        #region GetSequenceOracle
        public async Task<int> GetSecuenceOracle(string NameSequence)
        {
            try
            {
                int numsec = 0;
                string connectionString = _context.Database.GetConnectionString();
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    using (OracleCommand cmd = connection.CreateCommand())
                    {
                        // Nombre del procedimiento almacenado
                        cmd.CommandText = $"BEGIN SELECT AIRE.{NameSequence}.NEXTVAL INTO :NUMSEC FROM DUAL; END;";
                        cmd.CommandType = CommandType.Text;

                        List<OracleParameter> parameters = new List<OracleParameter>();
                        OracleParameter NUMSEC = new OracleParameter("NUMSEC", OracleDbType.Int64);
                        NUMSEC.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(NUMSEC);

                        // Ejecutar el procedimiento almacenado
                        var result = cmd.ExecuteNonQuery();

                        // Obtener el valor del parámetro de salida
                        numsec = Int32.Parse(cmd.Parameters["NUMSEC"].Value.ToString());
                    }
                }

                return numsec;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
        #endregion
    }
}