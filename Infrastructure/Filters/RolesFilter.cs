using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Filters
{
    public class RolesFilter : IAsyncActionFilter
    {
        private readonly IPermisosUsuarioxMenuService _permisosUsuarioxMenuService;

        public RolesFilter(IPermisosUsuarioxMenuService permisosUsuarioxMenuService)
        {
            _permisosUsuarioxMenuService = permisosUsuarioxMenuService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.Items.ContainsKey("UserID"))
            {
                string currentController = context.RouteData.Values["controller"].ToString();
                string metodo = context.HttpContext.Request.Method;
                int idUser = int.Parse(context.HttpContext.Items["UserID"].ToString());
                //var currentAction = context.RouteData.Values["action"];
                List<PermisosXUsuario> resp = await _permisosUsuarioxMenuService.GetPermisosXUsuarioController(idUser, currentController);
                if (resp.Count > 0)
                {
                    if (metodo == "POST" && resp[0].Pmp_Grabar != "GRABAR") {
                        throw new BusinessException($"Error: No tiene permiso de grabar");
                    }

                    if (metodo == "PUT" && resp[0].Pmp_Editar != "EDITAR")
                    {
                        throw new BusinessException($"Error: No tiene permiso de edición");                     
                    }

                    if (metodo == "DELETE" && resp[0].Pmp_Borrar != "BORRAR")
                    {
                        throw new BusinessException($"Error: No tiene permiso de borrado");
                    }

                    if (metodo == "GET" && resp[0].Pmp_Leer != "LEER")
                    {
                        throw new BusinessException($"Error: No tiene permiso de lectura");                        
                    }
                }
                else {
                    throw new BusinessException($"Error: No tiene permiso a esta operacion");
                }

                //var controller = context.ActionDescriptor.
            }

            await next();
            return;
        }
    }
}
