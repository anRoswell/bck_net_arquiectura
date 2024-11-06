using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class PermisosUsuarioxMenuConfiguration : IEntityTypeConfiguration<PermisosUsuarioxMenu>
    {
        public void Configure(EntityTypeBuilder<PermisosUsuarioxMenu> builder)
        {
            builder.ToTable("PermisosUsuarioxMenu", "usr");

            builder.HasKey(e => e.Id)
                .HasName("PK__Permisos__FA665AB5C8F2D1F8");

            builder.Property(e => e.Id)
                .HasColumnName("Pum_IdPermisosUsuarioxMenu");

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

            builder.Property(e => e.PumAplCodAplicacion)
                .HasColumnName("Pum_Apl_CodAplicacion")
                .HasComment("Codigo de aplicación");

            builder.Property(e => e.PumBorrar)
                .HasColumnName("Pum_Borrar")
                .HasDefaultValueSql("((1))")
                .HasComment("Indica si puede borrar");

            builder.Property(e => e.PumConsultar)
                .HasColumnName("Pum_Consultar")
                .HasDefaultValueSql("((1))")
                .HasComment("Indica si puede consultar");

            builder.Property(e => e.PumEditar)
                .HasColumnName("Pum_Editar")
                .HasDefaultValueSql("((1))")
                .HasComment("Indica si puede editar");

            builder.Property(e => e.PumEjecutar)
                .HasColumnName("Pum_Ejecutar")
                .HasDefaultValueSql("((1))")
                .HasComment("Indica si puede ejecutar accion");

            builder.Property(e => e.PumGrabar)
                .HasColumnName("Pum_Grabar")
                .HasDefaultValueSql("((1))")
                .HasComment("Indica si puede grabar");

            builder.Property(e => e.PumLeer)
                .HasColumnName("Pum_Leer")
                .HasDefaultValueSql("((1))")
                .HasComment("Indica si puede leer");

            builder.Property(e => e.PumMenCodMenu)
                .HasColumnName("Pum_Men_CodMenu")
                .HasComment("Codigo de menu");

            builder.Property(e => e.PumUsrCodUsuario)
                .HasColumnName("Pum_Usr_CodUsuario")
                .HasComment("Codigo de usuario");

            //builder.HasOne(d => d.PumAplCodAplicacionNavigation)
            //    .WithMany(p => p.PermisosUsuarioxMenus)
            //    .HasForeignKey(d => d.PumAplCodAplicacion)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_PermisosUsuarioxMenu_Aplicacion");

            //builder.HasOne(d => d.PumMenCodMenuNavigation)
            //    .WithMany(p => p.PermisosUsuarioxMenus)
            //    .HasForeignKey(d => d.PumMenCodMenu)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_PermisosUsuarioxMenu_Menu");

            //builder.HasOne(d => d.PumUsrCodUsuarioNavigation)
            //    .WithMany(p => p.PermisosUsuarioxMenus)
            //    .HasForeignKey(d => d.PumUsrCodUsuario)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_PermisosUsuarioxMenu_Usuario");
        }
    }
}
