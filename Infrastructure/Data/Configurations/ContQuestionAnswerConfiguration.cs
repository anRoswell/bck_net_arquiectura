using Core.Entities;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class ContQuestionAnswerConfiguration : IEntityTypeConfiguration<ContQuestionAnswer>
    {
        public void Configure(EntityTypeBuilder<ContQuestionAnswer> builder)
        {
            builder.ToTable("ContQuestionAnswer", "cont");

            builder.Property(e => e.Id).HasComment("Identificación del registro");

            builder.Property(e => e.CodUser)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7777777')")
                .HasComment("Id del usuario que crea el registro");

            builder.Property(e => e.CodUserUpdate)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7777777')")
                .HasComment("Cedula del ultimo usuario que actualizó el registro");

            builder.Property(e => e.Comentario).HasComment("Comentario realizado");

            builder.Property(e => e.ContieneAdjunto)
                .HasDefaultValueSql("((0))")
                .HasComment("Indica si el comentario tiene algun archivo adjunto");

            builder.Property(e => e.EsGestor)
                .HasDefaultValueSql("((0))")
                .HasComment("Si es gestor o no");

            builder.Property(e => e.EsPrivado)
                .HasDefaultValueSql("((0))")
                .HasComment("Indica si el comentario es privado");

            builder.Property(e => e.Estado)
                .HasDefaultValueSql("((1))")
                .HasComment("Indica el estado del comentario");

            builder.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha de creación del registro.");

            builder.Property(e => e.FechaRegistroUpdate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha de la ultima actualización del registro.");

            builder.Property(e => e.FileExt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Extensión del documento");

            builder.Property(e => e.FilePath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("Ruta del documento");

            builder.Property(e => e.FileRelativo)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasComment("Ruta relativa del documento");

            builder.Property(e => e.FileSize).HasComment("Tamaño del documento");

            builder.Property(e => e.IdContrato).HasComment("Id del contrato");

            builder.Property(e => e.IdQuestionAnswer).HasComment("indica el id padre al q pertenece esta respuesta, si es null, indica q es respueta padre.");

            builder.Property(e => e.IdUsuario).HasComment("Id del gestor o proveedor");

            builder.Property(e => e.Info)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')")
                .HasComment("En este campo almacenamos la direccion ip, navegador y version del navegador del cliente.");

            builder.Property(e => e.InfoUpdate)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')")
                .HasComment("En este campo almacenamos la ultima direccion ip, navegador y version del navegador del cliente.");

            builder.HasOne(d => d.IdQuestionAnswerNavigation)
                .WithMany(p => p.InverseIdQuestionAnswerNavigation)
                .HasForeignKey(d => d.IdQuestionAnswer)
                .HasConstraintName("FK_ContQuestionAnswer_ContQuestionAnswer_Padre");
        }
    }
}
