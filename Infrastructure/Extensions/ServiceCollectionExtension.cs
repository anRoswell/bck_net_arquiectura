using Core.Interfaces;
using Core.ModelProcess;
using Core.Options;
using Core.Services;
using Infrastructure.Data;
using Infrastructure.Factories;
using Infrastructure.Filters;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using System;
using System.Data;
using System.IO;
using Serilog.Exceptions;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            string connStringPortal = configuration.GetConnectionString("PortalProveedoresEntities");
            string connStringApoteosis = configuration.GetConnectionString("ApoteosysEntities");
            string connStringSispo = configuration.GetConnectionString("SispoEntities");
            string connStringAireOp360 = configuration.GetConnectionString("AireOp360");

            SqlConnectionStringBuilder builder = new(connStringPortal)
            {
                TrustServerCertificate = true
            };

            services.AddDbContext<DbModelContext>(options =>
            {
                options.UseSqlServer(builder.ConnectionString);
            }, ServiceLifetime.Transient);

            // Configuración para la conexión a la Base de Datos Oracle
            services.AddDbContext<DbOracleContext>(options =>
            {
                options.UseOracle(connStringApoteosis);
            }, ServiceLifetime.Transient);

            // Configuración para la conexión a la Base de Datos SISPO
            services.AddDbContext<DbSispoContext>(options =>
            {
                options.UseOracle(connStringSispo);
            }, ServiceLifetime.Transient);

            /*
             * Configuración para la conexión a la Base de Datos AIRE OP360
             * fecha: 19/12/2023
             * clave: asd6f4sad65f4sad4f6asd65f65as6f
             * carlos vargas
             */
            services.AddDbContext<DbAireContext>(options =>
            {
                options.UseOracle(connStringAireOp360);
            }, ServiceLifetime.Transient);

            services.AddTransient<IDbConnection>((sp) => new SqlConnection(builder.ConnectionString));

            return services;
        }

        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuraciones de Paginacion y Password
            services.Configure<PaginationOptions>(options => configuration.GetSection("Pagination").Bind(options));
            services.Configure<PasswordOptions>(options => configuration.GetSection("PasswordOptions").Bind(options));
            services.Configure<AdobeSignOptions>(options => configuration.GetSection("AdobeSignOptions").Bind(options));
            //services.Configure<PathOptions>(options => configuration.GetSection("PathOptions").Bind(options));
            services.Configure<PoliticasOptions>(options => configuration.GetSection("PoliticasOptions").Bind(options));
            services.Configure<ParametrosOptions>(options => configuration.GetSection("ParametrosOptions").Bind(options));

            services.Configure<SwaggerOptions>(options => configuration.GetSection("SwaggerOptions").Bind(options));
            services.AddSingleton<SwaggerOptions>();
            services.Configure<ParametrosCarguesOptions>(options => configuration.GetSection("ParametrosCargues").Bind(options));
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Inyección de Depencias
            /* Suponiendo que cambiamos de motor de Base de Datos, este proceso nos facilita que
             * no nos toque reestructurar el proyecto para acoplarlo a cada motor de Base de datos.
             */
            services.AddTransient<IFilesProcess, FilesProcess>();
            services.AddTransient<IMenuService, MenuService>();
            services.AddTransient<IParametrosInicialesService, ParametrosInicialesService>();
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<IPerfilService, PerfilService>();
            services.AddTransient<IPerfilesXusuarioService, PerfilesXusuarioService>();
            services.AddTransient<IPermisosEmpresasxUsuarioService, PermisosEmpresasXUsuarioService>();
            services.AddTransient<IPermisosMenuXPerfilService, PermisosMenuXPerfilService>();
            services.AddTransient<IPermisosUsuarioxMenuService, PermisosUsuarioxMenuService>();
            services.AddTransient<IProveedorService, ProveedorService>();
            services.AddTransient<IPrvDocumentoService, PrvDocumentoService>();
            services.AddTransient<IReqQuestionAnswerService, ReqQuestionAnswerService>();
            services.AddTransient<INotificationsService, NotificationsService>();
            services.AddTransient<ICertificadosEspecialesService, CertificadosEspecialesService>();

            services.AddTransient<IOrdenesMaestrasService, OrdenesMaestrasService>();

            services.AddTransient<IRefererServidoresService, RefererServidoresService>();
            services.AddTransient<IPeticionesCorsService, PeticionesCorsService>();            

            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IMailService, MailService>();
            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IAdobeSignService, AdobeSignService>();
            services.AddSingleton<IUriService>(provider =>
            {
                var accesor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accesor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(absoluteUri);
            });

            /*
            * fecha: 19/12/2023
            * clave: 5e6ewrt546weasdf _11
            * clave: 5e6ewrt546weasdfm _11
            * carlos vargas
            */
            services.AddTransient<IOp360Service, Op360Service>();            
            services.AddTransient<IOp360ReportingService, Op360ReportingService>();
            services.AddTransient<IOp360GDMovilService, Op360GDMovilService>();
            services.AddTransient<IOp360GDWebService, Op360GDWebService>();
            services.AddTransient<IOp360ASEGREDWebService, Op360ASEGREDWebService>();
            services.AddTransient<IOp360ASEGREDMovilService, Op360ASEGREDMovilService>();
            services.AddTransient<IOp360ASEGREDManualDeUsoMovilService, Op360ASEGREDManualDeUsoMovilService>();
            services.AddTransient<IOp360IntegrationService, Op360IntegrationService>();
            services.AddTransient<IVersonDBService, VersonDBService>();

            services.AddTransient<IPlantillaService, PlantillaService>();
            services.AddTransient<IApoteosysService, ApoteosysService>();
            services.AddTransient<ISispoSevice, SispoService>();
            services.AddTransient<IRequerimientosService, RequerimientosService>();
            services.AddTransient<INoticiaService, NoticiaService>();
            services.AddTransient<INoticiasDocService, NoticiasDocService>();
            services.AddTransient<IContratoService, ContratoService>();
            services.AddTransient<IDocumentoService, DocumentoService>();
            services.AddTransient<IUnidadesNegocioService, UnidadesNegocioService>();
            services.AddScoped<RolesFilter>();
            services.AddTransient<IReportingService, ReportingService>();
            services.AddTransient<ILogErroresService, LogErroresService>();
            services.AddTransient<IEmpresasService, EmpresasService>();
            services.AddTransient<ITipoMinutaService, TipoMinutaService>();
            services.AddTransient<ITerceroService, TerceroService>();
            services.AddTransient<IRepresentantesLegalEmpresaService, RepresentantesLegalEmpresaService>();

            services.AddTransient<IStoreProcedureService, StoreProcedureService>();

            services.AddTransient<IAire360MCargaInicialService, Aire360MCargaInicialService>();
            services.AddTransient<IFileManagementService, FileManagementService>();
            services.AddTransient<IFileAccessService, FileAccessService>();

            services.AddTransient<IDynamicBulkService, DynamicBulkService>();

            services.AddTransient<ICargaMasivaFactory, CargaMasivaFactory>();
            services.AddTransient<ICargaMasivaConfiguracion, CargaMasivaScrCrearOrdenService>();
            services.AddTransient<ICargaMasivaConfiguracion, CargaMasivaScrReasigOrdenService>();
            services.AddTransient<ICargaMasivaConfiguracion, CargaMasivaScrCerrarOrdenService>();
            services.AddTransient<ICargaMasivaConfiguracion, CargaMasivaGosService>();
            services.AddTransient<ICargaMasivaConfiguracion, CargaMasivaScrAsigTecnicoService>();
            services.AddTransient<ICargaMasivaConfiguracion, CargaMasivaScrDesasigTecnicoService>();
            services.AddTransient<ICargaMasivaConfiguracion, CargaMasivaScrReasigOrden2Service>();

            services.AddTransient<IOp360GosWebService, Op360GosWebService>();
            services.AddTransient<IOp360GosMobileService, Op360GosMobileService>();


            services.AddHttpClient();

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration, string xmlFileName)
        {
            // Configuracion Swagger
            services.AddSwaggerGen(doc =>
            {
                doc.SwaggerDoc(configuration["Swagger:Version"], new OpenApiInfo
                {
                    Title = configuration["Swagger:Title"],
                    Version = configuration["Swagger:Version"]
                });

                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
                doc.IncludeXmlComments(xmlPath);

                doc.AddSecurityDefinition(configuration["Swagger:SecurityName"], new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,                    
                    In = ParameterLocation.Header,
                    Name = configuration["Swagger:HeaderName"],
                    Description = configuration["Swagger:DescriptionToken"],
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });


                doc.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                   }
                });
            });

            return services;
        }

        public static IServiceCollection AddCorsApp(this IServiceCollection services)
        {
            // Configuracion CORS
            services.AddCors(options =>
            {
                options.AddPolicy("ApiCors", builder =>
                {
                    builder
                    .AllowAnyOrigin()
                    // Esto no va en produccion, sólo local
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithExposedHeaders("Authorization"); // Expone el token para que las Apps lo puedan ver
                    // .AllowCredentials()
                });
            });

            return services;
        }

        public static IApplicationBuilder UseStatusCodeMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<StatusCodeMiddleware>();
        }

        public static IServiceCollection AddElasticLogging(this IServiceCollection services, IConfiguration configuration, string environment)
        {
            ConfigureLogging(configuration, environment);

            return services;
        }

        public static void ConfigureLogging(IConfiguration configuration, string environment)
        {
            // Configurar Serilog
            Log.Logger = new LoggerConfiguration()
                //.Enrich.WithElasticApmCorrelationInfo()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .MinimumLevel.Debug()
                //.MinimumLevel.Verbose()
                .WriteTo.Debug()
                .WriteTo.Console(new ElasticsearchJsonFormatter())
                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration))
                .Enrich.WithProperty("Environment", environment)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        public static ElasticsearchSinkOptions ConfigureElasticSink(IConfiguration configuration)
        {
            string urlElastic = configuration["ElasticConfig:Url"];
            //string templateName = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM}";

            return new ElasticsearchSinkOptions(new Uri(urlElastic))
            {
                AutoRegisterTemplate = true,
                IndexFormat = "logs-op360-obras-{0:yyyy.MM.dd}",
                //IndexFormat = templateName,
                NumberOfReplicas = 1,
                NumberOfShards = 2,
                //CustomFormatter = new EcsTextFormatter()
                CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true)
            };
        }
    }
}