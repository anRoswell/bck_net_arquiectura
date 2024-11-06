using Api;
using Api.Responses;
using Api.ViewsProcess;
using Core.Enumerations;
using Core.HubConfig;
using Core.Interfaces;
using Core.Options;
using Infrastructure.Extensions;
using Infrastructure.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 209715200;
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
    options.Filters.Add<SoportarCorsAttribute>();
    options.Filters.Add<ValidationFilter>();
    
});

builder.Services.AddEndpointsApiExplorer();
// Agregamos Swagger
builder.Services.AddSwaggerGen();

// Configuraci�n de opciones
builder.Services.AddOptions(config);

builder.Services.AddSwaggerGen(c =>
{
    c.DocumentFilter<SwaggerDocumentFilter>();
});

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

// Agregamos mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


IOptions<ParametrosOptions> tmp = builder.Services.BuildServiceProvider().GetService<IOptions<ParametrosOptions>>();
bool HabilitarJWT = tmp.Value.HabilitarJWT ?? false;

// Configuraci�n para la conexi�n a la Base de Datos
builder.Services.AddDbContexts(config);

// Configuraci�n para la Autenticaci�n (JWT)
//claves_eguimiento_token: seguimientotokenasdf644654sdaf6
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;    

})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Validar el Emisor
        ValidateAudience = true, // Validar la audiencia
        ValidateLifetime = true, // Validar el tiempo de vida del token
        ValidateIssuerSigningKey = true, // Validar la firma del emisor
        ValidIssuer = config["Authentication:Issuer"],
        ValidAudience = config["Authentication:Audience"],
        //IssuerSigningKey = null // new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Authentication:SecretKey"]))
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Authentication:SecretKey"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = (context) =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.HttpContext.Items.Add("TipoError", "Expired");
            }
            else
            {
                context.HttpContext.Items.Add("TipoError", "Invalid");
            }

            return Task.CompletedTask;
        },
        OnChallenge = async (context) =>
        {
            int codigoResponse = 401;
            string error = "";

            // this is a default method
            // the response statusCode and headers are set here
            context.HandleResponse();

            if (HabilitarJWT)
            {
                // AuthenticateFailure property contains
                // the details about why the authentication has failed
                if (context.AuthenticateFailure != null)
                {
                    if (context.HttpContext.Items.ContainsKey("TipoError"))
                    {
                        string tipoError = context.HttpContext.Items["TipoError"].ToString();

                        // AuthenticateFailure property contains
                        // the details about why the authentication has failed
                        if (tipoError.Equals("Expired"))
                        {
                            error = "La sesi�n del usuario est� vencida, por favor ingrese de nuevo al sistema";
                            codigoResponse = (int)HttpStatusCode.Forbidden;
                        }
                        else if (tipoError.Equals("Invalid"))
                        {
                            error = "La autenticaci�n del usuario es inv�lida";
                            codigoResponse = (int)HttpStatusCode.Unauthorized;
                        }

                        // Write to the response in any way you wish
                        context.Response.StatusCode = codigoResponse;
                        context.Response.Headers.Append("WWW-Authenticate", $"Bearer error='{context.Error}'");

                        // we can write our own custom response content here
                        var response = ErrorResponse.GetErrorDescripcion(false, error, context.ErrorDescription, codigoResponse);
                        await context.Response.WriteAsJsonAsync(response);
                    }
                    else
                    {
                        // Write to the response in any way you wish
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.Response.Headers.Append("WWW-Authenticate", $"Bearer error='{context.Error}'");

                        // we can write our own custom response content here
                        var response = ErrorResponse.GetErrorDescripcion(false, "La autenticaci�n del usuario es inv�lida", context.ErrorDescription, 401);
                        await context.Response.WriteAsJsonAsync(response);
                        //await context.Response.CompleteAsync();
                    }
                }
                else
                {
                    error = "La autenticaci�n del usuario es inv�lida";
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.Headers.Append("WWW-Authenticate", $"Bearer error='{error}'");
                    var response = ErrorResponse.GetErrorDescripcion(false, error, "", codigoResponse);
                    await context.Response.WriteAsJsonAsync(response);
                    //await context.Response.CompleteAsync();
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                //context.Response.Headers.Append("WWW-Authenticate", $"Bearer asd6f4sa6df56asd6fsd");                
            }
            await Task.CompletedTask;
        }
    };
});

