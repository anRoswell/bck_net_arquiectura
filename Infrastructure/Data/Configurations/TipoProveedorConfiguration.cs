using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class TipoProveedorConfiguration : IEntityTypeConfiguration<TipoProveedor>
    {
        public void Configure(EntityTypeBuilder<TipoProveedor> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__TipoProv__3F2A1FFD1325F21B");

            builder.ToTable("TipoProveedor", "par");

            builder.Property(e => e.Id).HasColumnName("tipPrvIdTipoProveedor");

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

            builder.Property(e => e.TipPrvEstado)
                .IsRequired()
                .HasColumnName("tipPrvEstado")
                .HasDefaultValueSql("((1))")
                .HasComment("Estado del registro Activo - Inactivo");

            builder.Property(e => e.TipPrvNombreTipoProveedor)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("tipPrvNombreTipoProveedor")
                .HasComment("Descripcion del tipo de proveedor");
        }
    }
}
