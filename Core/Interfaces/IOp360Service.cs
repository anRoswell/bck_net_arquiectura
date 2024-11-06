namespace Core.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Dtos.FiltrosDto;
    using Core.DTOs;
    using Core.DTOs.ASEGREDWebDto;
    using Core.DTOs.FilesDto;
    using Core.DTOs.FiltrosDto;
    using Core.DTOs.ObtenerReporteTraza;
    using Core.Entities;
    using Core.Entities.AreaCentralVersion2;
    using Core.QueryFilters;
    using Core.QueryFilters.QueryFiltersSCRWeb;

    public interface IOp360Service
    {
        /*
        * fecha: 19/12/2023
        * clave: 5e6ewrt546weasdf _03
        * clave: 5e6ewrtLOGINEXTERNO6weasdf _03
        * carlos vargas
        */
        #region Seguridad
        Task<Usuarios_Perfiles> ConsultarUsusariosxPerfiles(QueryOp360Seguridad parameters);
        Task<Usuarios_Perfiles> ValidarLoginExterno(string Id_Usuario,string Token_Apex);
        #endregion

        /*
        * fecha: 19/12/2023
        * clave: aaaaORDENESa65sd4f65sdf _03
        * carlos vargas
        */
        #region Ordenes
        Task<ResponseDto<Op360ServerSideTestDto>> ServerSideTest(QueryOp360ServerSideTest parameters, bool ExportExcel = false);

        /// <summary>
        /// Retorna las ordenes de trabajo para el perfil de oficina central        
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<Aire_Scr_OrdenAreaCentral_Response>> Consultar_Ordenes_Area_Central(QueryOp360Ordenes parameters, bool ExportExcel = false);

        Task<ResponseDto<Aire_Scr_ReporteEjectuados_Response>> Consultar_Reporte_Ejecutados(QueryOp360ReporteEjecutados parameters, bool ExportExcel = false);

        Task<ResponseDto<Aire_Scr_ReporteEjectuados_Response>> Consultar_Reporte_Ejecutados_Contratistas(QueryOp360ReporteEjecutadosContratistas parameters, bool ExportExcel = false);

        /// <summary>
        /// Version 2 para retornar las ordenes de trabajo para el perfil de oficina central        
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<Aire_Scr_OrdenAreaCentral_Response_Version2>> Consultar_Ordenes_Area_CentralVersion2(QueryOp360OrdenesV2 parameters, bool ExportExcel = false);

        /// <summary>
        /// Retorna el listado de ordenes de trabajo para el dashboard de area central
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<Aire_Scr_Orden_DashBoard_AreaCentral_Response>> Consultar_Ordenes_DashBoard_Area_Central(QueryOp360OrdenesDashBoard parameters);

        /// <summary>
        /// Retorna el listado de ordenes de trabajo para el dashboard de contratistas
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<Aire_Scr_Orden_DashBoard_AreaCentral_Response>> Consultar_Ordenes_DashBoard_Contratistas(QueryOp360OrdenesDashBoardContratista parameters);

        /// <summary>
        /// consultar orden de trabajo por Id
        /// </summary>
        /// <param name="StoreProcedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<Aire_Scr_OrdenById_Response>> Consultar_Orden_Por_Id(QueryOp360Orden parameters);

        /// <summary>
        /// Retorna la lista de los exceles cargados para cargue masivo de ordenes
        /// </summary>
        /// <param name="StoreProcedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<Archivos_Instancia_Response>> Consultar_Archivos_Instancia(QueryOp360ArchivosInstancia parameters, bool ExportExcel = false);

        /// <summary>
        /// Retorna la lista de los exceles cargados para cargue masivo de ordenes
        /// </summary>
        /// <param name="StoreProcedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<Archivos_Instancia_Detalle_Response>> Consultar_Archivos_Instancia_Detalle(QueryOp360ArchivosInstanciaDetalle parameters, bool ExportExcel = false);

        /// <summary>
        /// Retorna las ordenes de trabajo 
        /// </summary>
        /// <param name="StoreProcedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<Aire_Scr_Orden_Agrupada_Response>> Consultar_Ordenes_Agrupadas_Area_Central(QueryOp360OrdenesAgrupada parameters);

        /// <summary>
        /// Asignar Aliados o Contratistas 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto> Gestionar_Ordenes_Contratista_Asignar(QueryOp360GestionarContratistas parameters);

        /// <summary>
        /// DesAsignar Aliados o Contratistas 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto> Gestionar_Ordenes_Contratista_DesAsignar(QueryOp360GestionarContratistas parameters);

        /// <summary>
        /// Asignar Brigadas o Técnicos 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto> Gestionar_Ordenes_Brigada_Asignar(QueryOp360GestionarBrigadas parameters);

        /// <summary>
        /// Deasignar Brigadas o Técnicos 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto> Gestionar_Ordenes_Brigada_DesAsignar(QueryOp360GestionarBrigadas parameters);

        /// <summary>
        /// Retorna los parametros iniciales para el modulo de ordenes en area central  
        /// </summary>
        /// <param name="StoreProcedure"></param>
        /// <returns></returns>
        Task<ResponseDto<Op360ParametrosInicialesAreaCentralDto>> GetAireScrOrdParametrosIniciales();

        /// <summary>
        /// Retorna los parametros iniciales para el modulo de ordenes Contratistas
        /// </summary>
        /// <param name="StoreProcedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<Op360ParametrosInicialesContratistaDto>> GetParametrosInicialesContratistas(QueryOp360ObtenerBrigadas parameters);

        /// <summary>
        /// Retorna la info del cliente por el nic o el nis
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<Aire_ScrOrdConsultaClienteNicNis>> ConsultarClienteByNicNis(QueryOp360NicNis parameters);

        /// <summary>
        /// Crea una orden de trabajo por medio de un formulario.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<FormOrdenRequest>> Guardar_Orden_Formdata(FormOrdenRequest parameters);

        /// <summary>
        /// Carga la informacion de las ordenes de trabajo por medio de un excel a una tabla temporal.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<FormOrdenRegistro>> Guardar_Orden_Excel(FormOrdenRegistro parameters);
        Task<ResponseDto<FormOrdenRegistro>> Guardar_Orden_ExcelBorreme(FormOrdenRegistro parameters);

        /// <summary>
        /// Procesa la información de las ordenes de trabajo cargadas en la tabla temporal y las guarda en la tabla definitiva.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<FormdataResponseMasivo>> Procesar_Masivo_Creacion_Ordenes_SCR(QueryOp360CargueMasivoValidacion parameters);

        /// <summary>
        /// Procesa la información de las ordenes de trabajo cargadas en la tabla temporal y las guarda en la tabla definitiva.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<FormdataResponseMasivo>> Guardar_Orden_Masivo_Final_Gos(QueryOp360CargueMasivoValidacion parameters);

        /// <summary>
        /// Retorna las ordenes de trabajo para el perfil de contratistas
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<Aire_Scr_Orden_Response>> Consultar_Ordenes_Contratistas(QueryOp360OrdenesContratistas parameters, bool ExportExcel = false);

        /// <summary>
        /// Consultar datos contratista por identificacion
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<Op360Contratistas>> ConsultarContratistaPorIdentificacion(QueryOp360Contratistas parameters);

        /// <summary>
        /// Retorna los parametros iniciales para el formulario de filtros
        /// </summary>
        /// <param name="StoreProcedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<Op360ParametrosInicialesFiltrosDto>> GetParametrosInicialesFiltros();

        /// <summary>
        /// Retorna resumen general de registros en la tabla de ordenes
        /// </summary>
        /// <param name="StoreProcedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<Op360ResumenGlobalOrdenesDto>> GetResumen_Global_Ordenes();

        /// Obtiene los filtros.
        /// </summary>
        /// <returns>Response.</returns>
        Task<ResponseDto<IList<GetFiltroResponseDto>>> ConsultarFiltros();

        /// <summary>
        /// Crea el filtro.
        /// </summary>
        /// <param name="createFiltroRequestDto">Parametros de entrada.</param>
        /// <returns>Response.</returns>
        Task<ResponseDto<CreateFiltroResponseDto>> CrearFiltro(CreateFiltroRequestDto createFiltroRequestDto);

        /// <summary>
        /// Actualiza el filtro.
        /// </summary>
        /// <param name="updateFiltroRequestDto">Parametros de entrada.</param>
        /// <returns>Response.</returns>
        Task<ResponseDto> ActualizarFiltro(UpdateFiltroRequestDto updateFiltroRequestDto);

        /// <summary>
        /// Elimina el filtro.
        /// </summary>
        /// <param name="deleteFiltroRequestDto">Parametros de entrada.</param>
        /// <returns>Response.</returns>
        Task<ResponseDto> EliminarFiltro(DeleteFiltroRequestDto deleteFiltroRequestDto);

        /// <summary>
        /// Consultar trazabilidad de ordenes
        /// </summary>
        /// <param name="StoreProcedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<IList<Op360Ordenes_TrazabilidadDto>>> GetConsultar_Trazabilidad_Ordenes(QueryOp360GetOrdenes parameters);

        /// <summary>
        /// Consultar georreferenciacion para Area Central
        /// </summary>
        /// <param name="StoreProcedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<IList<Op360GeoreferenciacionFechaDto>>> GetConsultar_Georreferencia_AreaCentral(QueryOp360GetFechaGeoreferenciacion parameters);

        /// <summary>
        /// Consultar georreferenciacion para Contratista Persona
        /// </summary>
        /// <param name="StoreProcedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<IList<Op360GeoreferenciacionFechaDto>>> GetConsultar_Georreferencia_ContratistaPersona(QueryOp360GetFechaGeoreferenciacionContratista parameters);

        /// <summary>
        /// Consultar data para reporte de ordenes
        /// </summary>
        /// <param name="StoreProcedure"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<Op360Ordenes_ReporteDto> GetData_Ordenes(QueryActaOsPdf parameters);

        /// <summary>
        /// Cerrar Ordenes Manualmente
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto> CerrarOrdenesManualmente(QueryOp360CerrarOrdenes parameters);

        /// <summary>
        /// Descargar archivo de la tabla gnl plantillas
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<FileResponsePlantillasOracleBase64Dto>> DescargarArhivoGnlPlantillaToBase64(QueryOp360ObtenerGnlPlantilla parameters);

        /// <summary>
        /// Descargar archivo de la tabla gnl plantillas
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<FileResponseReportesJasper>> DescargarArhivoGnlPlantilla(QueryOp360ObtenerGnlPlantilla parameters);

        /// <summary>
        /// Descargar archivo de la tabla gnl plantillas
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<FileResponseReportesJasper>> DescargarArhivoGnlSoporte(QueryOp360ObtenerGnlPlantilla parameters);

        /// <summary>
        /// Procesa la información de las ordenes de trabajo cerradas y cargadas en la tabla temporal y las guarda en la tabla definitiva.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<FormdataResponseMasivo>> Procesar_Masivo_Cierre_Ordenes_SCR(QueryOp360CargueMasivoValidacion parameters);

        /// <summary>
        /// Procesa la información de las ordenes de trabajo reasignadas y cargadas en la tabla temporal y las guarda en la tabla definitiva.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<FormdataResponseMasivo>> Procesar_Masivo_Reasignar_Ordenes_SCR(QueryOp360CargueMasivoValidacion parameters);

        /// <summary>
        /// Procesa la información de las ordenes de trabajo reasignadas y cargadas en la tabla temporal y las guarda en la tabla definitiva.
        /// Este proceso omite el tipo de suspension
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<FormdataResponseMasivo>> Procesar_Masivo_Reasignar_Ordenes2_SCR(QueryOp360CargueMasivoValidacion parameters);

        /// <summary>
        /// Procesa la información de las ordenes de trabajo de asignacion de tecnico y cargadas en la tabla temporal y las guarda en la tabla definitiva.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<FormdataResponseMasivo>> Procesar_Masivo_Asignar_Tecnicos_SCR(QueryOp360CargueMasivoValidacionContratistas parameters);

        /// <summary>
        /// Procesa la información de las ordenes de trabajo de desasignacion de tecnico y cargadas en la tabla temporal y las guarda en la tabla definitiva.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto<FormdataResponseMasivo>> Procesar_Masivo_DesAsignar_Tecnicos_SCR(QueryOp360CargueMasivoValidacionContratistas parameters);

        /// <summary>
        /// Lista los filtros
        /// </summary>
        /// <returns></returns>
        Task<Op360_ListarFiltrosResponseDto> Listar_Filtros();

        /// <summary>
        /// Edita los filtros
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ResponseDto> Editar_Filtros(Op360_EditarFiltrosDto parameters);

        Task<ResponseDto> DescomprometerOrdenesManualmente(QueryOp360DescomprometerOrdenes parameters);

        /// <summary>
        /// Obtiene los registros de la tabla ordenes historial.
        /// </summary>
        /// <param name="parameters">Los parametros.</param>
        /// <returns>Listado de ordenes.</returns>
        Task<ResponseDto<ObtenerReporteTrazaDto>> GetReporteTrazaAreaCentralAsync(OrdenesRequestDto parameters, bool ExportExcel = false);
        Task<ResponseDto<ObtenerReporteTrazaDto>> GetReporteTrazaContratistaAsync(OrdenesContratistaRequestDto parameters, bool ExportExcel = false);

        Task<ResponseDto<ObtenerReporteLogLegalizacionDto>> ObtenerReporteLogLegalizacion(QueryOp360LogLegalizacion parameters, bool ExportExcel = false);
        
        Task<ResponseDto<ObtenerReporteLogLegalizacionDto>> ObtenerReporteLogLegalizacioContratista(QueryOp360LogLegalizacionContratistas parameters, bool ExportExcel = false);

        Task<ResponseDto> Procesar_Crear_Tabla_Tmp_SCR(QueryOp360CargueMasivoData parameters);
        #endregion
    }
}