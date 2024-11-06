using Core.Options;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Filters
{
    public class SwaggerDocumentFilter : IDocumentFilter
    {
        private SwaggerOptions _swaggerOptions2;

        public SwaggerDocumentFilter(
            IOptions<SwaggerOptions> swaggerOptions2
            )
        {
            _swaggerOptions2 = swaggerOptions2.Value;
        }
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (!_swaggerOptions2.Autorizar)
            {
                swaggerDoc.Paths.Clear();
                context.ApiDescriptions.Reverse();
                swaggerDoc.Components.Schemas.Clear();
                swaggerDoc.Components.SecuritySchemes.Clear();                
            }
            else
            {
                //swaggerDoc.Paths.Remove("/WeatherForecast/GetMethodFour");
                string[] pathsmtrz =
                    new[]{
                        "/AdobeSign","/Apoteosys", "/Contrato", "/CertificadosEspeciales","/Documentos","/Empresa","/EmpresasxUsuario","/Files","/Menu"
                        ,"/MenuXPerfil","/Noticias","/Notifications","/OrdenesMaestras","/Perfil","/PerfilesXusuario","/Plantilla","/Proveedor"
                        ,"/RepresentantesLegalEmpresa","/ReqQuestionAnswer","/Requerimientos","/SSR","/Tercero","/TipoMinuta","/UnidadesNegocio","/Usuario","/UsuarioxMenu"
                    };
                foreach (var item in swaggerDoc.Paths)
                {
                    if (pathsmtrz.Any(x => item.Key.Contains(x)))
                        swaggerDoc.Paths.Remove(item.Key);
                }
            }
        }
    }
}
