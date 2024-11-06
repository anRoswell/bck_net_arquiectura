using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Filters
{
    public class SoportarCorsAttribute : IAsyncActionFilter
    {
        private readonly IRefererServidoresService _refererHostService;
        private readonly IPeticionesCorsService _peticionesCorsService;

        public SoportarCorsAttribute(IRefererServidoresService refererHostService, IPeticionesCorsService peticionesCorsService)
        {
            _refererHostService = refererHostService;
            _peticionesCorsService = peticionesCorsService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            var GetReferer = "unknow";
            var Request = filterContext.HttpContext.Request;
            RequestHeaders headers = Request.GetTypedHeaders();

            if (headers.Referer != null)
            {
                string AbsoluteUri = headers.Referer.AbsoluteUri;
                //string AbsolutePath = header.Referer.AbsolutePath;
                string PathAndQuery = headers.Referer.PathAndQuery;
                GetReferer = AbsoluteUri.Substring(0, AbsoluteUri.Length - PathAndQuery.Length);
            }

            var controllerActionDescriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;
            string controllerName = controllerActionDescriptor?.ControllerName;
            string actionMethod = controllerActionDescriptor?.ActionName;

            string Grupo = Request.Query["Grupo"].ToString();

            string token = headers.Headers["Authorization"].ToString();
            string metodo = Request.Method;

            var Myuser = filterContext?.HttpContext?.Items["id_usuario"]?.ToString() ?? "7777777";
            //------Grabar Log
            await GrabarLogOracle(new gnl_peticiones_cors
            {
                metodo_accion = actionMethod,
                usuario_registra = Myuser,
                nombre_controlador = controllerName,
                fecha_registra = DateTime.Now,
                grupo = Grupo,
                typo_metodo = metodo,
                encabezado_peticion_origen = GetReferer,
                token = token,
            });

            //-----Validar que el Referrer tenga permisos.
            bool PermitirHostRemoto;
            PermitirHostRemoto = await _refererHostService.GetPermisoAccesoPorRefererServidoresOracle(GetReferer, Grupo);

            if (!PermitirHostRemoto)
            {
                throw new BusinessException($"El Host Remoto: {GetReferer} actualmente no tiene permisos de acceso.");
            }

            await next();
        }

        private async Task GrabarLog(PeticionesCors peticionesCors)
        {
            await _peticionesCorsService.RegisterLog(peticionesCors);
        }

        private async Task GrabarLogOracle(gnl_peticiones_cors peticionesCors)
        {
            await _peticionesCorsService.RegisterLogOracle(peticionesCors);
        }
    }
}
