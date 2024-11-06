using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    class RequerimientosConfiguration : IEntityTypeConfiguration<Requerimientos>
    {
        public void Configure(EntityTypeBuilder<Requerimientos> builder)
        {
            builder.HasNoKey();

            //builder.HasKey(e => e.ReqIdRequerimientos)
            //        .HasName("PK__Requerim__B6C121E02AB7CB32");

            builder.ToTable("Requerimientos", "req");

            builder.Property(e => e.Id).HasColumnName("reqIdRequerimientos");

            builder.Property(e => e.CodUser)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7777777')")
                .HasComment("Cedula del usuario que crea el registro");

            builder.Property(e => e.CodUserUpdate)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7777777')")
                .HasComment("Cedula del ultimo usuario que actualizó el registro");

            builder.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha de creación del registro.");

            builder.Property(e => e.FechaRegistroUpdate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha de la ultima actualización del registro.");

            builder.Property(e => e.Info)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')")
                .HasComment("En este campo almacenamos la direccion ip, navegador y version del navegador del cliente.");

            builder.Property(e => e.InfoUpdate)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')")
                .HasComment("En este campo almacenamos la ultima direccion ip, navegador y version del navegador del cliente.");

            builder.Property(e => e.ReqCierraOferta)
                .HasColumnType("datetime")
                .HasColumnName("reqCierraOferta")
                .HasComment("Fecha de cierre de la oferta");

            builder.Property(e => e.ReqCodAgencia)
                .HasColumnName("reqCodAgencia")
                .HasComment("Id de la agencia seleccionada");

            builder.Property(e => e.ReqCodEmpresa)
                .HasColumnName("reqCodEmpresa")
                .HasComment("Empresa que requiere los productos o servicios: lista desplegable con las empresas de Urbaser Colombia");

            builder.Property(e => e.ReqCodGestorCompras)
                .HasColumnName("reqCodGestorCompras")
                .HasComment("Id del gesor de compras asignado");

            builder.Property(e => e.ReqCodGestorContrato)
                .HasColumnName("reqCodGestorContrato")
                .HasComment("Id del gestor de contrato, si aplica");

            builder.Property(e => e.ReqCompraPrevista)
                .HasColumnType("datetime")
                .HasColumnName("reqCompraPrevista")
                .HasComment("Fecha de compra prevista");

            builder.Property(e => e.ReqDescription)
                .IsRequired()
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("reqDescription")
                .HasComment("Campo de texto enriquecido mediante el cual se puede describir de forma general el requerimiento");

            builder.Property(e => e.ReqEstado)
                .HasColumnName("reqEstado")
                .HasComment("Estado en que se encuentra el registro");

            builder.Property(e => e.ReqFechaEntrega)
                .HasColumnType("datetime")
                .HasColumnName("reqFechaEntrega")
                .HasComment("Fecha de entrega prevista");

            builder.Property(e => e.ReqGarantiasExigidas)
                .IsRequired()
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("reqGarantiasExigidas")
                .HasComment("Texto en WYSING con las garatias exigidas");

            builder.Property(e => e.ReqLugarEntrega)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("reqLugarEntrega")
                .HasComment("Lugar de entrega");

            builder.Property(e => e.ReqNombreCentroCosto)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("reqNombreCentroCosto")
                .HasComment("Nombre");

            builder.Property(e => e.ReqReqType)
                .HasColumnName("reqReqType")
                .HasComment("Indica si es una orden de compra o de servicio");

            builder.Property(e => e.ReqRequiereContrato)
                .HasColumnName("reqRequiereContrato")
                .HasComment("Indica si la requisicion requiere o no contrato");

            builder.Property(e => e.ReqfinOfertaPresentada)
                .HasColumnType("datetime")
                .HasColumnName("reqfinOfertaPresentada")
                .HasComment("Fecha en que vence la oferta presentada");
        }
    }
}
