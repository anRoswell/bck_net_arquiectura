using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    class EstadosConfiguration : IEntityTypeConfiguration<Estados>
    {
        public void Configure(EntityTypeBuilder<Estados> builder)
        {
            builder.HasKey(e => e.Id);

            builder.ToTable("Estados", "par");

            builder.Property(e => e.Id)
                .HasColumnName("parIdEstado")
                .HasComment("Id autoincrementable ");

            builder.Property(e => e.CodTipoEstado)
                .HasDefaultValueSql("((1))")
                .HasComment("Indica el tipo de estado al q pertenece, pueden ser proveedores, requerimientos, noticias, ordenes, contratos, entre otros");

            builder.Property(e => e.CodUser)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7|7|7')");

            builder.Property(e => e.CodUserUpdate)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7|7|7')");

            builder.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.FechaRegistroUpdate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            builder.Property(e => e.Info)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')");

            builder.Property(e => e.InfoUpdate)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')");

            builder.Property(e => e.ParDescripcion)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("parDescripcion")
                .HasComment("Descripción del estado");

            builder.Property(e => e.ParvEstado)
                .IsRequired()
                .HasColumnName("parvEstado")
                .HasDefaultValueSql("((1))")
                .HasComment("Estado en q se encuentra el estado, activo - inactivo");
        }
    }
}
