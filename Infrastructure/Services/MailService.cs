namespace Infrastructure.Services
{
    using Core.CustomEntities;
    using Core.Entities;
    using Core.Exceptions;
    using Core.ModelProcess;
    using Core.ModelResponse;
    using Core.QueryFilters;
    using Core.Tools;
    using Infrastructure.Data;
    using Infrastructure.Interfaces;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MailService : IMailService
    {
        DbModelContext _context;

        public MailService(DbModelContext context)
        {
            _context = context;
        }

        public async Task<List<ResponseAction>> SendMailMasive(QuerySendMailMasive parameters)
        {
            try
            {
                SqlParameter[] paramts = new[] {
                    new SqlParameter("@Operacion","1"),
                    new SqlParameter("@To",parameters.To),
                    new SqlParameter("@Cco",parameters.CCO),
                    new SqlParameter("@Asunto",parameters.Asunto),
                    new SqlParameter("@Body",parameters.Body),
                };

                string sql = $"[conf].[SpGestionMail] @Operacion = @Operacion, @To = @To, @Cco = @Cco, @Asunto = @Asunto, @Body = @Body";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: paramts).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> SendMail_ContratosPendientes(QuerySendMailMasive parameters)
        {
            try
            {
                List<ResponseAction> response;
                bool todoOk = true;
                List<string> correosIssue = new List<string>();

                // Consultamos la tabla [temp].[ActividadesContratoPendientes]
                SqlParameter[] paramts = new[] {
                    new SqlParameter("@Operacion","24")
                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion";
                List<ActividadesPendientesContrato> listadoActPend = await _context.ActividadesPendientesContratos.FromSqlRaw(sql, paramts).ToListAsync();

                List<int> listadoIdsUsuarios = (from x in listadoActPend
                                                select x.CodUsuario).Distinct().ToList();

                // Recorremos los datos arrojados por [temp].[ActividadesContratoPendientes] y armamos el HTML
                foreach (int id in listadoIdsUsuarios)
                {
                    List<ActividadesPendientesContrato> listadoActPendUsuario = listadoActPend.Where(x => x.CodUsuario == id).ToList();
                    ActividadesPendientesContrato entity = listadoActPendUsuario.FirstOrDefault();

                    // Creamos el HTML de la estructura de la información
                    string html = ContratosProcess.MapearActividadesPendientesPdf(listadoActPendUsuario, parameters.OpcionAEjecutar);

                    // Generamos el PDF, por usuario, con actividades pendientes
                    byte[] archivoPdf = Funciones.PdfSharpConvertWithoutCreateFile(html);

                    // Enviamos el correo
                    Mail oMail = new Mail(entity.EmailUsuario, parameters.Body, parameters.Asunto);

                    oMail.AgregarAdjunto_Documento(new Email_Archivo()
                    {
                        Archivo = archivoPdf,
                        Nombre = "Contratos_Pendientes",
                        TipoArchivo = "application/pdf"
                    }
                    );

                    if (!oMail.EnviaMail())
                    {
                        //Console.Write("no se envio el mail: " + oMail.error);
                        todoOk = false;
                        correosIssue.Add($"Contrato:{entity.IdContrato}-IdUsuario:{entity.CodUsuario}-CorreoUsuario:{entity.EmailUsuario}[{oMail.error}]");
                    }
                }

                if (todoOk)
                {
                    response = new List<ResponseAction>
                    {
                        new ResponseAction
                        {
                            estado = true,
                            mensaje = "Notificaciones de actividades de contratos pendientes, ejecutada exitosamente!"
                        }
                    };
                }
                else
                {
                    response = new List<ResponseAction>
                        {
                            new ResponseAction()
                            {
                                estado = false,
                                mensaje = string.Join("|", correosIssue.ToArray())
                            }
                        };
                }

                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        public async Task<List<ResponseAction>> SendMail_ContratosFirmasPendientes(QuerySendMailMasive parameters)
        {
            try
            {
                List<ResponseAction> response;
                bool todoOk = true;
                List<string> correosIssue = new List<string>();

                // Consultamos la tabla [temp].[ActividadesContratoPendientes]
                SqlParameter[] paramts = new[] {
                    new SqlParameter("@Operacion","24")
                };

                string sql = $"[par].[SpParametrosIniciales] @Operacion = @Operacion";
                List<ActividadesPendientesContrato> listadoActPend = await _context.ActividadesPendientesContratos.FromSqlRaw(sql, paramts).ToListAsync();

                List<string> listadoEmailsUsuarios = (from x in listadoActPend
                                                      select x.EmailUsuario).Distinct().ToList();

                // Recorremos los datos arrojados por [temp].[ActividadesContratoPendientes] y armamos el HTML
                foreach (string email in listadoEmailsUsuarios)
                {
                    List<ActividadesPendientesContrato> listadoActPendUsuario = listadoActPend.Where(x => x.EmailUsuario == email).ToList();
                    ActividadesPendientesContrato entity = listadoActPendUsuario.FirstOrDefault();

                    // Creamos el HTML de la estructura de la información
                    string html = ContratosProcess.MapearActividadesPendientesPdf(listadoActPendUsuario, parameters.OpcionAEjecutar);

                    // Generamos el PDF, por usuario, con actividades pendientes
                    byte[] archivoPdf = Funciones.PdfSharpConvertWithoutCreateFile(html);

                    // Enviamos el correo
                    Mail oMail = new Mail(entity.EmailUsuario, parameters.Body, parameters.Asunto);

                    oMail.AgregarAdjunto_Documento(new Email_Archivo()
                    {
                        Archivo = archivoPdf,
                        Nombre = "Contratos_Pendientes_Firmar",
                        TipoArchivo = "application/pdf"
                    }
                    );

                    if (!oMail.EnviaMail())
                    {
                        //Console.Write("no se envio el mail: " + oMail.error);
                        todoOk = false;
                        correosIssue.Add($"Contrato:{entity.IdContrato}-IdUsuario:{entity.CodUsuario}-CorreoUsuario:{entity.EmailUsuario}[{oMail.error}]");
                    }
                }

                if (todoOk)
                {
                    response = new List<ResponseAction>
                    {
                        new ResponseAction
                        {
                            estado = true,
                            mensaje = "Notificaciones de actividades de contratos pendientes, ejecutada exitosamente!"
                        }
                    };
                }
                else
                {
                    response = new List<ResponseAction>
                        {
                            new ResponseAction()
                            {
                                estado = false,
                                mensaje = string.Join("|", correosIssue.ToArray())
                            }
                        };
                }

                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
    }
}
