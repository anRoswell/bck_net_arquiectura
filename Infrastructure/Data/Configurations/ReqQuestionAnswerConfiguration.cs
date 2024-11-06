using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class ReqQuestionAnswerConfiguration : IEntityTypeConfiguration<ReqQuestionAnswer>
    {
        public void Configure(EntityTypeBuilder<ReqQuestionAnswer> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__ReqQuest__B958DCF844C41D27");

            builder.ToTable("ReqQuestionAnswer", "req");

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

            builder.Property(e => e.RqaCodRequerimiento)
                .HasColumnName("rqaCodRequerimiento")
                .HasComment("Id del requerimiento");

            builder.Property(e => e.RqaCodUsuario)
                .HasColumnName("rqaCodUsuario")
                .HasComment("Id del gestor de proveedor");

            builder.Property(e => e.RqaComentario)
                .IsRequired()
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("rqaComentario")
                .HasComment("Comentario realizado");

            builder.Property(e => e.RqaEstado)
                .IsRequired()
                .HasColumnName("rqaEstado")
                .HasDefaultValueSql("((1))")
                .HasComment("Indica el estado del comentario");

            builder.Property(e => e.RqahasUploadFile)
                .HasColumnName("rqahasUploadFile")
                .HasComment("Indica si el comentario tiene algun archivo adjunto");

            builder.Property(e => e.RqaisPrivate)
                .HasColumnName("rqaisPrivate")
                .HasComment("Indica si el comentario es privado");

            //builder.HasOne(d => d.RqaCodRequerimientoNavigation)
            //    .WithMany(p => p.ReqQuestionAnswers)
            //    .HasForeignKey(d => d.RqaCodRequerimiento)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_ReqQuestionAnswer_Requerimientos");
        }
    }
}
