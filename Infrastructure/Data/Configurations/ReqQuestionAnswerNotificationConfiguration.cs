using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class ReqQuestionAnswerNotificationConfiguration : IEntityTypeConfiguration<ReqQuestionAnswerNotification>
    {
        public void Configure(EntityTypeBuilder<ReqQuestionAnswerNotification> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__ReqQuest__FAD3488DD69B061C");

            builder.ToTable("ReqQuestionAnswerNotification", "req");

            builder.Property(e => e.Id).HasColumnName("rqanIdReqQuestionAnswerNotification");

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

            builder.Property(e => e.RqanCodProveedor)
                .HasColumnName("rqanCodProveedor")
                .HasComment("Id del proveedor");

            builder.Property(e => e.RqanCodRequerimiento).HasColumnName("rqanCodRequerimiento");

            builder.Property(e => e.RqanCodUsuario)
                .HasColumnName("rqanCodUsuario")
                .HasComment("Id del gestor de requerimiento");

            builder.Property(e => e.RqanEstado)
                .HasColumnName("rqanEstado")
                .HasComment("Indicado el estado de la notificación");

            //builder.HasOne(d => d.RqanCodRequerimientoNavigation)
            //    .WithMany(p => p.ReqQuestionAnswerNotifications)
            //    .HasForeignKey(d => d.RqanCodRequerimiento)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_ReqQuestionAnswerNotification_Requerimientos");

        }
    }
}
