using Core.Entities;
using Core.Entities.SCRWebEntities;
using Core.QueryFilters;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    /*
    * fecha: 19/12/2023
    * clave: asd6f4sad65f4sad4f6asd65f65as6f
    * carlos vargas
    */
    public class DbAireContext : DbContext
    {
        public DbAireContext()
        {
        }

        public DbAireContext(DbContextOptions<DbAireContext> options)
            : base(options)
        {
        }
        public virtual DbSet<OrdenesSCR> ordenesSCR { get; set; }
        /*
        * fecha: 19/12/2023
        * clave: 5e6ewrt546weasdf _12
        * carlos vargas
        */
        public virtual DbSet<Usuarios_Perfiles> usuariosPerfiles { get; set; }

        /*
        * fecha: 19/12/2023
        * clave: aaaaORDENESa65sd4f65sdf _12
        * carlos vargas
        */
        public virtual DbSet<Aire_Scr_OrdenConGeorreferencia> aire_scr_Orden { get; set; }
        public virtual DbSet<gnl_peticiones_cors> gnl_peticiones_cors { get; set; }
        public virtual DbSet<gnl_peticiones_url_origen> gnl_peticiones_url_origen { get; set; }
        
        public virtual DbSet<gnl_rutas_archivo_servidor> gnl_rutas_archivo_servidor { get; set; }
        public virtual DbSet<gnl_actividades> gnl_actividades { get; set; }
        public virtual DbSet<gnl_tipos_soporte> gnl_tipos_soporte { get; set; }

        public virtual DbSet<gnl_soportes> gnl_soportes { get; set; }
        public virtual DbSet<SequencesResponseOracle> SequencesResponseOracle { get; set; }
        public virtual DbSet<Op360GuardarOrden> Op360GuardarOrdenes { get; set; }

        #region Entities Gos
        public virtual DbSet<gos_soporte> gos_soporte { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //object value = modelBuilder.ForOracleHasDefaultValueSql("asdasd");

            modelBuilder.ApplyAllConfigurations();
        }
    }
}