if (HabilitarJWT)
{
    builder.Services.AddAuthorization(config =>
    {
        config.AddPolicy("ShouldBeAnAdminAreaCentral", options =>
        {
            options.RequireAuthenticatedUser();
            options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            options.Requirements.Add(new ShouldBeAnAdminAreaCentral());
        });
        config.AddPolicy("ShouldBeAnAdminContratistas", options =>
        {
            options.RequireAuthenticatedUser();
            options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            options.Requirements.Add(new ShouldBeAnAdminContratistas());
        });
        config.AddPolicy("ShouldBeAnGosAreaCentral", options =>
        {
            options.RequireAuthenticatedUser();
            options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            options.Requirements.Add(new ShouldBeAnGosAreaCentral());
        });
        config.AddPolicy("ShouldBeAnAdminAreaCentralReadOnly", options =>
        {
            options.RequireAuthenticatedUser();
            options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            options.Requirements.Add(new ShouldBeAnAdminAreaCentralReadOnly());
        });
        config.AddPolicy("ShouldBeAnAdminContratistasReadOnly", options =>
        {
            options.RequireAuthenticatedUser();
            options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            options.Requirements.Add(new ShouldBeAnAdminContratistasReadOnly());
        });
        config.AddPolicy("ShouldBeAnAdminAreaCentralContratistas", options =>
        {
            options.RequireAuthenticatedUser();
            options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            options.Requirements.Add(new ShouldBeAnAdminAreaCentralContratistas());
        });
        config.AddPolicy("ShouldBeAnAdminAreaCentralContratistasTerritorial", options =>
        {
            options.RequireAuthenticatedUser();
            options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            options.Requirements.Add(new ShouldBeAnAdminAreaCentralContratistasTerritorial());
        });
        config.AddPolicy("ShouldBeOsfPolicy", options =>
        {
            options.RequireAuthenticatedUser();
            options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            options.Requirements.Add(new ShouldBeOsfPolicy());
        });
        config.AddPolicy("ShouldBeAnAdminAreaCentralTerritorial", options =>
        {
            options.RequireAuthenticatedUser();
            options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            options.Requirements.Add(new ShouldBeAnAdminAreaCentralTerritorial());
        });
        config.AddPolicy("ShouldBeAnAdminAreaCentralTerritorialReadOnly2", options =>
        {
            options.RequireAuthenticatedUser();
            options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            options.Requirements.Add(new ShouldBeAnAdminAreaCentralTerritorialReadOnly2());
        });
        config.AddPolicy("ShouldBeAnAdminContratistasTerritorial", options =>
        {
            options.RequireAuthenticatedUser();
            options.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            options.Requirements.Add(new ShouldBeAnAdminContratistasTerritorial());
        });
    });
}
else
{
    builder.Services.AddAuthorization(config =>
    {
        config.AddPolicy("ShouldBeAnAdminAreaCentral", options =>
        {
            options.Requirements.Add(new ShouldBeAnAdminAreaCentral());
        });
        config.AddPolicy("ShouldBeAnAdminContratistas", options =>
        {
            options.Requirements.Add(new ShouldBeAnAdminContratistas());
        });
        config.AddPolicy("ShouldBeAnGosAreaCentral", options =>
        {
            options.Requirements.Add(new ShouldBeAnGosAreaCentral());
        });
        config.AddPolicy("ShouldBeAnAdminAreaCentralReadOnly", options =>
        {
            options.Requirements.Add(new ShouldBeAnAdminAreaCentralReadOnly());
        });
        config.AddPolicy("ShouldBeAnAdminContratistasReadOnly", options =>
        {
            options.Requirements.Add(new ShouldBeAnAdminContratistasReadOnly());
        });
        config.AddPolicy("ShouldBeAnAdminAreaCentralContratistas", options =>
        {
            options.Requirements.Add(new ShouldBeAnAdminAreaCentralContratistas());
        });
        config.AddPolicy("ShouldBeAnAdminAreaCentralContratistasTerritorial", options =>
        {
            options.Requirements.Add(new ShouldBeAnAdminAreaCentralContratistasTerritorial());
        });
        config.AddPolicy("ShouldBeOsfPolicy", options =>
        {
            options.Requirements.Add(new ShouldBeOsfPolicy());
        });
        config.AddPolicy("ShouldBeAnAdminAreaCentralTerritorial", options =>
        {
            options.Requirements.Add(new ShouldBeAnAdminAreaCentralTerritorial());
        });
        config.AddPolicy("ShouldBeAnAdminAreaCentralTerritorialReadOnly2", options =>
        {
            options.Requirements.Add(new ShouldBeAnAdminAreaCentralTerritorialReadOnly2());
        });
        config.AddPolicy("ShouldBeAnAdminContratistasTerritorial", options =>
        {
            options.Requirements.Add(new ShouldBeAnAdminContratistasTerritorial());
        });
    });
}

