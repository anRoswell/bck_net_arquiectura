using Core.Entities;
using Core.Entities.SCRWebEntities;
using System;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    /// <summary>
    /// Interfaz encargada de indicar los atributos y métodos que la unidad de trabajo debe implementar
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IRepository<AppsFileServerPath> AppsFileServerPathRepository { get; }

        IRepository<PeticionesCors> PeticionesCorsRepository { get; }
        
        IRepositoryOracle<gnl_peticiones_cors> PeticionesCorsOracleRepository { get; }
        IRepositoryOracle<gnl_rutas_archivo_servidor> RutasArchivoServidorOracleRepository { get; }
        IRepositoryOracle2<gnl_actividades> ActividadesOracleRepository { get; }
        IRepositoryOracle2<gnl_tipos_soporte> TiposSoporteOracleRepository { get; }
        IRepositoryOracle<gnl_soportes> Gnl_SoportesOracleRepository { get; }

        IPlantillaRepository PlantillaRepository { get; }
        
        IRefererServidoresRepository RefererServidoresRepository { get; }
        IRefererServidoresRepositoryOracle RefererServidoresRepositoryOracle { get; }

        IUsuarioRepository UsuarioRepository { get; }
        IPerfilRepository PerfilesRepository { get; }
        IPerfilesXusuarioRepository PerfilesXusuarioRepository { get; }
        IPermisosEmpresasxUsuarioRepository PermisosEmpresasxUsuarioRepository { get; }
        IPermisosMenuXPerfilRepository PermisosMenuXPerfilRepository { get; }
        IPermisosUsuarioxMenuRepository PermisosUsuarioxMenuRepository { get; }
        IParametrosInicialesRepository ParametrosInicialesRepository { get; }

        IProveedorRepository ProveedorRepository { get; }
        IReqQuestionAnswerRepository ReqQuestionAnswerRepository { get; }
        INotificationsRepository NotificationsRepository { get; }
        IMenuRepository MenusRepository { get; }
        IPrvSocioRepository PrvSociosRepository { get; }
        IPrvReferenciaRepository PrvReferenciaRepository {get;}
        IPrvEmpresasSelectedRepository PrvEmpresasSelectedRepository { get; }
        IPrvProdServSelectedRepository PrvProdServSelectedRepository { get; }

        IOrdenesMaestrasRepository OrdenesMaestrasRepository { get; }

        IPrvDocumentoRepository PrvDocumentoRepository { get; }
        
        /*
        * fecha: 19/12/2023
        * clave: 5e6ewrt546weasdf _05
        * carlos vargas
        */
        IOp360Repository Op360Repository { get; }        

        IAdobeSignRepository AdobeSignRepository { get; }
        IApoteosysRepository ApoteosysRepository { get; }
        ISispoRepository SispoRepository { get; }
        IRequerimientosRepository RequerimientosRepository { get; }
        INoticiaRepository NoticiaRepository { get; }
        ICertificadosEspecialesRepository CertificadosEspecialesRepository { get; }
        INotEmpresaSelected NotEmpresaSelected { get; }
        INotTipoPlantilla NotTipoPlantilla { get; }
        INoticiasDocRepository NoticiasDocRepository { get; }
        IContratoRepository ContratoRepository { get; }
        IClaseContratoRepository ClaseContratoRepository { get; }
        IFormasPagoRepository FormasPagosRepository { get; }
        ISinoRepository SinosRepository { get; }
        ILogErroresRepository LogErroresRepository { get; }
        IEmpresasSelectedCertEspRepository EmpresasSelectedCertEspRepository { get; }
        IEmpresasRepository EmpresasRepository { get; }
        IDocumentoRepository DocumentoRepository { get; }
        IUnidadesNegocioRepository UnidadesNegocio { get; }
        ITipoMinutaRepository TipoMinutaRepository { get; }
        ITerceroRepository TerceroRepository { get; }
        IRepresentantesLegalEmpresaReporitory RepresentantesLegalEmpresaReporitory { get; }
        void SaveChanges();
        Task SaveChangesAsync();

        #region Store Procedure Repositories
        public IStoreProcedureRepository<T> StoreProcedure<T>() where T : class;
        #endregion

        #region Excel Generador Repositories
        public IExcelGeneradorRepository<T> ExcelGenerador<T>() where T : class;
        #endregion

        #region Gos Repositories
        IRepositoryOracle<gos_soporte> GosSoporteRepository { get; }
        #endregion

        IRepositoryOracle<sgr_auth_tokens> SgrAuthTokensRepository { get; }


        Task SaveChangesOracleAsync();
    }
}
