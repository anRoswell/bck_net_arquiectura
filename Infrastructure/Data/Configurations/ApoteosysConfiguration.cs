using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ApoteosysConfiguration : IEntityTypeConfiguration<Apoteosys>
    {
        public void Configure(EntityTypeBuilder<Apoteosys> builder)
        {
            builder.HasNoKey();
        }
    }

    #region Facturas Por Pagar
    /*public class EstadoCuentasXPorPagarConfiguration : IEntityTypeConfiguration<EstadoCuentasXPorPagar>
    {
        public void Configure(EntityTypeBuilder<EstadoCuentasXPorPagar> builder)
        {
            builder.HasNoKey();
        }
    }*/

    public class EstadoCuentasXPorPagarDteConfiguration : IEntityTypeConfiguration<EstadoCuentasXPorPagarDte>
    {
        public void Configure(EntityTypeBuilder<EstadoCuentasXPorPagarDte> builder)
        {
            builder.HasNoKey();
        }
    }

    public class EstadoCuentasXPagarDetalleConfiguration : IEntityTypeConfiguration<EstadoCuentasXPagarDetalle>
    {
        public void Configure(EntityTypeBuilder<EstadoCuentasXPagarDetalle> builder)
        {
            builder.HasNoKey();
        }
    }
    #endregion

    #region Facturas Pagadas
    public class EstadoCuentasPagadasConfiguration : IEntityTypeConfiguration<EstadoCuentasPagadas>
    {
        public void Configure(EntityTypeBuilder<EstadoCuentasPagadas> builder)
        {
            builder.HasNoKey();
        }
    }

    public class EstadoCuentasPagadasDteConfiguration : IEntityTypeConfiguration<EstadoCuentasPagadasDte>
    {
        public void Configure(EntityTypeBuilder<EstadoCuentasPagadasDte> builder)
        {
            builder.HasNoKey();
        }
    }

    public class EstadoCuentasPagadasDetalleConfiguration : IEntityTypeConfiguration<EstadoCuentasPagadasDetalle>
    {
        public void Configure(EntityTypeBuilder<EstadoCuentasPagadasDetalle> builder)
        {
            builder.HasNoKey();
        }
    }

    public class EstadoCuentasPagadasDetalle_ReporteConfiguration : IEntityTypeConfiguration<EstadoCuentasPagadasDetalle_Reporte>
    {
        public void Configure(EntityTypeBuilder<EstadoCuentasPagadasDetalle_Reporte> builder)
        {
            builder.HasNoKey();
        }
    }

    #region Descuentos Aplicados en Facturas Pagadas
    public class EstadoCuentasXPagar_FactPagas_MaestroConfiguration : IEntityTypeConfiguration<EstadoCuentasXPagar_FactPagas_Maestro>
    {
        public void Configure(EntityTypeBuilder<EstadoCuentasXPagar_FactPagas_Maestro> builder)
        {
            builder.HasNoKey();
        }
    }

    public class EstadoCuentasXPagar_FactPagas_DetalleConfiguration : IEntityTypeConfiguration<EstadoCuentasXPagar_FactPagas_Detalle>
    {
        public void Configure(EntityTypeBuilder<EstadoCuentasXPagar_FactPagas_Detalle> builder)
        {
            builder.HasNoKey();
        }
    }
    #endregion

    #region Consultas desde SP SQL Server
    public class FacturasPagas_SQLConfiguration : IEntityTypeConfiguration<FacturasPagas_SQL>
    {
        public void Configure(EntityTypeBuilder<FacturasPagas_SQL> builder)
        {
            builder.HasNoKey();
        }
    }

    public class FacturasPagasConfiguration : IEntityTypeConfiguration<FacturasPagas>
    {
        public void Configure(EntityTypeBuilder<FacturasPagas> builder)
        {
            builder.HasNoKey();
        }
    }
    #endregion
    #endregion

    #region Certificados
    public class CertificadoRetencionMaestroConfiguration : IEntityTypeConfiguration<CertificadoRetencionMaestro>
    {
        public void Configure(EntityTypeBuilder<CertificadoRetencionMaestro> builder)
        {
            builder.HasNoKey();
        }
    }

    public class CertificadoExperienciaConfiguration : IEntityTypeConfiguration<CertificadoExperiencia>
    {
        public void Configure(EntityTypeBuilder<CertificadoExperiencia> builder)
        {
            builder.HasNoKey();
        }
    }

    public class CertificadoRetencionFuenteDteConfiguration : IEntityTypeConfiguration<CertificadoRetencionFuenteDte>
    {
        public void Configure(EntityTypeBuilder<CertificadoRetencionFuenteDte> builder)
        {
            builder.HasNoKey();
        }
    }

    public class CertificadoRetencionIvaDteConfiguration : IEntityTypeConfiguration<CertificadoRetencionIvaDte>
    {
        public void Configure(EntityTypeBuilder<CertificadoRetencionIvaDte> builder)
        {
            builder.HasNoKey();
        }
    }

    public class CertificadoRetencionIcaDteConfiguration : IEntityTypeConfiguration<CertificadoRetencionIcaDte>
    {
        public void Configure(EntityTypeBuilder<CertificadoRetencionIcaDte> builder)
        {
            builder.HasNoKey();
        }
    }

    public class CertificadoRetencionEstampillaBoyacaDteConfiguration : IEntityTypeConfiguration<CertificadoRetencionEstampillaBoyacaDte>
    {
        public void Configure(EntityTypeBuilder<CertificadoRetencionEstampillaBoyacaDte> builder)
        {
            builder.HasNoKey();
        }
    }
    #endregion

    #region Requerimientos
    public class SolicitudesApoteosysConfiguration : IEntityTypeConfiguration<SolicitudesApoteosys>
    {
        public void Configure(EntityTypeBuilder<SolicitudesApoteosys> builder)
        {
            builder.HasNoKey();
        }
    }
    #endregion
}
