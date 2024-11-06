namespace Infrastructure.Utils
{
    public static class CallStoredProcedures
    {
        public static class AdobeSign
        {
            public const string GuardarTraza_Prv = @"[adb].[SpAdobeSign] @Operacion = @Operacion,
                                                                         @CodProveedor = @CodProveedor,
                                                                         @CodAdobeSignEstado = @CodAdobeSignEstado,
                                                                         @adsgnIdAdobeSign = @adsgnIdAdobeSign,
                                                                         @adsgnJson = @adsgnJson";

            public const string GuardarTraza_Contrato = @"[adb].[SpAdobeSign] @Operacion = @Operacion,
                                                                               @CodContrato = @CodContrato,
                                                                               @CodAdobeSignEstado = @CodAdobeSignEstado,
                                                                               @adsgnIdAdobeSign = @adsgnIdAdobeSign,
                                                                               @adsgnJson = @adsgnJson";

            public const string GuardarLogError_AdobeSign = @"[adb].[SpAdobeSign] @Operacion = @Operacion,
                                                                                  @adsgeErrorCode = @adsgeErrorCode,
                                                                                  @adsgeErrorMessage = @adsgeErrorMessage,
                                                                                  @adsgeCodLlave = @adsgeCodLlave,
                                                                                  @adsgeErrorProcess = @adsgeErrorProcess";
        }

        public static class Documento
        {
            public const string BuscarDocumentos = "[par].[SpDocumentos] @Operacion = @Operacion";
            public const string BuscarDocumentoById = "[par].[SpDocumentos] @Operacion = @Operacion, @cocIdDocumentos = @cocIdDocumentos";

            public const string CrearDocumento = @"[par].[SpDocumentos] @Operacion = @Operacion,
                                                                        @cocCodModuloDocumentos = @cocCodModuloDocumentos,
                                                                        @cocNombreDocumento = @cocNombreDocumento,
                                                                        @cocDescripcion = @cocDescripcion,
                                                                        @cocrequiered = @cocrequiered,
                                                                        @coclimitLoad = @coclimitLoad,
                                                                        @cocVigencia = @cocVigencia,
                                                                        @cocVigenciaMaxima = @cocVigenciaMaxima,
                                                                        @CodUser = @CodUser";

            public const string ActualizarDocumento = @"[par].[SpDocumentos] @Operacion = @Operacion,
                                                                        @cocIdDocumentos = @cocIdDocumentos,
                                                                        @cocCodModuloDocumentos = @cocCodModuloDocumentos,
                                                                        @cocNombreDocumento = @cocNombreDocumento,
                                                                        @cocDescripcion = @cocDescripcion,
                                                                        @cocEstado = @cocEstado,
                                                                        @cocrequiered = @cocrequiered,
                                                                        @coclimitLoad = @coclimitLoad,
                                                                        @cocVigencia = @cocVigencia,
                                                                        @cocVigenciaMaxima = @cocVigenciaMaxima,
                                                                        @CodUserUpdate = @CodUserUpdate";

            public const string EliminarDocumento = "[par].[SpDocumentos] @Operacion = @Operacion, @cocIdDocumentos = @cocIdDocumentos, @CodUserUpdate = @CodUserUpdate";
        }

        public static class Contrato
        {
            public const string BuscarContratoPorId = "[cont].[SpContrato] @Operacion = @Operacion, @contIdContrato = @contIdContrato";

            public const string BuscarContratosDeProveedor = "[cont].[SpContrato] @Operacion = @Operacion, @CodUser = @CodUser";

            public const string CambiarEstadoContrato = "[cont].[SpContrato] @Operacion = @Operacion, @contIdContrato = @contIdContrato, @contCodEstado = @contCodEstado, @CodUserUpdate = @CodUserUpdate, @ContConsecutivoAlterno = @ContConsecutivoAlterno";

            public const string CambiarEstadoContratoAnterior = "[cont].[SpCambioEstadoContrato] @contIdContrato = @contIdContrato, @contCodEstado = @contCodEstado, @CodUserUpdate = @CodUserUpdate";

            public const string BuscarDocumentosContratoPorId = "[cont].[SpContrato] @Operacion = @Operacion, @contIdContrato = @contIdContrato";

            public const string BuscarDocumentosReqContById = "[cont].[SpContrato] @Operacion = @Operacion, @contIdContrato = @contIdContrato";

            public const string BuscarDocumentosReqContOtroById = "[cont].[SpContrato] @Operacion = @Operacion, @contIdContrato = @contIdContrato";

            public const string BuscarAprobadoresContratoById = "[cont].[SpContrato] @Operacion = @Operacion, @contIdContrato=@contIdContrato";

            public const string EliminarContrato = "[cont].[SpContrato] @Operacion = @Operacion, @contIdContrato=@contIdContrato";

            public const string CrearContrato = @"[cont].[SpContrato]
                                                   @Operacion						            =   @Operacion
                                                  ,@contIdContrato					            =   @contIdContrato
                                                  ,@contCodEmpresa					            =   @contCodEmpresa
                                                  ,@contCodProveedor				            =   @contCodProveedor
                                                  ,@contCodRequerimiento			            =   @contCodRequerimiento
                                                  ,@contNombreRteLegalCtante		            =   @contNombreRteLegalCtante
                                                  ,@contCCRepresentanteCtante		            =   @contCCRepresentanteCtante
                                                  ,@contEmailRteLegalCtante                     =   @contEmailRteLegalCtante
                                                  ,@contDireccionNotificacionCtante	            =   @contDireccionNotificacionCtante
                                                  ,@contTelefonoCtante				            =   @contTelefonoCtante
                                                  ,@contCodGestorContrato			            =   @contCodGestorContrato
                                                  ,@contCodCoordinadorContrato		            =   @contCodCoordinadorContrato
                                                  ,@contCodGestorRiesgo				            =   @contCodGestorRiesgo
                                                  ,@contCodRequisitor				            =   @contCodRequisitor
                                                  ,@contCodUnidadNegocio			            =   @contCodUnidadNegocio
                                                  ,@contCodClaseContrato			            =   @contCodClaseContrato
                                                  ,@contAprobFinanciera				            =   @contAprobFinanciera
                                                  ,@contAprobCompras				            =   @contAprobCompras
                                                  ,@contAprobArea					            =   @contAprobArea
                                                  ,@contObjetoContrato				            =   @contObjetoContrato
                                                  ,@contCarateristicasEspecificas	            =   @contCarateristicasEspecificas
                                                  ,@contDuracionContrato			            =   @contDuracionContrato
                                                  ,@contVigenciaDesde				            =   @contVigenciaDesde
                                                  ,@contVigenciaHasta				            =   @contVigenciaHasta
                                                  ,@contValorContrato				            =   @contValorContrato
                                                  ,@contCodFormaPago				            =   @contCodFormaPago
                                                  ,@contRequierenAnticipos			            =   @contRequierenAnticipos
                                                  ,@contValorAnticipo				            =   @contValorAnticipo
                                                  ,@contRequiereIngresoPersonal		            =   @contRequiereIngresoPersonal
                                                  ,@contPresupuestado				            =   @contPresupuestado
                                                  ,@contCodTipoProrroga				            =   @contCodTipoProrroga
                                                  ,@contDuracionProrroga			            =   @contDuracionProrroga
                                                  ,@contPreavisoProrrogaDias		            =   @contPreavisoProrrogaDias
                                                  ,@contRequiereActaInicio			            =   @contRequiereActaInicio
                                                  ,@contRequiereActaLiquidacion		            =   @contRequiereActaLiquidacion
                                                  ,@contFechaLiquidacionEsperada	            =   @contFechaLiquidacionEsperada
                                                  ,@contTipoDocumento				            =   @contTipoDocumento
                                                  ,@contCodEstado					            =   @contCodEstado
                                                  ,@contObservacion                             =   @contObservacion
                                                  ,@contContactoContratista                     =   @contContactoContratista
                                                  ,@contEmailContactoContratista                =   @contEmailContactoContratista
                                                  ,@contNitContratista                          =   @contNitContratista
                                                  ,@CodUser							            =   @CodUser
                                                  ,@Info							            =   @Info
												  ,@CodUserUpdate                               =   @CodUserUpdate
                                                  ,@InfoUpdate                                  =   @InfoUpdate
                                                  ,@cadenaAprobadores                           =   @cadenaAprobadores
                                                  ,@cadenaDocumentosContrato		            =   @cadenaDocumentosContrato
                                                  ,@cadenaDocumentosProveedor		            =   @cadenaDocumentosProveedor
                                                  ,@cadenaDocumentosProveedorOtros	            =   @cadenaDocumentosProveedorOtros
                                                  ,@cadenaDocumentosPoliza	                    =   @cadenaDocumentosPoliza
                                                  ,@cadenaArchivos	                            =   @cadenaArchivos
												  ,@cadenaDocumentosContratoEliminados          =   @cadenaDocumentosContratoEliminados
                                                  ,@cadenaDocumentosProveedorElimminados        =   @cadenaDocumentosProveedorElimminados
                                                  ,@cadenaDocumentosProveedorOtrosElimminados   =   @cadenaDocumentosProveedorOtrosElimminados
                                                  ,@cadenaDocumentosPolizaEliminados            =   @cadenaDocumentosPolizaEliminados
                                                  ,@ContArchivoCompraNoPresupuestada            =   @ContArchivoCompraNoPresupuestada
                                                  ,@KeyFileCompraNoProsupuestada                =   @KeyFileCompraNoProsupuestada
                                                  ,@ContEmailContratante                        =   @ContEmailContratante
                                                  ,@ContArchivoActaInicio                       =   @ContArchivoActaInicio
                                                  ,@KeyFileActaInicio                           =   @KeyFileActaInicio
                                                  ,@ContFechaActaInicio                         =   @ContFechaActaInicio
                                                  ,@ContConsecutivoAlterno                      =   @ContConsecutivoAlterno
                                                  ,@ContCodContratoHistoricoActual              =   @ContCodContratoHistoricoActual
                                                  ,@ContCodTipoContrato                         =   @ContCodTipoContrato
                                                  ,@ContCodRepresentanteLegal                   =   @ContCodRepresentanteLegal
            ";

            public const string EditarContrato = @"[cont].[SpContrato]
                                                   @Operacion						            =   @Operacion
                                                  ,@contIdContrato					            =   @contIdContrato
                                                  ,@contCodEmpresa					            =   @contCodEmpresa
                                                  ,@contCodProveedor				            =   @contCodProveedor
                                                  ,@contCodRequerimiento			            =   @contCodRequerimiento
                                                  ,@contNombreRteLegalCtante		            =   @contNombreRteLegalCtante
                                                  ,@contCCRepresentanteCtante		            =   @contCCRepresentanteCtante
                                                  ,@contEmailRteLegalCtante                     =   @contEmailRteLegalCtante
                                                  ,@contDireccionNotificacionCtante	            =   @contDireccionNotificacionCtante
                                                  ,@contTelefonoCtante				            =   @contTelefonoCtante
                                                  ,@contCodGestorContrato			            =   @contCodGestorContrato
                                                  ,@contCodCoordinadorContrato		            =   @contCodCoordinadorContrato
                                                  ,@contCodGestorRiesgo				            =   @contCodGestorRiesgo
                                                  ,@contCodUnidadNegocio			            =   @contCodUnidadNegocio
                                                  ,@contCodClaseContrato			            =   @contCodClaseContrato
                                                  ,@contAprobFinanciera				            =   @contAprobFinanciera
                                                  ,@contAprobCompras				            =   @contAprobCompras
                                                  ,@contAprobArea					            =   @contAprobArea
                                                  ,@contObjetoContrato				            =   @contObjetoContrato
                                                  ,@contCarateristicasEspecificas	            =   @contCarateristicasEspecificas
                                                  ,@contDuracionContrato			            =   @contDuracionContrato
                                                  ,@contVigenciaDesde				            =   @contVigenciaDesde
                                                  ,@contVigenciaHasta				            =   @contVigenciaHasta
                                                  ,@contValorContrato				            =   @contValorContrato
                                                  ,@contCodFormaPago				            =   @contCodFormaPago
                                                  ,@contRequierenAnticipos			            =   @contRequierenAnticipos
                                                  ,@contValorAnticipo				            =   @contValorAnticipo
                                                  ,@contRequiereIngresoPersonal		            =   @contRequiereIngresoPersonal
                                                  ,@contPresupuestado				            =   @contPresupuestado
                                                  ,@contCodTipoProrroga				            =   @contCodTipoProrroga
                                                  ,@contDuracionProrroga			            =   @contDuracionProrroga
                                                  ,@contPreavisoProrrogaDias		            =   @contPreavisoProrrogaDias
                                                  ,@contRequiereActaInicio			            =   @contRequiereActaInicio
                                                  ,@contRequiereActaLiquidacion		            =   @contRequiereActaLiquidacion
                                                  ,@contFechaLiquidacionEsperada	            =   @contFechaLiquidacionEsperada
                                                  ,@contTipoDocumento				            =   @contTipoDocumento
                                                  ,@contCodEstado					            =   @contCodEstado
                                                  ,@contObservacion                             =   @contObservacion
                                                  ,@contContactoContratista                     =   @contContactoContratista
                                                  ,@contEmailContactoContratista                =   @contEmailContactoContratista
                                                  ,@contNitContratista                          =   @contNitContratista
                                                  ,@CodUserUpdate                               =   @CodUserUpdate
                                                  ,@InfoUpdate                                  =   @InfoUpdate
                                                  ,@cadenaAprobadores                           =   @cadenaAprobadores
                                                  ,@cadenaDocumentosContrato		            =   @cadenaDocumentosContrato
                                                  ,@cadenaDocumentosProveedor		            =   @cadenaDocumentosProveedor
                                                  ,@cadenaDocumentosProveedorOtros	            =   @cadenaDocumentosProveedorOtros
                                                  ,@cadenaDocumentosPoliza	                    =   @cadenaDocumentosPoliza
                                                  ,@cadenaArchivos	                            =   @cadenaArchivos
                                                  ,@cadenaDocumentosContratoEliminados          =   @cadenaDocumentosContratoEliminados
                                                  ,@cadenaDocumentosProveedorElimminados        =   @cadenaDocumentosProveedorElimminados
                                                  ,@cadenaDocumentosProveedorOtrosElimminados   =   @cadenaDocumentosProveedorOtrosElimminados
                                                  ,@cadenaDocumentosPolizaEliminados            =   @cadenaDocumentosPolizaEliminados
                                                  ,@ContArchivoCompraNoPresupuestada            =   @ContArchivoCompraNoPresupuestada
                                                  ,@KeyFileCompraNoProsupuestada                =   @KeyFileCompraNoProsupuestada
                                                  ,@ContEmailContratante                        =   @ContEmailContratante
                                                  ,@ContArchivoActaInicio                       =   @ContArchivoActaInicio
                                                  ,@KeyFileActaInicio                           =   @KeyFileActaInicio
                                                  ,@ContFechaActaInicio                         =   @ContFechaActaInicio
                                                  ,@ContConsecutivoAlterno                      =   @ContConsecutivoAlterno
                                                  ,@ContCodContratoHistoricoActual              =   @ContCodContratoHistoricoActual
                                                  ,@ContCodTipoContrato                         =   @ContCodTipoContrato
                                                  ,@ContCodRepresentanteLegal                   =   @ContCodRepresentanteLegal
            ";

            public const string ParametrosContrato = "[cont].[SpContrato] @Operacion = @Operacion";

            public const string AprobacionActoresContrato = "[cont].[SpContrato] @Operacion = @Operacion, @apcIdAprobadoresContrato = @apcIdAprobadoresContrato, @apcAprobacion = @apcAprobacion, @apcJustificacion = @apcJustificacion, @CodUserUpdate = @CodUserUpdate";

            public const string BuscarDetalleDelContratoPorId = "[cont].[SpContrato] @Operacion = @Operacion, @contIdContrato=@contIdContrato";

            public const string UpdateUrlContrato = "[cont].[SpContrato] @Operacion = @Operacion, @contIdContrato = @contIdContrato, @contUrlPdf = @contUrlPdf, @contUrlAbsolutePdf = @contUrlAbsolutePdf, @CodUserUpdate = @CodUserUpdate";

            public const string SolicitarDocumentosProveedor = "[cont].[SpContrato] @Operacion = @Operacion, @contIdContrato = @contIdContrato, @CodUserUpdate = @CodUserUpdate";

            public const string GuardarNotificacionNoProrroga = "[cont].[SpContrato] @Operacion = @Operacion, @contIdContrato = @contIdContrato, @contFechaNotificacionNoProrroga = @contFechaNotificacionNoProrroga, @CodArchivoNotificacionNoProrroga = @CodArchivoNotificacionNoProrroga, @contObservacionNoProrroga = @contObservacionNoProrroga, @KeyFileArchivoNotificacionNoProrroga = @KeyFileArchivoNotificacionNoProrroga, @cadenaArchivos = @cadenaArchivos, @CodUserUpdate = @CodUserUpdate";

            public const string GuardarNotificacionTerminacionAnticipada = "[cont].[SpContrato] @Operacion = @Operacion, @contIdContrato = @contIdContrato, @contFechaNotifTermAnticipada = @contFechaNotifTermAnticipada, @CodArchivoNotificacionTerminacion = @CodArchivoNotificacionTerminacion, @contObservacionTermAnticipada = @contObservacionTermAnticipada, @KeyFileArchivoNotificacionTerminacion = @KeyFileArchivoNotificacionTerminacion, @cadenaArchivos = @cadenaArchivos, @CodUserUpdate = @CodUserUpdate";

            public const string AsociarPolizasRenovadas = "[cont].[SpContrato] @Operacion = @Operacion, @contIdContrato = @contIdContrato, @cadenaArchivos = @cadenaArchivos, @cadenaDocumentosPoliza = @cadenaDocumentosPoliza, @CodUserUpdate = @CodUserUpdate";

            public const string GuardarActaLiquidacion = "[cont].[SpContrato] @Operacion = @Operacion, @contIdContrato = @contIdContrato, @cadenaArchivos = @cadenaArchivos, @CodArchivoActaLiquidacion = @CodArchivoActaLiquidacion, @KeyFileArchivoActaLiquidacion = @KeyFileArchivoActaLiquidacion, @CodUserUpdate = @CodUserUpdate";

            public const string AprobacionAdministrador = "[cont].[SpContrato] @Operacion = @Operacion, @contIdContrato = @contIdContrato";

            public static class Comentarios 
            {
                public const string GuardarComentario = "[cont].[SpContQuestionAnswer] @Operacion = @Operacion, @IdQuestionAnswer = @IdQuestionAnswer, @IdContrato = @IdContrato, @IdUsuario = @IdUsuario, @EsGestor = @EsGestor, @Comentario = @Comentario, @ContieneAdjunto = @ContieneAdjunto, @EsPrivado = @EsPrivado, @Estado = @Estado, @FilePath = @FilePath, @FileRelativo = @FileRelativo, @FileSize = @FileSize, @FileExt = @FileExt, @CodUser = @CodUser, @Info = @Info";
                public const string BuscarComentariosContrato = "[cont].[SpContQuestionAnswer] @Operacion = @Operacion, @IdContrato = @IdContrato";
            }

            public static class Reportes 
            {
                public const string Listado = "[cont].[SpListadoContratos] @Operacion = @Operacion, @NumeroContrato = @NumeroContrato, @CedulaProveedor = @CedulaProveedor, @TipoContrato = @TipoContrato, @VigenciaDesde = @VigenciaDesde, @VigenciaHasta = @VigenciaHasta, @Estado = @Estado";
            }
        }

        public static class SolicitudModificacion
        {
            public const string SolicitarModificacion = "[cont].[SpContrato] " +
                                                        "@Operacion = @Operacion, " +
                                                        "@contIdContrato = @contIdContrato, " +
                                                        "@ContObjetoContrato = @ContObjetoContrato, " +
                                                        "@ContCarateristicasEspecificas = @ContCarateristicasEspecificas, " +
                                                        "@ContDuracionContrato = @ContDuracionContrato, " +
                                                        "@ContVigenciaDesde = @ContVigenciaDesde, " +
                                                        "@ContVigenciaHasta = @ContVigenciaHasta, " +
                                                        "@ContValorContrato = @ContValorContrato, " +
                                                        "@ContCodFormaPago = @ContCodFormaPago, " +
                                                        "@contObservacion = @contObservacion, " +
                                                        "@cadenaAprobadores = @cadenaAprobadores, " +
                                                        "@oscConsecutivoAlterno = @oscConsecutivoAlterno," +
                                                        "@ContCodRepresentanteLegal = @ContCodRepresentanteLegal,"+
                                                        "@CodUserUpdate = @CodUserUpdate";
        }

        public static class PasosContrato
        {
            public const string BuscarTimelineContratoById = "[cont].[SpPasosContrato] @Operacion = @Operacion, @pcnCodContrato = @pcnCodContrato";
        }

        public static class Proveedor
        {
            public const string AprobarDesaprobarInspektor = "[prv].[SpProveedores] @Operacion = @Operacion, @prvIdProveedores = @prvIdProveedores, @aprobarInspektor = @aprobarInspektor, @CodUserUpdate = @CodUserUpdate";
            public const string ReenviarNotificacionCodigoValidacion = "[prv].[SpProveedores] @Operacion = @Operacion, @prvNit = @prvNit";
            public const string CambiarCorreoProveedor = "[prv].[SpProveedores] @Operacion = @Operacion, @prvNit = @prvNit, @prvMail = @prvMail, @prvMailAlterno = @prvMailAlterno, @CodUserUpdate = @CodUserUpdate";
        }

        public static class Tercero
        {
            public const string AprobarDesaprobarInspektor = "[prv].[SpTerceros] @Operacion = @Operacion, @prvIdProveedores = @prvIdProveedores, @aprobarInspektor = @aprobarInspektor, @CodUserUpdate = @CodUserUpdate";
            public const string ReenviarNotificacionCodigoValidacion = "[prv].[SpTerceros] @Operacion = @Operacion, @prvNit = @prvNit";
            public const string CambiarCorreoProveedor = "[prv].[SpTerceros] @Operacion = @Operacion, @prvNit = @prvNit, @prvMail = @prvMail, @prvMailAlterno = @prvMailAlterno, @CodUserUpdate = @CodUserUpdate";
        }

        public static class DocumentosProveedor
        {
            public const string AsociarDocumentos = @"[cont].[SpContrato]
                                                   @Operacion						=   @Operacion
                                                  ,@contIdContrato					=   @contIdContrato
                                                  ,@cadenaDocumentosProveedor		=   @cadenaDocumentosProveedor
                                                  ,@cadenaDocumentosProveedorOtros	=   @cadenaDocumentosProveedorOtros
                                                  ,@cadenaArchivos	                =   @cadenaArchivos
                                                  ,@cadenaDocumentosProveedorElimminados        = @cadenaDocumentosProveedorElimminados
                                                  ,@cadenaDocumentosProveedorOtrosElimminados   = @cadenaDocumentosProveedorOtrosElimminados
                                                  ,@CodUserUpdate = @CodUserUpdate";

            public const string RechazarDocumentos = @"[cont].[SpContrato]
                                                   @Operacion						            =   @Operacion
                                                  ,@contIdContrato					            =   @contIdContrato
                                                  ,@contJustificacionRechazo                    =   @contJustificacionRechazo
                                                  ,@cadenaArchivos	                            =   @cadenaArchivos
                                                  ,@cadenaDocumentosProveedor		            =   @cadenaDocumentosProveedor
                                                  ,@cadenaDocumentosProveedorElimminados        =   @cadenaDocumentosProveedorElimminados
                                                  ,@CodUserUpdate = @CodUserUpdate";
        }

        public static class Seguimientos
        {
            public const string CrearSeguimiento = @"[cont].[SpContrato]
                                                   @Operacion						=   @Operacion
                                                  ,@contIdContrato					=   @contIdContrato
                                                  ,@scoIdSeguimiento                =   @scoIdSeguimiento
                                                  ,@scoFecha                        =   @scoFecha
                                                  ,@scoObservacion                  =   @scoObservacion
                                                  ,@scoPagosEfectuados              =   @scoPagosEfectuados
                                                  ,@codArchivoSeguimiento           =   @codArchivoSeguimiento
                                                  ,@keyFileSeguimiento              =   @keyFileSeguimiento
                                                  ,@cadenaArchivos	                =   @cadenaArchivos
                                                  ,@scoTipo	                        =   @scoTipo
                                                  ,@contCodGestorContrato           =   @contCodGestorContrato
                                                  ,@contCodGestorRiesgo             =   @contCodGestorRiesgo
                                                  ,@CodUserUpdate = @CodUserUpdate";
        }
    }
}