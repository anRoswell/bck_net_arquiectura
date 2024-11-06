using AutoMapper;
using Core.DTOs.FilesDto;
using Core.Entities;
using Core.Entities.SCRWebEntities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Clase encargada de guardar los cambios en la Base de Datos
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbModelContext _context;
        private readonly DbOracleContext _oracleContext;
        private readonly DbSispoContext _sispoContext;

        /*
        * fecha: 19/12/2023
        * clave: asd6f4sad65f4sad4f6asd65f65as6f
        * carlos vargas
        */
        private readonly DbAireContext _aireContext;

        private readonly IMapper _mapper;
        private readonly IDbConnection _dapperSource;

        private readonly IRepository<AppsFileServerPath> _appsFileServerPathRepository;

        private readonly IRepository<PeticionesCors> _peticionesCorsRepository;
        private readonly IRepositoryOracle<gnl_peticiones_cors> _peticionesCorsOracleRepository;
        private readonly IRepositoryOracle<gnl_rutas_archivo_servidor> _rutasArchivoServidorOracleRepository;

        private readonly IRepositoryOracle2<gnl_actividades> _actividadesOracleRepository;
        private readonly IRepositoryOracle2<gnl_tipos_soporte> _tiposSoporteOracleRepository;

        private readonly IRepositoryOracle<gnl_soportes> _gnlsoportesOracleRepository;

        private readonly IPlantillaRepository _plantillaRepository;
        private readonly IRepository<Menu> _menuRepository;
        private readonly IRefererServidoresRepository _refererHostRepository;
        private readonly IRefererServidoresRepositoryOracle _refererHostRepositoryOracle;

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPerfilRepository _perfilesRepository;
        private readonly IPerfilesXusuarioRepository _perfilesXusuarioRepository;
        private readonly IPermisosEmpresasxUsuarioRepository _permisosEmpresasxUsuarioRepository;
        private readonly IPermisosMenuXPerfilRepository _permisosMenuXPerfilRepository;
        private readonly IPermisosUsuarioxMenuRepository _permisosUsuarioxMenuRepository;
        private readonly IParametrosInicialesRepository _parametrosInicialesRepository;
        private readonly IProveedorRepository _proveedoresRepository;
        private readonly ITerceroRepository _tercerosRepository;
        private readonly IReqQuestionAnswerRepository _reqQuestionAnswerRepository;
        private readonly INotificationsRepository _notificationsRepository;
        private readonly IMenuRepository _menusRepository;
        private readonly IPrvSocioRepository _prvSociosRepository;
        private readonly IPrvReferenciaRepository _prvReferenciaRepository;
        private readonly IPrvEmpresasSelectedRepository _prvEmpresasSelectedRepository;
        private readonly IPrvDocumentoRepository _prvDocumentoRepository;
        private readonly IPrvProdServSelectedRepository _prodServSelectedRepository;

        /*
        * fecha: 19/12/2023
        * clave: 5e6ewrt546weasdf _06
        * carlos vargas
        */
        private readonly IOp360Repository _op360Repository;        

        private readonly IApoteosysRepository _apoteosysRepository;
        private readonly ISispoRepository _sispoRepository;
        private readonly IAdobeSignRepository _adobeSignRepository;
        private readonly IRequerimientosRepository _requerimientosRepository;
        private readonly INoticiaRepository _noticiaRepository;

        private readonly IOrdenesMaestrasRepository _ordenesMaestrasRepository;
        private readonly ICertificadosEspecialesRepository _certificadosEspecialesRepository;
        private readonly INoticiasDocRepository _noticiasDocRepository;
        private readonly IContratoRepository _contratoRepository;
        private readonly IClaseContratoRepository _claseContratoRepository;
        private readonly IFormasPagoRepository _formasPagoRepository;
        private readonly ISinoRepository _sinoRepository;
        private readonly ILogErroresRepository _logErroresRepository;
        private readonly IEmpresasSelectedCertEspRepository _empresasSelectedCertEspRepository;
        private readonly IEmpresasRepository _empresasRepository;
        private readonly IDocumentoRepository _documentoRepository;
        private readonly IUnidadesNegocioRepository _unidadesNegocioRepository;
        private readonly IOptions<Core.Options.PasosContratosOptions> _pasosContratosOptions;
        private readonly IOptions<Core.Options.PerfilesOptions> _perfilesOptions;
        private readonly ITipoMinutaRepository _tipoMinutaRepository;
        private readonly IRepresentantesLegalEmpresaReporitory _representantesLegalEmpresaReporitory;
        private readonly Dictionary<Type, object> _storeProcedureRepositories = new();

        #region Private Gos Repositories
        private readonly IRepositoryOracle<gos_soporte> _gosSoporteRepository;
        #endregion

        private readonly IRepositoryOracle<sgr_auth_tokens> _sgrAuthTokensRepository;

        /*
        * fecha: 19/12/2023
        * clave: asd6f4sad65f4sad4f6asd65f65as6f
        * carlos vargas
        */
        public UnitOfWork(
            DbModelContext context, 
            DbOracleContext oracleContext, 
            DbSispoContext sispoContext,
            IDbConnection dapperSource, 
            IMapper mapper, 
            IOptions<Core.Options.PasosContratosOptions> pasosContratosOptions, 
            IOptions<Core.Options.PerfilesOptions> perfilesOptions,
            DbAireContext aireContext
            )
        {
            _context = context;
            _oracleContext = oracleContext;
            _sispoContext = sispoContext;
            _pasosContratosOptions = pasosContratosOptions;
            _perfilesOptions = perfilesOptions;
            _mapper = mapper;
            _dapperSource = dapperSource;
            _aireContext = aireContext;
        }

        //public IPostRepository PostRepository => _postRepository ?? new PostRepository(_context);x

        //public IRepository<Entity> UserRepository => _userRepository ?? new BaseRepository<User>(_context);
        public IRepository<AppsFileServerPath> AppsFileServerPathRepository => _appsFileServerPathRepository ?? new BaseRepository<AppsFileServerPath>(_context);

        public IRepository<PeticionesCors> PeticionesCorsRepository => _peticionesCorsRepository ?? new BaseRepository<PeticionesCors>(_context);
        public IRepositoryOracle<gnl_peticiones_cors> PeticionesCorsOracleRepository => _peticionesCorsOracleRepository ?? new BaseRepositoryOracle<gnl_peticiones_cors>(_aireContext);
        public IRepositoryOracle<gnl_rutas_archivo_servidor> RutasArchivoServidorOracleRepository => _rutasArchivoServidorOracleRepository ?? new BaseRepositoryOracle<gnl_rutas_archivo_servidor>(_aireContext);

        public IRepositoryOracle2<gnl_actividades> ActividadesOracleRepository => _actividadesOracleRepository ?? new BaseRepositoryOracle2<gnl_actividades>(_aireContext);
        public IRepositoryOracle2<gnl_tipos_soporte> TiposSoporteOracleRepository => _tiposSoporteOracleRepository ?? new BaseRepositoryOracle2<gnl_tipos_soporte>(_aireContext);

        public IRepositoryOracle<gnl_soportes> Gnl_SoportesOracleRepository => _gnlsoportesOracleRepository ?? new BaseRepositoryOracle<gnl_soportes>(_aireContext);

        public IPlantillaRepository PlantillaRepository => _plantillaRepository ?? new PlantillaRepository(_context);
        public IRepository<Menu> MenuRepository => _menuRepository ?? new BaseRepository<Menu>(_context);
        
        public IRefererServidoresRepository RefererServidoresRepository => _refererHostRepository ?? new RefererServidoresRepository(_context);
        public IRefererServidoresRepositoryOracle RefererServidoresRepositoryOracle => _refererHostRepositoryOracle ?? new RefererServidoresRepositoryOracle(_aireContext);

        public IUsuarioRepository UsuarioRepository => _usuarioRepository ?? new UsuarioRepository(_context);
        public IPerfilRepository PerfilesRepository => _perfilesRepository ?? new PerfilRepository(_context);
        public IPerfilesXusuarioRepository PerfilesXusuarioRepository => _perfilesXusuarioRepository ?? new PerfilesXusuarioRepository(_context);
        public IPermisosEmpresasxUsuarioRepository PermisosEmpresasxUsuarioRepository => _permisosEmpresasxUsuarioRepository ?? new PermisosEmpresasxUsuarioRepository(_context);
        public IPermisosMenuXPerfilRepository PermisosMenuXPerfilRepository => _permisosMenuXPerfilRepository ?? new PermisosMenuXPerfilRepository(_context);
        public IPermisosMenuXPerfilRepository PermisosXMenuRepository => _permisosMenuXPerfilRepository ?? new PermisosMenuXPerfilRepository(_context);
        public IPermisosUsuarioxMenuRepository PermisosUsuarioxMenuRepository => _permisosUsuarioxMenuRepository ?? new PermisosUsuarioxMenuRepository(_context);
        public IParametrosInicialesRepository ParametrosInicialesRepository => _parametrosInicialesRepository ?? new ParametrosInicialesRepository(_context);
        public IReqQuestionAnswerRepository ReqQuestionAnswerRepository => _reqQuestionAnswerRepository ?? new ReqQuestionAnswerRepository(_context);
        public IProveedorRepository ProveedorRepository => _proveedoresRepository ?? new ProveedorRepository(_context, _perfilesOptions);
        public INotificationsRepository NotificationsRepository => _notificationsRepository ?? new NotificationsRepository(_context);
        public IMenuRepository MenusRepository => _menusRepository ?? new MenuRepository(_context);
        public IPrvSocioRepository PrvSociosRepository => _prvSociosRepository ?? new PrvSocioRepository(_context);
        public IPrvReferenciaRepository PrvReferenciaRepository => _prvReferenciaRepository ?? new PrvReferenciaRepository(_context);
        public IPrvEmpresasSelectedRepository PrvEmpresasSelectedRepository => _prvEmpresasSelectedRepository ?? new PrvEmpresasSelectedRepository(_context);
        public IPrvProdServSelectedRepository PrvProdServSelectedRepository => _prodServSelectedRepository ?? new PrvProdServSelectedRepository(_context);
        public IPrvDocumentoRepository PrvDocumentoRepository => _prvDocumentoRepository ?? new PrvDocumentoRepository(_context);
        
        /*
        * fecha: 19/12/2023
        * clave: 5e6ewrt546weasdf _06
        * carlos vargas
        */
        public IOp360Repository Op360Repository => _op360Repository ?? new Op360Repository(_aireContext);        

        public IApoteosysRepository ApoteosysRepository => _apoteosysRepository ?? new ApoteosysRepository(_oracleContext);

        public ISispoRepository SispoRepository => _sispoRepository ?? new SispoRepository(_sispoContext);

        public IAdobeSignRepository AdobeSignRepository => _adobeSignRepository ?? new AdobeSignRepository(_context);

        public IRequerimientosRepository RequerimientosRepository => _requerimientosRepository ?? new RequerimientosRepository(_context);

        public INoticiaRepository NoticiaRepository => _noticiaRepository ?? new NoticiaRepository(_context);

        public INotEmpresaSelected NotEmpresaSelected => throw new System.NotImplementedException();

        public INotTipoPlantilla NotTipoPlantilla => throw new System.NotImplementedException();

        public IOrdenesMaestrasRepository OrdenesMaestrasRepository => _ordenesMaestrasRepository ?? new OrdenesMaestrasRepository(_context);

        public ICertificadosEspecialesRepository CertificadosEspecialesRepository => _certificadosEspecialesRepository ?? new CertificadosEspecialesRepository(_context);

        public INoticiasDocRepository NoticiasDocRepository => _noticiasDocRepository ?? new NoticiasDocRepository(_context);
        public IEmpresasRepository EmpresasRepository => _empresasRepository ?? new EmpresasRepository(_context);
        public IContratoRepository ContratoRepository => _contratoRepository ?? new ContratoRepository(_context, _pasosContratosOptions, _dapperSource);
        public IClaseContratoRepository ClaseContratoRepository => _claseContratoRepository ?? new ClaseContratoRepository(_context);
        public IFormasPagoRepository FormasPagosRepository => _formasPagoRepository ?? new FormasPagoRespository(_context);
        public ISinoRepository SinosRepository => _sinoRepository ?? new SinoRepository(_context);

        public ILogErroresRepository LogErroresRepository => _logErroresRepository ?? new LogErroresRepository(_aireContext);

        public IEmpresasSelectedCertEspRepository EmpresasSelectedCertEspRepository => _empresasSelectedCertEspRepository ?? new EmpresasSelectedCertEspRepository(_context);

        public IDocumentoRepository DocumentoRepository => _documentoRepository ?? new DocumentoRepository(_context);

        public IUnidadesNegocioRepository UnidadesNegocio => _unidadesNegocioRepository ?? new UnidadesNegocioRepository(_context, _mapper);

        public ITipoMinutaRepository TipoMinutaRepository => _tipoMinutaRepository ?? new TipoMinutaRepository(_context);
        public ITerceroRepository TerceroRepository => _tercerosRepository ?? new TerceroRepository(_context, _dapperSource, _perfilesOptions);
        public IRepresentantesLegalEmpresaReporitory RepresentantesLegalEmpresaReporitory => _representantesLegalEmpresaReporitory ?? new RepresentantesLegalEmpresaReporitory(_context);

        #region Gos Repositories
        public IRepositoryOracle<gos_soporte> GosSoporteRepository => _gosSoporteRepository ?? new BaseRepositoryOracle<gos_soporte>(_aireContext);
        #endregion

        public IRepositoryOracle<sgr_auth_tokens> SgrAuthTokensRepository => _sgrAuthTokensRepository ?? new BaseRepositoryOracle<sgr_auth_tokens>(_aireContext);


        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        #region Store Procedure Repositories
        public IStoreProcedureRepository<T> StoreProcedure<T>() where T : class
        {
            var type = typeof(StoreProcedureRepository<T>);

            if (!_storeProcedureRepositories.ContainsKey(type))
            {
                _storeProcedureRepositories[type] = new StoreProcedureRepository<T>(_aireContext);
            }

            return (IStoreProcedureRepository<T>)_storeProcedureRepositories[type];
        }
        #endregion

        #region Excel Generador Repositories
        public IExcelGeneradorRepository<T> ExcelGenerador<T>() where T : class
        {
            return new ExcelGeneradorRepository<T>();
        }
        #endregion

        public async Task SaveChangesOracleAsync()
        {
            await _aireContext.SaveChangesAsync();
        }
    }
}