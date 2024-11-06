using Core.DTOs;
using Core.DTOs.GDWebDto;
using Core.Entities;
using Core.Entities.ASEGREDWebEntities;
using Core.Entities.GDWebEntities;
using Core.QueryFilters;
using Core.QueryFilters.QueryFiltersASEGREDWeb;
using Core.QueryFilters.QueryFiltersGDWeb;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IOp360GDWebService
    {
        Task<ResponseDto<Op360GD_CargaInicialDto>> CargaInicialAsync();
        Task<ResponseDto<Op360GD_ResumenGlobalOrdenesDto>> GetResumenGlobalOrdenes();
        Task<ResponseDto<Op360GD_Orden_Response>> Consultar_Ordenes_Contratistas(QueryOp360GD_OrdenesContratistas parameters);
        Task<ResponseDto<Op360GD_Orden_Agrupada_Response>> ConsultarOrdenesAgrupadasAreaCentral(QueryOp360GDOrdenesAgrupada parameters);
        Task<ResponseDto<Op360GD_OrdenById_Response>> ConsultarOrdenXId(QueryOp360GD_Orden parameters);
        Task<ResponseDto> GenerarOT(Op360GD_GenerarOTDto parameters);
        Task<ResponseDto> asignarTareaAOT(Op360GD_asignarTareaAOTDto parameters);
        Task<ResponseDto> certificarOT(Op360GD_certificarOTDto parameters);
        Task<ResponseDto> RechazarSolicitud(Op360GD_RechazarSolicitudDto parameters);
        Task<ResponseDto<Op360GD_ObtenerListaCircuitosDto>> ObtenerListaCircuitos();
        Task<ResponseDto> CrearCircuito(Op360GD_CrearCircuitoDto op360GD_CrearCircuito);
        Task<ResponseDto> ActualizarCircuito(Op360GD_ActualizarCircuitoDto op360GD_ActualizarCircuito);
        Task<ResponseDto<Op360GD_ObtenerListaSubEstacionDto>> ObtenerListaSubEstacion();
        Task<ResponseDto> CrearSubEstacion(Op360GD_CrearSubEstacionDto op360GD_CrearSubEstacion);
        Task<ResponseDto> ActualizarSubEstacion(Op360GD_ActualizarSubEstacionDto op360GD_ActualizarSubEstacion);
        Task<ResponseDto<Op360GD_GetListaReferenciaIconosDto>> GetListaReferenciaIconos();
        Task<ResponseDto<Op360GD_GetListaTiposTrabajoDto>> GetListaTiposTrabajo();
        Task<ResponseDto> CrearTipoTrabajo(Op360GD_CrearTipoTrabajoDto op360GD_CrearTipoTrabajo);
        Task<ResponseDto> ActualizarTipoTrabajo(Op360GD_ActualizarTipoTrabajoDto op360GD_ActualizarTipoTrabajo);
        Task<ResponseDto<Op360GD_GetListaTipoAvisosDto>> GetListaTipoAvisos();
        Task<ResponseDto> CrearTipoAviso(Op360GD_CrearTipoAvisoDto op360GD_CrearTipoAviso);
        Task<ResponseDto> ActualizarTipoAviso(Op360GD_ActualizarTipoAvisoDto op360GD_ActualizarTipoAviso);
        Task<ResponseDto<Op360GD_ObtenerOrdenesDashBoardDto>> ObtenerOrdenesDashBoard();
        Task<ResponseDto<Op360GD_ObtenerReporteOrdenesTiemposDto>> ObtenerReporteOrdenesTiempos();
        Task<ResponseDto<Op360GD_ObtenerOrdenesAdministrarDto>> ObtenerOrdenesAdministrar();
        Task<ResponseDto<Op360GD_GenerarPdfActaCierreDto>> GenerarPdfActaCierre();
        Task<ResponseDto<Op360GD_ObtenerRecorridoxBrigadaRangoTiempoDto>> ObtenerRecorridoxBrigadaRangoTiempo();
        Task<ResponseDto<Op360GD_ObtenerInfoAvisoBasicoGeoreferenciadoDto>> ObtenerInfoAvisoBasicoGeoreferenciado();
        Task<ResponseDto<Op360GD_ObtenerInfoAvisoCompletoDto>> ObtenerInfoAvisoCompleto();
        Task<ResponseDto<Op360GD_ObtenerInfoAvisoBitacoraDto>> ObtenerInfoAvisoBitacora();
        Task<ResponseDto<Op360GD_ObtenerInfoAvisoCertificadoDto>> ObtenerInfoAvisoCertificado();
        Task<ResponseDto> CerrarOrdenTrabajo(Op360GD_CerrarOrdenTrabajoDto op360GD_CerrarOrdenTrabajo);
        Task<ResponseDto<Op360GD_ObtenerInfoOTxTareasDto>> ObtenerInfoOTxTareas();
        Task<ResponseDto> AvisoPendienteOrdenTrabajo(Op360GD_AvisoPendienteOrdenTrabajoDto op360GD_AvisoPendienteOrdenTrabajo);
        Task<ResponseDto> AvisoImprocedenteOrdenTrabajo(Op360GD_AvisoImprocedenteOrdenTrabajoDto op360GD_AvisoImprocedenteOrdenTrabajo);
        Task<ResponseDto> CerrarTurnoBrigada(Op360GD_CerrarTurnoBrigadaDto op360GD_CerrarTurnoBrigada);
        Task<ResponseDto> GestionarPeligro(Op360GD_GestionarPeligroDto op360GD_GestionarPeligro);
        Task<ResponseDto> CambiarSector(Op360GD_CambiarSectorDto op360GD_CambiarSector);
        Task<ResponseDto> DeclararImprocedente(Op360GD_DeclararImprocedenteDto op360GD_DeclararImprocedente);
        Task<ResponseDto> CargueParamCertiAvisos(Op360GD_CargueParamCertiAvisosDto op360GD_CargueParamCertiAvisos);
        Task<ResponseDto> CargueTurnos(Op360GD_CargueTurnosDto op360GD_CargueTurnos);
        Task<ResponseDto<Op360GD_ReporteLiquidacionxCumplimientoTurnosDto>> ReporteLiquidacionxCumplimientoTurnos();
        Task<ResponseDto<Op360GD_ReporteLiquidacionxBrigadaDto>> ReporteLiquidacionxBrigada();
        Task<ResponseDto<Op360GD_ReporteLiquidacionConsolidadoDto>> ReporteLiquidacionConsolidado();
        Task<ResponseDto<Op360GD_ConsultadeTurnosDto>> ConsultaDeTurnos();
        Task<ResponseDto> CrearTurno(Op360GD_CrearTurnoDto op360GD_CrearTurno);
        Task<ResponseDto> EliminarTurno(Op360GD_EliminarTurnoDto op360GD_EliminarTurno);
    }
}
