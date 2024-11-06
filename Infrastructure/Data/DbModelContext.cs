using Core.CustomEntities;
using Core.Entities;
using Core.ModelResponse;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public partial class DbModelContext : DbContext
    {
        public DbModelContext()
        {
        }

        public DbModelContext(DbContextOptions<DbModelContext> options)
            : base(options)
        {
        }

        #region Propiedades Contexto
        #region Apoteosys
        public virtual DbSet<FacturasPagas> FacturasPagas { get; set; }
        #endregion

        public virtual DbSet<ResponseAction> ResponseActions { get; set; }
        public virtual DbSet<ResponseActionUrl> ResponseActionUrls { get; set; }
        public virtual DbSet<ResponseJsonString> ResponseJsonStrings { get; set; }
        public virtual DbSet<ParametrosIniciales> ParametrosIniciales { get; set; }
        public virtual DbSet<ActividadesPendientesContrato> ActividadesPendientesContratos { get; set; }
        public virtual DbSet<ParamsGenerale> ParamsGenerales { get; set; }
        public virtual DbSet<IdentidadesLocale> IdentidadesLocales { get; set; }
        public virtual DbSet<TipoCriterio> TipoCriterios { get; set; }
        public virtual DbSet<Aplicacion> Aplicaciones { get; set; }
        public virtual DbSet<AppsFileServerPath> AppsFileServerPaths { get; set; }
        public virtual DbSet<Ciudade> Ciudades { get; set; }
        public virtual DbSet<Departamento> Departamentos { get; set; }
        public virtual DbSet<Empresa> Empresas { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Perfil> Perfiles { get; set; }
        public virtual DbSet<PerfilesXusuario> PerfilesXusuarios { get; set; }
        public virtual DbSet<PermisosEmpresasxUsuario> PermisosEmpresasxUsuarios { get; set; }
        public virtual DbSet<PermisosMenuXperfil> PermisosMenuXperfils { get; set; }
        public virtual DbSet<PermisosUsuarioxMenu> PermisosUsuarioxMenus { get; set; }
        public virtual DbSet<PermisosXUsuario> PermisosXUsuario { get; set; }
        public virtual DbSet<PeticionesCors> PeticionesCors { get; set; }
        public virtual DbSet<RefererServidore> RefererServidores { get; set; }
        public virtual DbSet<TipoUsuario> TipoUsuarios { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<PerfilesXusuarioView> PerfilesXusuariosView { get; set; }
        public virtual DbSet<PrvProdServ> ProductoServicios { get; set; }
        public virtual DbSet<PrvSocio> PrvSocios { get; set; }
        public virtual DbSet<PrvReferencia> PrvReferencias { get; set; }
        public virtual DbSet<PrvDocumento> PrvDocumentos { get; set; }
        public virtual DbSet<PrvDocumentoDocReqProveedor> PrvDocumentoDocReqProveedors { get; set; }

        public virtual DbSet<TipoProveedor> TipoProveedors { get; set; }
        public virtual DbSet<PrvCondicionesPago> CondicionesPagos { get; set; }
        public virtual DbSet<ReqQuestionAnswer> ReqQuestionAnswers { get; set; }
        public virtual DbSet<Proveedores> Proveedores { get; set; }
        public virtual DbSet<Tercero> Terceros { get; set; }
        public virtual DbSet<Documento> Documento { get; set; }
        public virtual DbSet<Banco> Bancos { get; set; }
        public virtual DbSet<Notifications> Notifications { get; set; }
        public virtual DbSet<PrvEmpresasSelected> PrvEmpresasSelecteds { get; set; }
        public virtual DbSet<PrvProdServSelected> PrvProdServSelecteds { get; set; }
        public virtual DbSet<ProveedorInspektor> ProveedorInspektors { get; set; }
        public virtual DbSet<PrvListaRestrictiva> ListaRestrictivas { get; set; }
        public virtual DbSet<Estados> EstadosProveedor { get; set; }
        public virtual DbSet<Requerimientos> Requerimientos { get; set; }
        public virtual DbSet<ReqArtSerRequeridos> ReqArtSerRequeridos { get; set; }
        public virtual DbSet<ReqArtSerParticipante> ReqArtSerParticipantes { get; set; }
        public virtual DbSet<ParticipantesReq> ParticipantesReqs { get; set; }
        public virtual DbSet<ReqCriterosEvaluacion> ReqCriteriosEvaluacions { get; set; }
        public virtual DbSet<ReqRtaCriteriosEvaluacion> ReqRtaCriteriosEvaluacions { get; set; }
        public virtual DbSet<ReqCriterosEvaluacionRangoRta> ReqCriterosEvaluacionRangoRta { get; set; }
        public virtual DbSet<ReqPolizasSeguro> ReqPolizasSeguros { get; set; }
        public virtual DbSet<ReqListDocumentos> ReqListDocumentos { get; set; }
        public virtual DbSet<PathArchivosArtSerReq> PathArchivosArtSerReqs { get; set; }
        public virtual DbSet<PrvAdobeSign> PrvAdobeSigns { get; set; }
        public virtual DbSet<Noticia> Noticias { get; set; }
        public virtual DbSet<TiposPlantilla> TiposPlantillas { get; set; }
        public virtual DbSet<TipoNoticia> TipoNoticias { get; set; }
        public virtual DbSet<Alcance> Alcances { get; set; }
        public virtual DbSet<OrdenesMaestras> OrdenesMaestras { get; set; }
        public virtual DbSet<CertificadosEspeciale> CertificadosEspeciales { get; set; }
        public virtual DbSet<TipoEstado> TipoEstados { get; set; }
        public virtual DbSet<ReqArtSerRequeridosDoc> ReqArtSerRequeridosDocs { get; set; }
        public virtual DbSet<ReqTipoDocArtSerRequerido> ReqTipoDocArtSerRequeridos { get; set; }
        public virtual DbSet<Estados> EstadosRequerimiento { get; set; }
        public virtual DbSet<UnidadMedida> UnidadMedidas { get; set; }
        public virtual DbSet<ReqTipoDocArtSerRequerido> ReqTipoDocArtSerRequerido { get; set; }
        public virtual DbSet<NoticiasDoc> NoticiasDocs { get; set; }
        public virtual DbSet<Contrato> Contratos { get; set; }
        public virtual DbSet<ClaseContrato> ClaseContratos { get; set; }
        public virtual DbSet<FormasPago> FormasPagos { get; set; }
        public virtual DbSet<Sino> Sinos { get; set; }
        public virtual DbSet<UnidadNegocio> UnidadNegocios { get; set; }
        public virtual DbSet<TiposCertificado> TiposCertificados { get; set; }
        public virtual DbSet<TiposCertificadoEspeciale> TiposCertificadoEspeciales { get; set; }
        public virtual DbSet<EmpresasSelectedCertEsp> EmpresasSelectedCertEsps { get; set; }
        public virtual DbSet<RespuestasCertEspeciale> RespuestasCertEspeciales { get; set; }
        public virtual DbSet<OrdenReq> OrdenReqs { get; set; }
        public virtual DbSet<DocumentoContrato> DocumentosContrato { get; set; }
        public virtual DbSet<HashCertifiedValidation> HashCertifiedValidations { get; set; }
        public virtual DbSet<PathsPortal> PathsPortals { get; set; }
        public virtual DbSet<ArticulosRequerimiento> ArticulosRequerimientos { get; set; }
        public virtual DbSet<TipoRequerimiento> TipoRequerimientos { get; set; }
        public virtual DbSet<AdjudicacionOrdenes> AdjudicacionOrdenes { get; set; }
        public virtual DbSet<ReqAdjudicacion> ReqAdjudicacions { get; set; }
        public virtual DbSet<ReqAdjudicacionDetalle> ReqAdjudicacionDetalles { get; set; }
        public virtual DbSet<AprobadoresContrato> AprobadoresContratos { get; set; }
        public virtual DbSet<TipoAprobadoresContrato> TipoAprobadoresContratos { get; set; }
        public virtual DbSet<PasosContrato> PasosContrato { get; set; }
        public virtual DbSet<TipoPasosContrato> TipoPasosContrato { get; set; }
        public virtual DbSet<FlujosContrato> FlujosContrato { get; set; }

        public virtual DbSet<DocReqPoliza> DocReqPolizas { get; set; }
        public virtual DbSet<DocReqProveedor> DocReqProveedors { get; set; }
        public virtual DbSet<DocReqProveedorOtro> DocReqProveedorOtros { get; set; }
        public virtual DbSet<DocReqUpload> DocReqUploads { get; set; }
        public virtual DbSet<SeguimientosContrato> SeguimientosContratos { get; set; }

        public virtual DbSet<AprobadoresContratoHistorico> AprobadoresContratoHistoricos { get; set; }
        public virtual DbSet<ContratoHistorico> ContratoHistoricos { get; set; }
        public virtual DbSet<DocReqPolizaHistorico> DocReqPolizaHistoricos { get; set; }
        public virtual DbSet<DocReqProveedorHistorico> DocReqProveedorHistoricos { get; set; }
        public virtual DbSet<DocReqProveedorOtrosHistorico> DocReqProveedorOtrosHistoricos { get; set; }
        public virtual DbSet<DocumentoContratoHistorico> DocumentoContratoHistoricos { get; set; }
        public virtual DbSet<HistoricosProrroga> HistoricosProrroga { get; set; }
        public virtual DbSet<TipoMinuta> TipoMinuta { get; set; }
        public virtual DbSet<TipoContrato> TipoContrato { get; set; }
        public virtual DbSet<RepresentantesLegalEmpresa> RepresentantesLegalEmpresas { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.ApplyAllConfigurations();

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
