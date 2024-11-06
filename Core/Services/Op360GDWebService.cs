using Core.DTOs;
using Core.DTOs.GDWebDto;
using Core.Entities;
using Core.Entities.GDWebEntities;
using Core.Interfaces;
using Core.QueryFilters.QueryFiltersGDWeb;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class Op360GDWebService : IOp360GDWebService
    {
        private readonly IUnitOfWork _unitOfWork;

        public Op360GDWebService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto<Op360GD_CargaInicialDto>> CargaInicialAsync()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360GD_CargaInicialDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi");  //sp de ejemplo
        }

        public Task<ResponseDto<Op360GD_ResumenGlobalOrdenesDto>> GetResumenGlobalOrdenes()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_ResumenGlobalOrdenesDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi");  //sp de ejemplo
        }

        public Task<ResponseDto<Op360GD_Orden_Response>> Consultar_Ordenes_Contratistas(QueryOp360GD_OrdenesContratistas parameters)
        {
            // Asigna a parameters.id_contratista_persona el valor de parameters.id_contratista_persona si no es nulo; de lo contrario, asigna -2.
            parameters.id_contratista_persona = parameters.id_contratista_persona ?? -2;
            parameters.id_zona = parameters.id_zona ?? -2;
            parameters.id_estado_orden = parameters.id_estado_orden ?? -2;
            parameters.id_orden = parameters.id_orden ?? -1;
            parameters.pageSize = parameters.pageSize ?? 1000000;
            parameters.pageNumber = parameters.pageNumber ?? 0;
            parameters.sortColumn = parameters.sortColumn ?? "id_orden";
            parameters.sortDirection = parameters.sortDirection ?? "desc";

            var param = JsonConvert.SerializeObject(parameters);
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_Orden_Response>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public Task<ResponseDto<Op360GD_Orden_Agrupada_Response>> ConsultarOrdenesAgrupadasAreaCentral(QueryOp360GDOrdenesAgrupada parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_Orden_Agrupada_Response>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public Task<ResponseDto<Op360GD_OrdenById_Response>> ConsultarOrdenXId(QueryOp360GD_Orden parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_OrdenById_Response>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto> GenerarOT(Op360GD_GenerarOTDto parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummigd.prc_generar_orden_trabajo", param);  //sp cascaron
        }

        public async Task<ResponseDto> asignarTareaAOT(Op360GD_asignarTareaAOTDto parameters)
        {
            var param = JsonConvert.SerializeObject(parameters) ;
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto> certificarOT(Op360GD_certificarOTDto parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummigd.prc_certificar_orden_trabajo", param); //sp cascaron
        }

        public async Task<ResponseDto> RechazarSolicitud(Op360GD_RechazarSolicitudDto parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummi.prc_dummi_parametros", param); //sp de ejemplo
        }

        public Task<ResponseDto<Op360GD_ObtenerListaCircuitosDto>> ObtenerListaCircuitos()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_ObtenerListaCircuitosDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_obtener_lista_circuitos");  //sp cascaron
        }

        public async Task<ResponseDto> CrearCircuito(Op360GD_CrearCircuitoDto op360GD_CrearCircuito)
        {
            var param = JsonConvert.SerializeObject(op360GD_CrearCircuito);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_crear_circuitos", param);  //sp cascaron
        }

        public async Task<ResponseDto> ActualizarCircuito(Op360GD_ActualizarCircuitoDto op360GD_ActualizarCircuito)
        {
            var param = JsonConvert.SerializeObject(op360GD_ActualizarCircuito);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummigd.prc_actualizar_circuito", param);  //sp cascaron
        }

        public Task<ResponseDto<Op360GD_ObtenerListaSubEstacionDto>> ObtenerListaSubEstacion()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_ObtenerListaSubEstacionDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_obtener_lista_sub_estacion");  //sp cascaron
        }

        public async Task<ResponseDto> CrearSubEstacion(Op360GD_CrearSubEstacionDto op360GD_CrearSubEstacion)
        {
            var param = JsonConvert.SerializeObject(op360GD_CrearSubEstacion);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_crear_sub_estacion", param);  //sp cascaron
        }

        public async Task<ResponseDto> ActualizarSubEstacion(Op360GD_ActualizarSubEstacionDto op360GD_ActualizarSubEstacion)
        {
            var param = JsonConvert.SerializeObject(op360GD_ActualizarSubEstacion);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummigd.prc_actualizar_sub_estacion", param);  //sp cascaron
        }

        public Task<ResponseDto<Op360GD_GetListaReferenciaIconosDto>> GetListaReferenciaIconos()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_GetListaReferenciaIconosDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_obtener_lista_referencia_iconos");  //sp cascaron
        }

        public Task<ResponseDto<Op360GD_GetListaTiposTrabajoDto>> GetListaTiposTrabajo()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_GetListaTiposTrabajoDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_obtener_lista_tipos_trabajo");  //sp cascaron
        }

        public async Task<ResponseDto> CrearTipoTrabajo(Op360GD_CrearTipoTrabajoDto op360GD_CrearTipoTrabajo)
        {
            var param = JsonConvert.SerializeObject(op360GD_CrearTipoTrabajo);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_crear_tipo_trabajo", param);  //sp cascaron
        }

        public async Task<ResponseDto> ActualizarTipoTrabajo(Op360GD_ActualizarTipoTrabajoDto op360GD_ActualizarTipoTrabajo)
        {
            var param = JsonConvert.SerializeObject(op360GD_ActualizarTipoTrabajo);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummigd.prc_actualizar_tipo_trabajo", param);  //sp cascaron
        }

        public Task<ResponseDto<Op360GD_GetListaTipoAvisosDto>> GetListaTipoAvisos()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_GetListaTipoAvisosDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_obtener_lista_tipo_avisos");  //sp cascaron
        }

        public async Task<ResponseDto> CrearTipoAviso(Op360GD_CrearTipoAvisoDto op360GD_CrearTipoAviso)
        {
            var param = JsonConvert.SerializeObject(op360GD_CrearTipoAviso);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_crear_tipo_aviso", param);  //sp cascaron
        }

        public async Task<ResponseDto> ActualizarTipoAviso(Op360GD_ActualizarTipoAvisoDto op360GD_ActualizarTipoAviso)
        {
            var param = JsonConvert.SerializeObject(op360GD_ActualizarTipoAviso);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummigd.prc_actualizar_tipo_aviso", param);  //sp cascaron
        }

        public Task<ResponseDto<Op360GD_ObtenerOrdenesDashBoardDto>> ObtenerOrdenesDashBoard()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_ObtenerOrdenesDashBoardDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_obtener_ordenes_dashboard");  //sp cascaron
        }

        public Task<ResponseDto<Op360GD_ObtenerReporteOrdenesTiemposDto>> ObtenerReporteOrdenesTiempos()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_ObtenerReporteOrdenesTiemposDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_obtener_reporte_ordenes_tiempos");  //sp cascaron
        }

        public Task<ResponseDto<Op360GD_ObtenerOrdenesAdministrarDto>> ObtenerOrdenesAdministrar()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_ObtenerOrdenesAdministrarDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_obtener_ordenes_administrar");  //sp cascaron
        }

        public Task<ResponseDto<Op360GD_GenerarPdfActaCierreDto>> GenerarPdfActaCierre()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_GenerarPdfActaCierreDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_generar_pdf_acta_cierre");  //sp cascaron
        }

        public Task<ResponseDto<Op360GD_ObtenerRecorridoxBrigadaRangoTiempoDto>> ObtenerRecorridoxBrigadaRangoTiempo()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_ObtenerRecorridoxBrigadaRangoTiempoDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_obtener_recorrido_x_brigada_rango_tiempo");  //sp cascaron
        }

        public Task<ResponseDto<Op360GD_ObtenerInfoAvisoBasicoGeoreferenciadoDto>> ObtenerInfoAvisoBasicoGeoreferenciado()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_ObtenerInfoAvisoBasicoGeoreferenciadoDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_obtener_info_aviso_basico_georeferenciado");  //sp cascaron
        }

        public Task<ResponseDto<Op360GD_ObtenerInfoAvisoCompletoDto>> ObtenerInfoAvisoCompleto()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_ObtenerInfoAvisoCompletoDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_obtener_info_aviso_completo");  //sp cascaron
        }

        public Task<ResponseDto<Op360GD_ObtenerInfoAvisoBitacoraDto>> ObtenerInfoAvisoBitacora()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_ObtenerInfoAvisoBitacoraDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_obtener_info_aviso_bitacora");  //sp cascaron
        }

        public Task<ResponseDto<Op360GD_ObtenerInfoAvisoCertificadoDto>> ObtenerInfoAvisoCertificado()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_ObtenerInfoAvisoCertificadoDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_obtener_info_aviso_certificado");  //sp cascaron
        }

        public async Task<ResponseDto> CerrarOrdenTrabajo(Op360GD_CerrarOrdenTrabajoDto op360GD_CerrarOrdenTrabajo)
        {
            var param = JsonConvert.SerializeObject(op360GD_CerrarOrdenTrabajo);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummigd.prc_cerrar_orden_trabajo", param);  //sp cascaron
        }

        public Task<ResponseDto<Op360GD_ObtenerInfoOTxTareasDto>> ObtenerInfoOTxTareas()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_ObtenerInfoOTxTareasDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_obtener_info_ot_x_tareas");  //sp cascaron
        }

        public async Task<ResponseDto> AvisoPendienteOrdenTrabajo(Op360GD_AvisoPendienteOrdenTrabajoDto op360GD_AvisoPendienteOrdenTrabajo)
        {
            var param = JsonConvert.SerializeObject(op360GD_AvisoPendienteOrdenTrabajo);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummigd.prc_aviso_pendiente_orden_trabajo", param);  //sp cascaron
        }

        public async Task<ResponseDto> AvisoImprocedenteOrdenTrabajo(Op360GD_AvisoImprocedenteOrdenTrabajoDto op360GD_AvisoImprocedenteOrdenTrabajo)
        {
            var param = JsonConvert.SerializeObject(op360GD_AvisoImprocedenteOrdenTrabajo);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummigd.prc_aviso_improcedente_orden_trabajo", param);  //sp cascaron
        }

        public async Task<ResponseDto> CerrarTurnoBrigada(Op360GD_CerrarTurnoBrigadaDto op360GD_CerrarTurnoBrigada)
        {
            var param = JsonConvert.SerializeObject(op360GD_CerrarTurnoBrigada);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummigd.prc_cerrar_turno_brigada", param);  //sp cascaron
        }

        public async Task<ResponseDto> GestionarPeligro(Op360GD_GestionarPeligroDto op360GD_GestionarPeligro)
        {
            var param = JsonConvert.SerializeObject(op360GD_GestionarPeligro);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummigd.prc_gestionar_peligro", param);  //sp cascaron
        }

        public async Task<ResponseDto> CambiarSector(Op360GD_CambiarSectorDto op360GD_CambiarSector)
        {
            var param = JsonConvert.SerializeObject(op360GD_CambiarSector);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummigd.prc_cambiar_sector", param);  //sp cascaron
        }

        public async Task<ResponseDto> DeclararImprocedente(Op360GD_DeclararImprocedenteDto op360GD_DeclararImprocedente)
        {
            var param = JsonConvert.SerializeObject(op360GD_DeclararImprocedente);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummigd.prc_declarar_improcedente", param);  //sp cascaron
        }

        public async Task<ResponseDto> CargueParamCertiAvisos(Op360GD_CargueParamCertiAvisosDto op360GD_CargueParamCertiAvisos)
        {
            var param = JsonConvert.SerializeObject(op360GD_CargueParamCertiAvisos);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_cargue_param_certi_avisos", param);  //sp cascaron
        }

        public async Task<ResponseDto> CargueTurnos(Op360GD_CargueTurnosDto op360GD_CargueTurnos)
        {
            var param = JsonConvert.SerializeObject(op360GD_CargueTurnos);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_cargue_turnos", param);  //sp cascaron
        }

        public Task<ResponseDto<Op360GD_ReporteLiquidacionxCumplimientoTurnosDto>> ReporteLiquidacionxCumplimientoTurnos()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_ReporteLiquidacionxCumplimientoTurnosDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_reporte_liquidacion_x_cumplimiento_turnos");  //sp cascaron
        }

        public Task<ResponseDto<Op360GD_ReporteLiquidacionxBrigadaDto>> ReporteLiquidacionxBrigada()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_ReporteLiquidacionxBrigadaDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_reporte_liquidacion_x_brigada");  //sp cascaron
        }

        public Task<ResponseDto<Op360GD_ReporteLiquidacionConsolidadoDto>> ReporteLiquidacionConsolidado()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_ReporteLiquidacionConsolidadoDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_reporte_liquidacion_consolidado");  //sp cascaron
        }

        public Task<ResponseDto<Op360GD_ConsultadeTurnosDto>> ConsultaDeTurnos()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_ConsultadeTurnosDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_consulta_de_turnos");  //sp cascaron
        }

        public async Task<ResponseDto> CrearTurno(Op360GD_CrearTurnoDto op360GD_CrearTurno)
        {
            var param = JsonConvert.SerializeObject(op360GD_CrearTurno);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_crear_turno", param);  //sp cascaron
        }

        public async Task<ResponseDto> EliminarTurno(Op360GD_EliminarTurnoDto op360GD_EliminarTurno)
        {
            var param = JsonConvert.SerializeObject(op360GD_EliminarTurno);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummigd.prc_eliminar_turno", param);  //sp cascaron
        }
    }
}
