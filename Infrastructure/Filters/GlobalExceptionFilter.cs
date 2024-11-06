using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Filters
{
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogErroresService _logErroresService;
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogErroresService logErroresService, ILogger<GlobalExceptionFilter> logger)
        {
            _logErroresService = logErroresService;
            _logger = logger;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception.GetType() == typeof(BusinessException))
            {
                var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
                string controllerName = controllerActionDescriptor?.ControllerName;
                string actionMethod = controllerActionDescriptor?.ActionName;
                var routeData = string.Join(", ", context.HttpContext.Request.RouteValues.Select(kv => $"{kv.Key}: {kv.Value}"));
                var queryParameters = string.Join(", ", context.HttpContext.Request.Query.Select(kv => $"{kv.Key}: {kv.Value}"));
                string requestBody;
                using (StreamReader reader = new(context.HttpContext.Request.Body, Encoding.UTF8))
                {
                    requestBody = await reader.ReadToEndAsync();
                }

                var exception = (BusinessException)context.Exception;
                var exceptionMessage = $"Message: {exception.Message}{(string.IsNullOrWhiteSpace(exception.Message) ? "N/A" : " - StackTrace: " + exception.StackTrace)}{(exception.InnerException != null ? " - InnerException: " + exception.InnerException.Message : "")}";
                string user = context.HttpContext.Items.ContainsKey("UserID") ? context.HttpContext.Items["UserID"]?.ToString() : "0";

                // Registramos el error en la Base de Datos
                ResponseAction logSaved = await _logErroresService.SaveError(new LogErrores()
                {
                    Controlador = controllerName,
                    Descripcion = $"{exceptionMessage} - Route: {(string.IsNullOrWhiteSpace(routeData) ? "N/A" : routeData)} - Query: {(string.IsNullOrWhiteSpace(queryParameters) ? "N/A" : queryParameters)} - Body: {(string.IsNullOrWhiteSpace(requestBody) ? "N/A" : requestBody)}",
                    Origen = "C#",
                    Usuario = user,
                    Funcion = actionMethod
                });

                _logger.LogError(exception.Message);

                ResponseAction resp;

                if (logSaved != null)
                {
                    if (logSaved.estado)
                    {
                        // Armamos respuesta al front
                        resp = new ResponseAction
                        {
                            estado = false,
                            error = exception.Message,
                            mensaje = "Error interno del sistema" // Mensaje para el usuario
                        };
                    }
                    else
                    {
                        // Armamos respuesta al front
                        resp = new ResponseAction
                        {
                            estado = logSaved.estado,
                            error = logSaved.error,
                            mensaje = "Error interno del sistema.",
                            codigo_error = logSaved.codigo_error
                        };
                    }
                }
                else
                {
                    // Armamos respuesta al front
                    resp = new ResponseAction
                    {
                        estado = false,
                        error = "No se pudo guardar log de errores en la base de datos",
                        mensaje = "Error interno del sistema." // Mensaje para el usuario
                    };
                }

                // Armamos respuesta al front y la enviamos
                var validation = new
                {
                    Status = 400,
                    Errors = new[] { resp }
                };

                context.Result = new BadRequestObjectResult(validation);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.ExceptionHandled = true;
            }
            else if (context.Exception.GetType() == typeof(Exception))
            {
                var exception = context.Exception;
                _logger.LogError(exception.Message);
            }
        }
    }
}
