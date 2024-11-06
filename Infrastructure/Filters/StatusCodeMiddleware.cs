namespace Infrastructure.Filters
{
    using Core.Entities;
    using Core.Interfaces;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class StatusCodeMiddleware
    {
        private readonly RequestDelegate _next;

        public StatusCodeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                int statusCode = context.Response.StatusCode;

                if (statusCode == 400)
                {
                    var controllerName = context.GetRouteData()?.Values["controller"]?.ToString();
                    var actionName = context.GetRouteData()?.Values["action"]?.ToString();
                    var routeValues = string.Join(", ", context.GetRouteData()?.Values.Select(kv => $"{kv.Key}: {kv.Value}"));
                    var queryParameters = string.Join(", ", context.Request.Query.Select(kv => $"{kv.Key}: {kv.Value}"));
                    string requestBody;
                    using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                    {
                        requestBody = await reader.ReadToEndAsync();
                        context.Request.Body.Position = 0;
                    }

                    responseBody.Seek(0, SeekOrigin.Begin);
                    string responseBodyContent = await new StreamReader(responseBody).ReadToEndAsync();
                    dynamic responseObject = JObject.Parse(responseBodyContent);

                    string errorMessages = ExtractErrorMessages(responseObject);
                    string sendError = $"{errorMessages} - Route: {(string.IsNullOrWhiteSpace(routeValues) ? "N/A" : routeValues)} - Query: {(string.IsNullOrWhiteSpace(queryParameters) ? "N/A" : queryParameters)} - Body: {(string.IsNullOrWhiteSpace(requestBody) ? "N/A" : requestBody)}";

                    if (!string.IsNullOrEmpty(errorMessages))
                    {
                        await LogAndReturnError(context, serviceProvider, controllerName, actionName, sendError);        
                    }
                }
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
        private static string ExtractErrorMessages(dynamic responseObject)
        {
            StringBuilder errorMessagesBuilder = new StringBuilder();

            if (responseObject != null && responseObject.errors != null)
            {
                var errorObject = responseObject.errors as JObject;
                if (errorObject != null)
                {
                    foreach (var errorProperty in errorObject.Properties())
                    {
                        foreach (var errorMessage in errorProperty.Value)
                        {
                            errorMessagesBuilder.Append(errorMessage.ToString());
                            errorMessagesBuilder.Append(", ");
                        }
                    }
                }
            }

            return errorMessagesBuilder.Length > 0
                ? errorMessagesBuilder.ToString(0, errorMessagesBuilder.Length - 2)
                : string.Empty;
        }

        private static async Task LogAndReturnError(HttpContext context, IServiceProvider serviceProvider, string controllerName, string actionName, string errorMessages)
        {
            var logErroresService = serviceProvider.GetService<ILogErroresService>();

            var errorResult = await logErroresService.SaveError(new LogErrores()
            {
                Controlador = $"{controllerName} - {actionName}",
                Descripcion = errorMessages,
                Origen = "C#",
                Usuario = "NA",
                Funcion = actionName
            });

            var validation = new
            {
                Status = 400,
                Errors = new[] { errorResult }
            };

            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(validation, Formatting.Indented));
        }
    }
}