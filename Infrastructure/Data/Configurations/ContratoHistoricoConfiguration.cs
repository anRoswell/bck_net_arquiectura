using Core.Entities;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class ContratoHistoricoConfiguration : IEntityTypeConfiguration<ContratoHistorico>
    {
        public void Configure(EntityTypeBuilder<ContratoHistorico> entity)
        {
            entity.HasKey(e => e.ContIdContratoHistorico)
                    .HasName("PK__Contrato__CAAF208FAC20E7CC");

            entity.ToTable("ContratoHistorico", "cont");

            entity.Property(e => e.ContIdContratoHistorico)
                .HasColumnName("contIdContratoHistorico")
                .HasComment("(generado automáticamente por el sistema),");

            entity.Property(e => e.CodArchivoActaLiquidacion).HasComment("Archivo de acta de liquidación");

            entity.Property(e => e.CodArchivoNotificacionNoProrroga).HasComment("Archivo de notificación de no prorroga");

            entity.Property(e => e.CodArchivoNotificacionTerminacion).HasComment("Archivo de notificación de terminación");

            entity.Property(e => e.CodUser)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7777777')")
                .HasComment("Cedula del usuario que crea el registro");

            entity.Property(e => e.CodUserUpdate)
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('7777777')")
                .HasComment("Cedula del ultimo usuario que actualizó el registro");

            entity.Property(e => e.ContAprobArea).HasColumnName("contAprobArea");

            entity.Property(e => e.ContAprobCompras).HasColumnName("contAprobCompras");

            entity.Property(e => e.ContAprobFinanciera).HasColumnName("contAprobFinanciera");

            entity.Property(e => e.ContAprobacionProrroga)
                .HasColumnName("contAprobacionProrroga")
                .HasComment("Aprobación de Prorroga");

            entity.Property(e => e.ContArchivoActaInicio).HasComment("Id archivo compra no presupuestada");

            entity.Property(e => e.ContArchivoCompraNoPresupuestada)
                .HasColumnName("contArchivoCompraNoPresupuestada")
                .HasComment("Id archivo compra no presupuestada");

            entity.Property(e => e.ContCarateristicasEspecificas)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("contCarateristicasEspecificas")
                .HasComment("Obligaciones especificas");

            entity.Property(e => e.ContCcrepresentanteCtante)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("contCCRepresentanteCtante");

            entity.Property(e => e.ContCodClaseContrato)
                .HasColumnName("contCodClaseContrato")
                .HasComment("clase [de servicios | de suministros | etc.]");

            entity.Property(e => e.ContCodContrato)
                .HasColumnName("contCodContrato")
                .HasComment("Codigo contrato maestro,");

            entity.Property(e => e.ContCodCoordinadorContrato)
                .HasColumnName("contCodCoordinadorContrato")
                .HasComment("id del coordinador del contrato");

            entity.Property(e => e.ContCodEmpresa)
                .HasColumnName("contCodEmpresa")
                .HasComment("Nit del contratante");

            entity.Property(e => e.ContCodEstado)
                .HasColumnName("contCodEstado")
                .HasDefaultValueSql("((1))")
                .HasComment("Estado del registro");

            entity.Property(e => e.ContCodFormaPago)
                .HasColumnName("contCodFormaPago")
                .HasComment("Id de la forma de pago");

            entity.Property(e => e.ContCodGestorContrato)
                .HasColumnName("contCodGestorContrato")
                .HasComment("Id del gestor del contrato");

            entity.Property(e => e.ContCodGestorRiesgo).HasColumnName("contCodGestorRiesgo");

            entity.Property(e => e.ContCodProveedor)
                .HasColumnName("contCodProveedor")
                .HasComment("información del contratista,");

            entity.Property(e => e.ContCodRequerimiento).HasColumnName("contCodRequerimiento");

            entity.Property(e => e.ContCodRequisitor)
                .HasColumnName("contCodRequisitor")
                .HasComment("Codigo del Requisitor del Contrato");

            entity.Property(e => e.ContCodTipoProrroga).HasColumnName("contCodTipoProrroga");

            entity.Property(e => e.ContCodUnidadNegocio)
                .HasColumnName("contCodUnidadNegocio")
                .HasComment("Unidad de negocio");

            entity.Property(e => e.ContConsecutivoAlterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contConsecutivoAlterno")
                .HasComment("Consecutivo alterno de la version del contrato");

            entity.Property(e => e.ContConsecutivoFlujo)
                .HasColumnName("contConsecutivoFlujo")
                .HasDefaultValueSql("((1))")
                .HasComment("Consecutivo para determinar la cantidad de veces que se ha solicitado modificacion al contrato");

            entity.Property(e => e.ContContactoContratista)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("Contacto del Contratista del Contrato");

            entity.Property(e => e.ContDireccionNotificacionCtante)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("contDireccionNotificacionCtante");

            entity.Property(e => e.ContDuracionContrato)
                .HasColumnName("contDuracionContrato")
                .HasComment("Duracion del contrato en dias");

            entity.Property(e => e.ContDuracionProrroga)
                .HasColumnName("contDuracionProrroga")
                .HasDefaultValueSql("((0))")
                .HasComment("Días para notificación de no renovación automática");

            entity.Property(e => e.ContEmailContactoContratista)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contEmailContactoContratista")
                .HasComment("Email de contacto del Contratista del Contrato");

            entity.Property(e => e.ContEmailContratante)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contEmailContratante")
                .HasComment("Correo para la notificacion de correo del rep. legar y firma electronica");

            entity.Property(e => e.ContEmailRteLegalCtante)
                .HasMaxLength(256)
                .HasColumnName("contEmailRteLegalCtante")
                .HasComment("Email del representante legal del contratante");

            entity.Property(e => e.ContFechaInicioProrroga)
                .HasColumnType("datetime")
                .HasColumnName("contFechaInicioProrroga")
                .HasComputedColumnSql("(dateadd(day,([contPreavisoProrrogaDias]+(5))*(-1),[contVigenciaHasta]))", false);

            entity.Property(e => e.ContFechaLiquidacionEsperada)
                .HasColumnType("datetime")
                .HasColumnName("contFechaLiquidacionEsperada")
                .HasComment("fecha liquidacion esperada");

            entity.Property(e => e.ContFechaLiquidacionReal)
                .HasColumnType("datetime")
                .HasColumnName("contFechaLiquidacionReal")
                .HasComment("fecha liquidacion real");

            entity.Property(e => e.ContFechaNotifTermAnticipada)
                .HasColumnType("datetime")
                .HasColumnName("contFechaNotifTermAnticipada")
                .HasComment("Fecha de notificacion de terminacion anticipada");

            entity.Property(e => e.ContFechaNotificacionNoProrroga)
                .HasColumnType("datetime")
                .HasColumnName("contFechaNotificacionNoProrroga")
                .HasComment("Fecha de notificacion de No prorroga");

            entity.Property(e => e.ContJustificacionRechazo)
                .IsUnicode(false)
                .HasColumnName("contJustificacionRechazo")
                .HasComment("Justificación de rechazo de documentos del proveedor del Contrato.");

            entity.Property(e => e.ContNitContratista)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("contNitContratista")
                .HasComment("Nit del Contratista del Contrato");

            entity.Property(e => e.ContNombreRteLegalCtante)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("contNombreRteLegalCtante");

            entity.Property(e => e.ContObjetoContrato)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("contObjetoContrato")
                .HasComment("Objeto del contrato");

            entity.Property(e => e.ContObservacion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("contObservacion")
                .HasComment("Descripcion de Contrato");

            entity.Property(e => e.ContObservacionNoProrroga)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("contObservacionNoProrroga")
                .HasComment("Observacion de notificacion de No prorroga");

            entity.Property(e => e.ContObservacionTermAnticipada)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("contObservacionTermAnticipada")
                .HasComment("Observacion de notificacion de terminacion anticipada");

            entity.Property(e => e.ContOtroSi)
                .HasColumnName("contOtroSi")
                .HasComment("Codigo de Otro Si relacionado a la version del contrato");

            entity.Property(e => e.ContPolizasRenovadas).HasComment("Contiene polizas renovadas");

            entity.Property(e => e.ContPreavisoProrrogaDias)
                .HasColumnName("contPreavisoProrrogaDias")
                .HasDefaultValueSql("((0))")
                .HasComment("Pre aviso prorroga en meses");

            entity.Property(e => e.ContPresupuestado)
                .HasColumnName("contPresupuestado")
                .HasComment("Si el contrato es presupuestado o no");

            entity.Property(e => e.ContRequiereActaInicio)
                .HasColumnName("contRequiereActaInicio")
                .HasDefaultValueSql("((0))")
                .HasComment("¿requiere acta de inicio? [si | no],");

            entity.Property(e => e.ContRequiereActaLiquidacion)
                .HasColumnName("contRequiereActaLiquidacion")
                .HasDefaultValueSql("((0))")
                .HasComment("¿requiere acta de liquidacion? [si | no],");

            entity.Property(e => e.ContRequiereIngresoPersonal)
                .HasColumnName("contRequiereIngresoPersonal")
                .HasDefaultValueSql("((0))")
                .HasComment("¿ingresará personal del contratista a las instalaciones del contratante? [si | no],");

            entity.Property(e => e.ContRequierenAnticipos).HasColumnName("contRequierenAnticipos");

            entity.Property(e => e.ContTelefonoCtante)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("contTelefonoCtante");

            entity.Property(e => e.ContTipoDocumento)
                .HasColumnName("contTipoDocumento")
                .HasComment("1 = Contrato, 2 = Prorroga");

            entity.Property(e => e.ContUrlPdf)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("contUrlPdf")
                .HasComment("URL relativa, en el File Server, del contrato");

            entity.Property(e => e.ContValorAnticipo)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("contValorAnticipo");

            entity.Property(e => e.ContValorContrato)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("contValorContrato")
                .HasComment("valor del contrato en pesos");

            entity.Property(e => e.ContVigenciaDesde)
                .HasColumnType("datetime")
                .HasColumnName("contVigenciaDesde")
                .HasComment("Vigencia desde");

            entity.Property(e => e.ContVigenciaHasta)
                .HasColumnType("datetime")
                .HasColumnName("contVigenciaHasta")
                .HasComment("Vigencia hasta");

            entity.Property(e => e.ContCodTipoContrato)
                .HasColumnName("contCodTipoContrato");

            entity.Property(e => e.ContCodRepresentanteLegal)
                .HasColumnName("contCodRepresentanteLegal");

            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha de creación del registro.");

            entity.Property(e => e.FechaRegistroUpdate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Fecha de la ultima actualización del registro.");

            entity.Property(e => e.Info)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')")
                .HasComment("En este campo almacenamos la direccion ip, navegador y version del navegador del cliente.");

            entity.Property(e => e.InfoUpdate)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasDefaultValueSql("('0|0|0')")
                .HasComment("En este campo almacenamos la ultima direccion ip, navegador y version del navegador del cliente.");

            entity.HasOne(d => d.ContContrato)
                .WithMany(p => p.ContratoHistoricos)
                .HasForeignKey(d => d.ContCodContrato)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContratoHistorico_Contrato");

            entity.Property(e => e.ContFechaActaInicio)
                .HasColumnType("datetime")
                .HasColumnName("contFechaActaInicio")
                .HasComment("Fecha Inicio de contrato segun acta de inicio");

            entity.Property(e => e.ContUrlAbsolutePdf)
                    .IsUnicode(false)
                    .HasColumnName("contUrlAbsolutePdf")
                    .HasComment("Url publica del archivo del contrato");

            entity.HasOne(d => d.ArchivoActaLiquidacion)
                    .WithMany(p => p.ContratoHistoricoCodArchivoActaLiquidacion)
                    .HasForeignKey(d => d.CodArchivoActaLiquidacion)
                    .HasConstraintName("FK_ContratoHistorico_DocReqUpload_Acta_Liquidacion");

            entity.HasOne(d => d.ArchivoNotificacionNoProrroga)
                .WithMany(p => p.ContratoHistoricoCodArchivoNotificacionNoProrroga)
                .HasForeignKey(d => d.CodArchivoNotificacionNoProrroga)
                .HasConstraintName("FK_ContratoHistorico_DocReqUpload_Notificacion_No_Prorroga");

            entity.HasOne(d => d.ArchivoNotificacionTerminacion)
                .WithMany(p => p.ContratoHistoricoCodArchivoNotificacionTerminacion)
                .HasForeignKey(d => d.CodArchivoNotificacionTerminacion)
                .HasConstraintName("FK_ContratoHistorico_DocReqUpload_Notificacion_Terminacion");

            entity.HasOne(d => d.ArchivoActaInicio)
                .WithMany(p => p.ContratoHistoricoContArchivoActaInicio)
                .HasForeignKey(d => d.ContArchivoActaInicio)
                .HasConstraintName("FK_ContratoHistorico_DocReqUpload_Acta_Inicio");

            entity.HasOne(d => d.ArchivoCompraNoPresupuestada)
                .WithMany(p => p.ContratoHistoricoContArchivoCompraNoPresupuestada)
                .HasForeignKey(d => d.ContArchivoCompraNoPresupuestada)
                .HasConstraintName("FK_ContratoHistorico_DocReqUpload_Compra_No_Presupuestada");
        }
    }
}
