namespace Infrastructure.Filters
{
    using Core.Entities;
    using Core.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Newtonsoft.Json;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ValidationFilter : IAsyncActionFilter
    {
        private readonly ILogErroresService _logErroresService;

        public ValidationFilter(ILogErroresService logErroresService)
        {
            _logErroresService = logErroresService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
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
                var exception = JsonConvert.SerializeObject(context.ModelState);
                
                var errorResult = await _logErroresService.SaveError(new LogErrores()
                {
                    Controlador = controllerName,
                    Descripcion = $"{exception} - Route: {(string.IsNullOrWhiteSpace(routeData) ? "N/A" : routeData)} - Query: {(string.IsNullOrWhiteSpace(queryParameters) ? "N/A" : queryParameters)} - Body: {(string.IsNullOrWhiteSpace(requestBody) ? "N/A" : requestBody)}",
                    Origen = "C#",
                    Usuario = "NA",
                    Funcion = actionMethod
                });

                var validation = new
                {
                    Status = 400,
                    data = new[] { errorResult }
                };

                context.Result = new BadRequestObjectResult(validation);
                return;
            }

            await next();
        }
    }
}
