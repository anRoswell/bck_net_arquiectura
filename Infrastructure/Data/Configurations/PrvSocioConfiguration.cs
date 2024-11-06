using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class PrvSocioConfiguration : IEntityTypeConfiguration<PrvSocio>
    {
        public void Configure(EntityTypeBuilder<PrvSocio> builder)
        {
            builder.HasKey(e => e.Id)
                        .HasName("PK__PrvSocio__C7504AB2EED62747");

            builder.ToTable("PrvSocios", "prv");

            builder.Property(e => e.Id)
                .HasColumnName("socIdPrvSocios")
                .HasComment("Id socios");

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

            builder.Property(e => e.SocCodCiudad).HasColumnName("socCodCiudad");

            builder.Property(e => e.SocDireccion)
                        .IsRequired()
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnName("socDireccion");

            builder.Property(e => e.SocIdentificacion)
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnName("socIdentificacion")
                        .HasComment("Identificación");

            builder.Property(e => e.SocDigVerificacion)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("socDigVerificacion");

            builder.Property(e => e.SocNombre)
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnName("socNombre");

            //builder.HasOne(d => d.CodProveedorNavigation)
            //            .WithMany(p => p.PrvSocios)
            //            .HasForeignKey(d => d.CodProveedor)
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK_PrvSocios_Proveedores");
        }
    }
}
