using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class PermisosEmpresasXUsuarioConfiguration : IEntityTypeConfiguration<PermisosEmpresasxUsuario>
    {
        public void Configure(EntityTypeBuilder<PermisosEmpresasxUsuario> builder)
        {
            builder.ToTable("PermisosEmpresasxUsuario", "usr");

            builder.HasKey(e => e.Id)
                .HasName("PK__Permisos__7A7F26590D53AACC");

            builder.Property(e => e.Id)
            .HasColumnName("Peu_IdPermisosEmpresasxUsuario");

            builder.Property(e => e.CodArchivo)
                .IsRequired()
                .HasMaxLength(450)
                .HasComment("Se carga una imagen con el logotipo de la empresa, esta debe de ser de maximo 128x128. Conecta con la tabla maestra de archivos.");

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

            builder.Property(e => e.PeuEmpCodEmpresa)
                .HasColumnName("Peu_Emp_CodEmpresa")
                .HasComment("Codigo de Empresa");

            builder.Property(e => e.PeuEstado)
                .IsRequired()
                .HasColumnName("Peu_Estado")
                .HasDefaultValueSql("((1))")
                .HasComment("Estado del registro");

            builder.Property(e => e.PeuUsrCodUsuario)
                .HasColumnName("Peu_Usr_CodUsuario")
                .HasComment("Codigo de usuario");

            //builder.HasOne(d => d.PeuEmpCodEmpresaNavigation)
            //    .WithMany(p => p.PermisosEmpresasxUsuarios)
            //    .HasForeignKey(d => d.PeuEmpCodEmpresa)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_PermisosEmpresasxUsuario_Empresas");

            //builder.HasOne(d => d.PeuUsrCodUsuarioNavigation)
            //    .WithMany(p => p.PermisosEmpresasxUsuarios)
            //    .HasForeignKey(d => d.PeuUsrCodUsuario)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_PermisosEmpresasxUsuario_Usuario");
        }
    }
}
