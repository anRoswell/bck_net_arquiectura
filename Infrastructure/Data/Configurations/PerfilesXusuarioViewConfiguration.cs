using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    class PerfilesXusuarioViewConfiguration : IEntityTypeConfiguration<PerfilesXusuarioView>
    {
        public void Configure(EntityTypeBuilder<PerfilesXusuarioView> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__Perfiles__A6E8FE4D7DD8D933");

          

            builder.Property(e => e.Id)
            .HasColumnName("Pxu_IdPerfilesXUsuarios");

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

            builder.Property(e => e.PxuAplCodAplicacion)
                .HasColumnName("Pxu_Apl_CodAplicacion")
                .HasComment("Codigo de aplicación");

            builder.Property(e => e.PxuEstado)
                .HasColumnName("Pxu_Estado")
                .HasComment("Estado del registro");

            builder.Property(e => e.PxuPrfCodPerfil)
                .HasColumnName("Pxu_Prf_CodPerfil")
                .HasComment("Codigo de perfil");

            builder.Property(e => e.PxuUsrCodUsuario)
                .HasColumnName("Pxu_Usr_CodUsuario")
                .HasComment("Codigo de usuario");

           
        }
    }
}
