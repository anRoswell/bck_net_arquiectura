using Core.Entities;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ContratoConfiguration : IEntityTypeConfiguration<Contrato>
    {
        public void Configure(EntityTypeBuilder<Contrato> builder)
        {
            builder.HasKey(e => e.Id)
                    .HasName("PK__Contrato__F8CC530EF65D43FB");

            builder.ToTable("Contrato", "cont");

            builder.Property(e => e.Id)
                .HasColumnName("contIdContrato")
                .HasComment("(generado automáticamente por el sistema),");

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

            builder.Property(e => e.ContAprobArea).HasColumnName("contAprobArea");

            builder.Property(e => e.ContAprobCompras).HasColumnName("contAprobCompras");

            builder.Property(e => e.ContAprobFinanciera).HasColumnName("contAprobFinanciera");

            builder.Property(e => e.ContCarateristicasEspecificas)
                .IsUnicode(false)
                .HasColumnName("contCarateristicasEspecificas")
                .HasComment("Obligaciones especificas");

            builder.Property(e => e.ContCcrepresentanteCtante)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("contCCRepresentanteCtante");

            builder.Property(e => e.ContCodClaseContrato)
                .HasColumnName("contCodClaseContrato")
                .HasComment("clase [de servicios | de suministros | etc.]");

            builder.Property(e => e.ContCodCoordinadorContrato)
                .HasColumnName("contCodCoordinadorContrato")
                .HasComment("id del coordinador del contrato");

            builder.Property(e => e.ContCodEmpresa)
                .HasColumnName("contCodEmpresa")
                .HasComment("Nit del contratante");

            builder.Property(e => e.ContCodEstado)
                .HasColumnName("contCodEstado")
                .HasDefaultValueSql("((1))")
                .HasComment("Estado del registro");

            builder.Property(e => e.ContCodFormaPago)
                .HasColumnName("contCodFormaPago")
                .HasComment("Id de la forma de pago");

            builder.Property(e => e.ContCodGestorContrato)
                .HasColumnName("contCodGestorContrato")
                .HasComment("Id del gestor del contrato");

            builder.Property(e => e.ContCodGestorRiesgo).HasColumnName("contCodGestorRiesgo");

            builder.Property(e => e.ContCodProveedor)
                .HasColumnName("contCodProveedor")
                .HasComment("información del contratista,");

            builder.Property(e => e.ContCodRequerimiento).HasColumnName("contCodRequerimiento");

            builder.Property(e => e.ContCodTipoProrroga).HasColumnName("contCodTipoProrroga");

            builder.Property(e => e.ContCodUnidadNegocio)
                .HasColumnName("contCodUnidadNegocio")
                .HasComment("Unidad de negocio");

            builder.Property(e => e.ContDireccionNotificacionCtante)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("contDireccionNotificacionCtante");

            builder.Property(e => e.ContDuracionContrato)
                .HasColumnName("contDuracionContrato")
                .HasComment("Duracion del contrato en dias");

            builder.Property(e => e.ContDuracionProrroga)
                .HasColumnName("contDuracionProrroga")
                .HasDefaultValueSql("((0))")
                .HasComment("Días para notificación de no renovación automática");

            builder.Property(e => e.ContFechaLiquidacionEsperada)
                .HasColumnType("datetime")
                .HasColumnName("contFechaLiquidacionEsperada")
                .HasComment("fecha liquidacion esperada");

            builder.Property(e => e.ContFechaLiquidacionReal)
                .HasColumnType("datetime")
                .HasColumnName("contFechaLiquidacionReal")
                .HasComment("fecha liquidacion real");

            builder.Property(e => e.ContNombreRteLegalCtante)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("contNombreRteLegalCtante");

            builder.Property(e => e.ContObjetoContrato)
                .IsUnicode(false)
                .HasColumnName("contObjetoContrato")
                .HasComment("Objeto del contrato");

            builder.Property(e => e.ContPreavisoProrrogaDias)
                .HasColumnName("contPreavisoProrrogaDias")
                .HasDefaultValueSql("((0))")
                .HasComment("Pre aviso prorroga en meses");

            builder.Property(e => e.ContPresupuestado)
                .HasColumnName("contPresupuestado")
                .HasComment("Si el contrato es presupuestado o no");

            builder.Property(e => e.ContRequiereActaInicio)
                .HasColumnName("contRequiereActaInicio")
                .HasDefaultValueSql("((0))")
                .HasComment("¿requiere acta de inicio? [si | no],");

            builder.Property(e => e.ContRequiereActaLiquidacion)
                .HasColumnName("contRequiereActaLiquidacion")
                .HasDefaultValueSql("((0))")
                .HasComment("¿requiere acta de liquidacion? [si | no],");

            builder.Property(e => e.ContRequiereIngresoPersonal)
                .HasColumnName("contRequiereIngresoPersonal")
                .HasDefaultValueSql("((0))")
                .HasComment("¿ingresará personal del contratista a las instalaciones del contratante? [si | no],");

            builder.Property(e => e.ContRequierenAnticipos).HasColumnName("contRequierenAnticipos");

            builder.Property(e => e.ContTelefonoCtante)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("contTelefonoCtante");

            builder.Property(e => e.ContTipoDocumento)
                .HasColumnName("contTipoDocumento")
                .HasComment("1 = Contrato, 2 = Prorroga");

            builder.Property(e => e.ContValorAnticipo)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("contValorAnticipo");

            builder.Property(e => e.ContValorContrato)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("contValorContrato")
                .HasComment("valor del contrato en pesos");

            builder.Property(e => e.ContVigenciaDesde)
                .HasColumnType("datetime")
                .HasColumnName("contVigenciaDesde")
                .HasComment("Vigencia desde");

            builder.Property(e => e.ContVigenciaHasta)
                .HasColumnType("datetime")
                .HasColumnName("contVigenciaHasta")
                .HasComment("Vigencia hasta");

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

            builder.Property(e => e.ContEmailContactoContratista)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("contEmailContactoContratista")
                    .HasComment("En este campo almacenamos el email de contacto del contratista del Contrato.");

            builder.Property(e => e.ContContactoContratista)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasComment("En este campo almacenamos el contacto del contratista del Contrato."); 

            builder.Property(e => e.ContObservacion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("contObservacion")
                    .HasComment("En este campo almacenamos las observaciones del Contrato.");

            builder.Property(e => e.ContNitContratista)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("contNitContratista")
                    .HasComment("Nit del Contratista del Contrato");

            builder.Property(e => e.ContCodRequisitor)
                    .HasColumnName("contCodRequisitor")
                    .HasComment("Codigo del Requisitor del Contrato");

            builder.Property(e => e.ContConsecutivoFlujo)
                    .HasColumnName("contConsecutivoFlujo")
                    .HasDefaultValueSql("((1))")
                    .HasComment("Consecutivo para determinar la cantidad de veces que se ha solicitado modificacion al contrato");

            builder.Property(e => e.ContEmailRteLegalCtante)
                .HasMaxLength(256)
                .HasColumnName("contEmailRteLegalCtante")
                .HasComment("Email del representante legal del contratante");

            builder.Property(e => e.ContJustificacionRechazo)
                    .IsUnicode(false)
                    .HasColumnName("contJustificacionRechazo")
                    .HasComment("Justificación de rechazo de documentos del proveedor del Contrato.");

            builder.Property(e => e.ContArchivoCompraNoPresupuestada)
                    .HasColumnName("contArchivoCompraNoPresupuestada")
                    .HasComment("Id archivo compra no presupuestada");

            builder.Property(e => e.ContEmailContratante)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("contEmailContratante")
                    .HasComment("Correo para la notificacion de correo del rep. legar y firma electronica");

            builder.Property(e => e.ContArchivoActaInicio).HasComment("Id archivo acta de inicio");

            builder.Property(e => e.CodArchivoNotificacionNoProrroga).HasComment("Archivo de notificación de no prorroga");

            builder.Property(e => e.CodArchivoNotificacionTerminacion).HasComment("Archivo de notificación de terminación");

            builder.Property(e => e.ContFechaInicioProrroga)
                    .HasColumnType("datetime")
                    .HasColumnName("contFechaInicioProrroga")
                    .HasComputedColumnSql("(dateadd(day,([contPreavisoProrrogaDias]+5)*(-1),[contVigenciaHasta]))", false);

            builder.Property(e => e.ContAprobacionProrroga)
                    .HasColumnName("contAprobacionProrroga")
                    .HasComment("Aprobación de Prorroga");

            builder.Property(e => e.CodArchivoActaLiquidacion).HasComment("Archivo de acta de liquidación");

            builder.Property(e => e.ContPolizasRenovadas).HasComment("Contiene polizas renovadas");

            builder.Property(e => e.ContOtroSiActual)
                    .HasColumnName("contOtroSiActual")
                    .HasComment("Codigo de Otro Si relacionado actualmente al contrato");

            builder.Property(e => e.ContConsecutivoAlterno)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("contConsecutivoAlterno")
                    .HasComment("Consecutivo alterno de la version del contrato");

            builder.Property(e => e.ContCodContratoHistoricoActual)
                    .HasColumnName("contCodContratoHistoricoActual")
                    .HasComment("Codigo de Contrato Historico actual (Version del contrato)");

            //Relaciones

            builder.HasOne(d => d.ContratoHistoricoActual)
                    .WithMany(p => p.Contratos)
                    .HasForeignKey(d => d.ContCodContratoHistoricoActual)
                    .HasConstraintName("FK_Contrato_ContratoHistorico_OtroSi");

            builder.HasOne(d => d.ContClaseContrato)
                    .WithMany(p => p.Contratos)
                    .HasForeignKey(d => d.ContCodClaseContrato)
                    .HasConstraintName("FK_Contrato_ClaseContrato");

            builder.HasOne(d => d.ContProveedor)
                .WithMany(p => p.Contratos)
                .HasForeignKey(d => d.ContCodProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contrato_Proveedores");
            
            builder.Property(e => e.ContFechaActaInicio)
                .HasColumnType("datetime")
                .HasColumnName("contFechaActaInicio")
                .HasComment("Fecha Inicio de contrato segun acta de inicio");

            builder.Property(e => e.ContUrlAbsolutePdf)
                    .IsUnicode(false)
                    .HasColumnName("contUrlAbsolutePdf")
                    .HasComment("Url publica del archivo del contrato");
            
            builder.Property(e => e.ContCodTipoContrato)
                    .HasColumnName("contCodTipoContrato")
                    .HasComment("Codigo del Tipo del Contrato");

            builder.Property(e => e.contCodRepresentanteLegal)
                    .HasColumnName("contCodRepresentanteLegal")
                    .HasComment("Codigo del representante legal");

            //Ignore fields
            builder.Ignore(e => e.ClaseContrato);
            builder.Ignore(e => e.ProveedorContrato);
            builder.Ignore(e => e.EmpresaContrato);
            builder.Ignore(e => e.AprobadoPorCoordinador);
            builder.Ignore(e => e.AprobadoPorFinanciero);
            builder.Ignore(e => e.AprobadoPorCompras);
            builder.Ignore(e => e.AprobadoPorArea);
            builder.Ignore(e => e.UrlArchivoCompraNoPresupuestada);
            builder.Ignore(e => e.NombreArchivoCompraNoPresupuestada);
            builder.Ignore(e => e.UrlArchivoActaInicio);
            builder.Ignore(e => e.NombreArchivoActaInicio);
            builder.Ignore(e => e.UrlArchivoNotificacionNoProrroga);
            builder.Ignore(e => e.NombreArchivoNotificacionNoProrroga);
            builder.Ignore(e => e.UrlArchivoNotificacionTerminacion);
            builder.Ignore(e => e.NombreArchivoNotificacionTerminacion);
            builder.Ignore(e => e.UrlArchivoActaLiquidacion);
            builder.Ignore(e => e.NombreArchivoActaLiquidacion);
        }
    }
}
