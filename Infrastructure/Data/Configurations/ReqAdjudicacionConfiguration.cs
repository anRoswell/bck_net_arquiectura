using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ReqAdjudicacionConfiguration : IEntityTypeConfiguration<ReqAdjudicacion>
    {
        public void Configure(EntityTypeBuilder<ReqAdjudicacion> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__ReqAdjud__5EF72BCA6E86B962");

            builder.ToTable("ReqAdjudicacion", "req");

            builder.HasIndex(e => e.RadjCodRequerimiento, "IX_Unique_CodRequerimiento")
                .IsUnique();

            builder.Property(e => e.Id).HasColumnName("radjIdReqAdjudicacion");

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

            builder.Property(e => e.RadjCodGestorContrato)
                .HasColumnName("radjCodGestorContrato")
                .HasComment("Codigo del usuario gestor para el contrato");

            builder.Property(e => e.RadjCodProveedorSelecTotal)
                .HasColumnName("radjCodProveedorSelecTotal")
                .HasComment("Codigo del proveedor seleccionado en adjudicacion 'Total'");

            builder.Property(e => e.RadjCodRequerimiento)
                .HasColumnName("radjCodRequerimiento")
                .HasComment("Codigo del Requerimiento Adjudicado");

            builder.Property(e => e.RadjCodRequisitorContrato)
                .HasColumnName("radjCodRequisitorContrato")
                .HasComment("Codigo del usuario requisitor para el contrato");

            builder.Property(e => e.RadjIsGuardadoTemporal)
                .HasColumnName("radjIsGuardadoTemporal")
                .HasDefaultValueSql("((0))")
                .HasComment("Indica si es una adjudicacion de guardado temporal");

            builder.Property(e => e.RadjEstado)
                .IsRequired()
                .HasColumnName("radjEstado")
                .HasDefaultValueSql("((1))")
                .HasComment("Estado de la adjudicacion");

            builder.Property(e => e.RadjTipoAdjudicacion)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("radjTipoAdjudicacion")
                .HasComment("Tipo de Adjudicacíon");
        }
    }
}
