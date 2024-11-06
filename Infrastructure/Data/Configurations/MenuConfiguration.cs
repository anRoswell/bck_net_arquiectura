using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable("Menu", "usr");

            builder.HasKey(e => e.Id)
                .HasName("PK__Menu__B16E9B1CD9B56275");

            builder.Property(e => e.Id)
            .HasColumnName("Men_IdMenu");

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

            builder.Property(e => e.MenAccion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Men_Accion")
                .HasComment("Metodo del controlador");

            builder.Property(e => e.MenAplCodAplicacion)
                .HasColumnName("Men_Apl_CodAplicacion")
                .HasComment("Codigo de aplicación");

            builder.Property(e => e.MenControlador)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Men_Controlador")
                .HasComment("Controlador correspondiente a la opcion");

            builder.Property(e => e.MenEstado)
                .IsRequired()
                .HasColumnName("Men_Estado")
                .HasDefaultValueSql("((1))")
                .HasComment("Estado del registro");

            builder.Property(e => e.MenImagen)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Men_Imagen")
                .HasComment("Imagen de la opcion");

            builder.Property(e => e.MenModuloDescripcion)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Men_Modulo_Descripcion")
                .HasComment("Descricion del modulo");

            builder.Property(e => e.MenNivelDos)
                .HasColumnName("Men_Nivel_dos")
                .HasComment("Nivel dos del menu");

            builder.Property(e => e.MenNivelTres)
                .HasColumnName("Men_Nivel_tres")
                .HasComment("Nivel tres del menu");

            builder.Property(e => e.MenNivelUno)
                .HasColumnName("Men_Nivel_uno")
                .HasComment("Nivel uno del menu");

            builder.Property(e => e.MenTusrCodTipoUsuario)
                .HasColumnName("Men_Tusr_CodTipoUsuario")
                .HasComment("Codigo de tipo de usuario");

            //builder.HasOne(d => d.MenAplCodAplicacionNavigation)
            //    .WithMany(p => p.Menus)
            //    .HasForeignKey(d => d.MenAplCodAplicacion)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_Menu_Aplicacion");

            //builder.HasOne(d => d.MenTusrCodTipoUsuarioNavigation)
            //    .WithMany(p => p.Menus)
            //    .HasForeignKey(d => d.MenTusrCodTipoUsuario)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_Menu_TipoUsuario");
        }
    }
}