// claves_eguimiento_token: seguimientotokenasdf644654sdaf6
builder.Services.AddHttpContextAccessor(); // Para poder utilizar HttpContext en ShouldBeAnAdminRequirementHandler
builder.Services.AddSingleton<IAuthorizationHandler, ShouldBeAnAdminAreaCentralHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ShouldBeAnAdminContratistasHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ShouldBeAnGosAreaCentralHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ShouldBeAnAdminAreaCentralReadOnlyHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ShouldBeAnAdminContratistasReadOnlyHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ShouldBeAnAdminAreaCentralContratistasHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ShouldBeAnAdminAreaCentralContratistasTerritorialHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ShouldBeOsfPolicyHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ShouldBeAnAdminAreaCentralTerritorialHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ShouldBeAnAdminAreaCentralTerritorialReadOnly2Handler>();
builder.Services.AddSingleton<IAuthorizationHandler, ShouldBeAnAdminContratistasTerritorialHandler>();

// Configuraci�n para controlar CORS
builder.Services.AddCorsApp();

// Inyecci�n de Dependencias
builder.Services.AddServices();

// Configuraci�n para Socket SignalR
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

// Agregamos Swagger
builder.Services.AddSwagger(config, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");

// Configuraci�n para controlar Filtros del Request y las Validaciones de las entidades
builder.Services.AddMvc();

_ = bool.TryParse(builder.Configuration["ElasticConfig:Enabled"], out bool isEnabledElastic);

SetElasticSerilog();

string appsettingFile = $"appsettings.{builder.Environment.EnvironmentName}.json";

builder.Configuration.AddJsonFile(appsettingFile, optional: true, reloadOnChange: true);

// Compilaci�n de app

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .AllowAnyOrigin() // Permitir cualquier origen
            .AllowAnyMethod() // Permitir cualquier m�todo (GET, POST, PUT, DELETE, etc.)
            .AllowAnyHeader(); // Permitir cualquier encabezado
    });
});

var MyAllowSpecificOrigins = "MyAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
});

var app = builder.Build();

if (isEnabledElastic)
{
    // Para registrar cada solicitud HTTP
    app.UseSerilogRequestLogging();
}

// Configuración Swagger
if (new string[] {
    ApiEnvironments.QA,
    ApiEnvironments.Development,
    ApiEnvironments.DevLocal,
    ApiEnvironments.Production,
    ApiEnvironments.Sirion
}.Contains(app.Environment.EnvironmentName))
{
    app.UseSwaggerAuthorized();
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(config["Swagger:Url"], config["Swagger:DefinitionName"]);
        options.RoutePrefix = string.Empty;

        options.DocumentTitle = config["Swagger:DocumentTitle"];
        options.DocExpansion(DocExpansion.None);
    });
}

//app.UseHttpsRedirection();

// Habilitamos los CORS
app.UseCors("ApiCors");
//app.UseStatusCodeMiddleware();
app.UseCors(MyAllowSpecificOrigins);
app.UseCors();
app.UseAuthorization();
app.UseRouting().
    UseAuthentication().
    UseAuthorization().UseEndpoints(endpoints =>
    {
        // Configuramos ruta para Socket SignalR
        endpoints.MapHub<TestHub>(HubConections.Test);
        endpoints.MapHub<UsuarioHub>(HubConections.Usuarios);
        endpoints.MapHub<ProveedorHub>(HubConections.Proveedores);
        endpoints.MapHub<NotificationsHub>(HubConections.Notifications);
    });


app.MapControllers();
app.Run();


void SetElasticSerilog()
{
    string environment = builder.Environment.EnvironmentName;
    string appsettingFile = $"appsettings.{environment}.json";

    builder.Configuration.AddJsonFile(appsettingFile, optional: true, reloadOnChange: true);

    if (isEnabledElastic)
    {
        builder.Services.AddElasticLogging(builder.Configuration, environment);

        // Reemplazar el Logger por defecto con Serilog
        builder.Host.UseSerilog();
    }
}