namespace Core.Services
{
    using Core.Dtos.FiltrosDto;
    using Core.DTOs;
    using Core.DTOs.FilesDto;
    using Core.Entities;
    using Core.Interfaces;
    using Core.Options;
    using Core.QueryFilters;
    using Core.Tools;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Core.Enumerations;
    using Core.Entities.AreaCentralVersion2;
    using Core.QueryFilters.QueryFiltersSCRWeb;
    using Core.DTOs.FiltrosDto;
    using Core.DTOs.ObtenerReporteTraza;

    /*
    * fecha: 19/12/2023
    * clave: 5e6ewrt546weasdf _04
    * clave: 5e6ewrtLOGINEXTERNO6weasdf _04
    * carlos vargas
    */
    public class Op360Service : IOp360Service
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ParametrosOptions _parametrosOptions;

        public Op360Service(IUnitOfWork unitOfWork, IOptions<ParametrosOptions> parametrosOptions)
        {
            _unitOfWork = unitOfWork;
            _parametrosOptions = parametrosOptions.Value;
        }

        #region Op360Seguridad
        public async Task<Usuarios_Perfiles> ConsultarUsusariosxPerfiles(QueryOp360Seguridad parameters)
        {
            return await _unitOfWork.Op360Repository.ConsultarUsusariosxPerfiles(parameters);
        }

        public async Task<Usuarios_Perfiles> ValidarLoginExterno(string Id_Usuario, string Token_Apex)
        {
            return await _unitOfWork.Op360Repository.ValidarLoginExterno(Id_Usuario, Token_Apex);
        }
        #endregion        

        #region Ordenes    
        public async Task<ResponseDto<Op360ServerSideTestDto>> ServerSideTest(QueryOp360ServerSideTest parameters, bool ExportExcel = false)
        {
            parameters.ServerSideJson = parameters.ServerSide == null ? null : JsonConvert.DeserializeObject<QueryOp360ServerSide>(parameters.ServerSide);
            if (ExportExcel)
            {
                parameters.ServerSideJson.rows = 500000;
                parameters.ServerSideJson.first = 0;
            }
            var param = JsonConvert.SerializeObject(parameters);
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ServerSideTestDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_ServerSideTest.prc_prueba_serverside", param);
        }

        public Task<ResponseDto<Aire_Scr_OrdenAreaCentral_Response>> Consultar_Ordenes_Area_Central(QueryOp360Ordenes parameters, bool ExportExcel = false)
        {
            parameters.id_contratista = parameters.id_contratista ?? "-2";
            parameters.id_zona = parameters.id_zona ?? "-2";
            //parameters.codigo_estado = string.IsNullOrWhiteSpace(parameters.codigo_estado) ? "-2": parameters.codigo_estado;
            //parameters.suspension = string.IsNullOrWhiteSpace(parameters.suspension) ? "-2": parameters.suspension;
            parameters.id_orden = parameters.id_orden ?? -1;            
            parameters.ServerSideJson = parameters.ServerSide == null ? null : JsonConvert.DeserializeObject<QueryOp360ServerSide>(parameters.ServerSide);

            if (ExportExcel)
            {
                parameters.ServerSideJson.rows = 500000;
                parameters.ServerSideJson.first = 0;
            }

            var param = JsonConvert.SerializeObject(parameters);

            //return _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_OrdenAreaCentral_Response>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST13.prc_consultar_ordenes_area_central", param);
            //return _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_OrdenAreaCentral_Response>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST8.prc_consultar_ordenes_area_central", param);
            //return _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_OrdenAreaCentral_Response>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST 5.prc_consultar_ordenes_area_central", param);
            //return _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_OrdenAreaCentral_Response>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST 4.prc_consultar_ordenes_area_central", param);
            //return _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_OrdenAreaCentral_Response>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST 3.prc_consultar_ordenes_area_central", param);
            return _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_OrdenAreaCentral_Response>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_consultar_ordenes_area_central", param);
        }

        public async Task<ResponseDto<Aire_Scr_ReporteEjectuados_Response>> Consultar_Reporte_Ejecutados(QueryOp360ReporteEjecutados parameters, bool ExportExcel = false)
        {
            parameters.id_contratista = parameters.id_contratista ?? "-2";
            parameters.id_zona = parameters.id_zona ?? "-2";
            parameters.ServerSideJson = parameters.ServerSide == null ? null : JsonConvert.DeserializeObject<QueryOp360ServerSide>(parameters.ServerSide);
            parameters.fechaJson = parameters.fecha == null ? null : JsonConvert.DeserializeObject<DateTime[]>(parameters.fecha.Replace("null", ""));

            if (ExportExcel)
            {
                parameters.ServerSideJson.rows = 500000;
                parameters.ServerSideJson.first = 0;
            }

            var param = JsonConvert.SerializeObject(parameters);
            //var tmp = await _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_ReporteEjectuados_Response>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST13.prc_consultar_reportes_ejecutados", param);
            //var tmp = await _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_ReporteEjectuados_Response>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST9.prc_consultar_reportes_ejecutados", param);
            //var tmp = await _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_ReporteEjectuados_Response>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST8.prc_consultar_reportes_ejecutados", param);
            var tmp = await _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_ReporteEjectuados_Response>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_consultar_reportes_ejecutados", param);

            return tmp;
        }

        public async Task<ResponseDto<Aire_Scr_ReporteEjectuados_Response>> Consultar_Reporte_Ejecutados_Contratistas(QueryOp360ReporteEjecutadosContratistas parameters, bool ExportExcel = false)
        {
            parameters.id_contratista_persona = parameters.id_contratista_persona ?? "-2";
            parameters.id_zona = parameters.id_zona ?? "-2";
            parameters.ServerSideJson = parameters.ServerSide == null ? null : JsonConvert.DeserializeObject<QueryOp360ServerSide>(parameters.ServerSide);
            parameters.fechaJson = parameters.fecha == null ? null : JsonConvert.DeserializeObject<DateTime[]>(parameters.fecha.Replace("null", ""));

            if (ExportExcel)
            {
                parameters.ServerSideJson.rows = 500000;
                parameters.ServerSideJson.first = 0;
            }

            var param = JsonConvert.SerializeObject(parameters);
            //var tmp = await _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_ReporteEjectuados_Response>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST13.prc_consultar_reportes_ejecutados_contratista", param);
            //var tmp = await _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_ReporteEjectuados_Response>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST9.prc_consultar_reportes_ejecutados_contratista", param);
            var tmp = await _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_ReporteEjectuados_Response>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_consultar_reportes_ejecutados_contratista", param);

            return tmp;            
        }

        public Task<ResponseDto<Aire_Scr_OrdenAreaCentral_Response_Version2>> Consultar_Ordenes_Area_CentralVersion2(QueryOp360OrdenesV2 parameters, bool ExportExcel = false)
        {
            parameters.id_contratista = parameters.id_contratista ?? -2;
            parameters.id_zona = parameters.id_zona ?? -2;
            parameters.codigo_estado = string.IsNullOrWhiteSpace(parameters.codigo_estado) ? "-2" : parameters.codigo_estado;
            parameters.id_orden = parameters.id_orden ?? -1;
            parameters.ServerSideJson = parameters.ServerSide == null ? null : JsonConvert.DeserializeObject<QueryOp360ServerSideV2>(parameters.ServerSide);

            if (ExportExcel)
            {
                parameters.ServerSideJson.rows = 500000;
                parameters.ServerSideJson.first = 0;
            }
            var param = JsonConvert.SerializeObject(parameters);
            //return _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_OrdenAreaCentral_Response_Version2>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST 5.prc_consultar_ordenes_area_central", param);
            return _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_OrdenAreaCentral_Response_Version2>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_consultar_ordenes_area_central", param);
        }

        public Task<ResponseDto<Aire_Scr_Orden_DashBoard_AreaCentral_Response>> Consultar_Ordenes_DashBoard_Area_Central(QueryOp360OrdenesDashBoard parameters)
        {
            parameters.id_contratista = parameters.id_contratista ?? -2;
            var param = JsonConvert.SerializeObject(parameters);
            return _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_Orden_DashBoard_AreaCentral_Response>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_consultar_ordenes_dashboard_area_central", param);
        }

        public Task<ResponseDto<Aire_Scr_Orden_DashBoard_AreaCentral_Response>> Consultar_Ordenes_DashBoard_Contratistas(QueryOp360OrdenesDashBoardContratista parameters)
        {
            parameters.id_contratista_persona = parameters.id_contratista_persona ?? -2;
            var param = JsonConvert.SerializeObject(parameters);
            return _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_Orden_DashBoard_AreaCentral_Response>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_consultar_ordenes_dashboard_contratistas", param);
        }

        public Task<ResponseDto<Aire_Scr_OrdenById_Response>> Consultar_Orden_Por_Id(QueryOp360Orden parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            return _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_OrdenById_Response>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_consultar_orden_por_id", param);
        }

        public Task<ResponseDto<Archivos_Instancia_Response>> Consultar_Archivos_Instancia(QueryOp360ArchivosInstancia parameters, bool ExportExcel = false)
        {
            parameters.codigo = parameters.codigo ?? "";
            parameters.id_ruta_archivo_servidor = _parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? (int)Enum_RutasArchivos.ExcelScrMasivOrdBase64Local : (int)Enum_RutasArchivos.ExcelScrMasivOrdBase64;
            parameters.ServerSideJson = parameters.ServerSide == null ? null : JsonConvert.DeserializeObject<QueryOp360ServerSide>(parameters.ServerSide);

            if (ExportExcel)
            {
                parameters.ServerSideJson.rows = 500000;
                parameters.ServerSideJson.first = 0;
            }

            var param = JsonConvert.SerializeObject(parameters);
            //return _unitOfWork.StoreProcedure<ResponseDto<Archivos_Instancia_Response>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST 3.prc_consultar_archivos_instancia", param);
            return _unitOfWork.StoreProcedure<ResponseDto<Archivos_Instancia_Response>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_consultar_archivos_instancia", param);
        }

        public Task<ResponseDto<Archivos_Instancia_Detalle_Response>> Consultar_Archivos_Instancia_Detalle(QueryOp360ArchivosInstanciaDetalle parameters, bool ExportExcel = false)
        {
            //parameters.pageSize = parameters.pageSize ?? 1000000;
            //parameters.pageNumber = parameters.pageNumber ?? 0;
            //parameters.sortColumn = parameters.sortColumn ?? "id_orden";
            //parameters.sortDirection = parameters.sortDirection ?? "desc";

            parameters.ServerSideJson = parameters.ServerSide == null ? null : JsonConvert.DeserializeObject<QueryOp360ServerSide>(parameters.ServerSide);

            if (ExportExcel)
            {
                //parameters.pageSize = 500000;
                //parameters.pageNumber = 0;
                parameters.ServerSideJson.rows = 500000;
                parameters.ServerSideJson.first = 0;
            }

            var param = JsonConvert.SerializeObject(parameters);
            //return _unitOfWork.StoreProcedure<ResponseDto<Archivos_Instancia_Detalle_Response>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST 4.prc_consultar_archivos_instancia_detalle", param);
            return _unitOfWork.StoreProcedure<ResponseDto<Archivos_Instancia_Detalle_Response>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_consultar_archivos_instancia_detalle", param);
        }

        public Task<ResponseDto<Aire_Scr_Orden_Agrupada_Response>> Consultar_Ordenes_Agrupadas_Area_Central(QueryOp360OrdenesAgrupada parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            //return _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_Orden_Agrupada_Response>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST 5.prc_consulta_agrupada_ordenes_area_central", param);
            return _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_Orden_Agrupada_Response>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_consulta_agrupada_ordenes_area_central", param);
        }

        public Task<ResponseDto> Gestionar_Ordenes_Contratista_Asignar(QueryOp360GestionarContratistas parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            return _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_gestionar_orden_asignar_contratista", param);
        }

        public Task<ResponseDto> Gestionar_Ordenes_Contratista_DesAsignar(QueryOp360GestionarContratistas parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            return _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_gestionar_orden_des_asignar_contratista", param);
        }

        public Task<ResponseDto> Gestionar_Ordenes_Brigada_Asignar(QueryOp360GestionarBrigadas parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            //return _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_carlos_vargas_test7.prc_gestionar_orden_asignar_brigada", param);
            //return _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_carlos_vargas_test 8.prc_gestionar_orden_asignar_brigada", param);
            return _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_gestionar_orden_asignar_brigada", param);
        }

        public Task<ResponseDto> Gestionar_Ordenes_Brigada_DesAsignar(QueryOp360GestionarBrigadas parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            //var tmp = _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_carlos_vargas_test8.prc_gestionar_orden_des_asignar_brigada", param);
            var tmp = _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_gestionar_orden_des_asignar_brigada", param);
            return tmp;
        }

        public async Task<ResponseDto<Op360ParametrosInicialesAreaCentralDto>> GetAireScrOrdParametrosIniciales()
        {
            //--En ese caso si dejarlo apuntando a PKG_G_CARLOS_VARGAS_TEST 3 por jueves y viernes
            //var tmp = await _unitOfWork.StoreProcedure<ResponseDto<Op360ParametrosInicialesAreaCentralDto>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST12.prc_parametros_iniciales_areacentral");
            //var tmp = await _unitOfWork.StoreProcedure<ResponseDto<Op360ParametrosInicialesAreaCentralDto>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST 3.prc_parametros_iniciales_areacentral");
            var tmp = await _unitOfWork.StoreProcedure<ResponseDto<Op360ParametrosInicialesAreaCentralDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_parametros_iniciales_areacentral");
            Enum_RutasArchivos msvoCargueOrdenes = _parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? Enum_RutasArchivos.TmplteScrOrdenesMasivoLocal : Enum_RutasArchivos.TmplteScrOrdenesMasivo;
            Enum_RutasArchivos msvoReasignacion = _parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? Enum_RutasArchivos.TmplteScrReasignacionMasivoLocal : Enum_RutasArchivos.TmplteScrReasignacionMasivo;
            Enum_RutasArchivos msvoLegalizacion = _parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? Enum_RutasArchivos.TmplteScrLegalizacionMasivoLocal : Enum_RutasArchivos.TmplteScrLegalizacionMasivo;
            Enum_RutasArchivos msvoAsignacion = _parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? Enum_RutasArchivos.TmplteScrAsignacionMasivoLocal : Enum_RutasArchivos.TmplteScrAsignacionMasivo;
            Enum_RutasArchivos msvoDesasignacion = _parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? Enum_RutasArchivos.TmplteScrDesAsignacionMasivoLocal : Enum_RutasArchivos.TmplteScrDesAsignacionMasivo;
            Enum_RutasArchivos msvoReasignacion2 = _parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? Enum_RutasArchivos.TmplteScrReasignacionMasivo2Local : Enum_RutasArchivos.TmplteScrReasignacionMasivo2;
            
            

            gnl_rutas_archivo_servidor rootFileServer = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)msvoCargueOrdenes);
            gnl_rutas_archivo_servidor rootFileServer2 = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)msvoReasignacion);
            gnl_rutas_archivo_servidor rootFileServer3 = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)msvoLegalizacion);
            gnl_rutas_archivo_servidor rootFileServer4 = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)msvoAsignacion);
            gnl_rutas_archivo_servidor rootFileServer5 = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)msvoDesasignacion);
            gnl_rutas_archivo_servidor rootFileServer6 = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)msvoReasignacion2);
            
            
            tmp.Datos.Url_plantilla_Generacion_Ordenes = new Uri(rootFileServer.RUTA_WEB);
            tmp.Datos.Url_plantilla_Reasignacion_Contratista = new Uri(rootFileServer2.RUTA_WEB);
            tmp.Datos.Url_plantilla_Legalizacion_Orden = new Uri(rootFileServer3.RUTA_WEB);
            tmp.Datos.Url_plantilla_Asignacion_Tecnico = new Uri(rootFileServer4.RUTA_WEB);
            tmp.Datos.Url_plantilla_DesAsignacion_Tecnico = new Uri(rootFileServer5.RUTA_WEB);
            tmp.Datos.Url_plantilla_Reasignacion_Contratista2 = new Uri(rootFileServer6.RUTA_WEB);
            return tmp;
        }

        public async Task<ResponseDto<Op360ParametrosInicialesContratistaDto>> GetParametrosInicialesContratistas(QueryOp360ObtenerBrigadas parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            //--En ese caso si dejarlo apuntando a PKG_G_CARLOS_VARGAS_TEST 3 por jueves y viernes
            //var tmp = await _unitOfWork.StoreProcedure<ResponseDto<Op360ParametrosInicialesContratistaDto>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST 6_Casa.prc_parametros_iniciales_contratistas", param);
            var tmp = await _unitOfWork.StoreProcedure<ResponseDto<Op360ParametrosInicialesContratistaDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_parametros_iniciales_contratistas", param);

            Enum_RutasArchivos msvoAsignacion = _parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? Enum_RutasArchivos.TmplteScrAsignacionMasivoLocal : Enum_RutasArchivos.TmplteScrAsignacionMasivo;
            Enum_RutasArchivos msvoDesasignacion = _parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? Enum_RutasArchivos.TmplteScrDesAsignacionMasivoLocal : Enum_RutasArchivos.TmplteScrDesAsignacionMasivo;

            gnl_rutas_archivo_servidor rootFileServer4 = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)msvoAsignacion);
            gnl_rutas_archivo_servidor rootFileServer5 = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById((int)msvoDesasignacion);


            tmp.Datos.Url_plantilla_Asignacion_Tecnico = new Uri(rootFileServer4.RUTA_WEB);
            tmp.Datos.Url_plantilla_DesAsignacion_Tecnico = new Uri(rootFileServer5.RUTA_WEB);

            return tmp;
            //return _unitOfWork.StoreProcedure<ResponseDto<Op360ParametrosInicialesContratistaDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_parametros_iniciales_contratistas", param);
        }

        public Task<ResponseDto<Aire_ScrOrdConsultaClienteNicNis>> ConsultarClienteByNicNis(QueryOp360NicNis parameters)
        {
            //---Eliminar null
            parameters.nic = string.IsNullOrWhiteSpace(parameters.nic) || parameters.nic == "null" ? "" : parameters.nic;
            parameters.nis = string.IsNullOrWhiteSpace(parameters.nis) || parameters.nis == "null" ? "" : parameters.nis;
            var param = JsonConvert.SerializeObject(parameters);
            return _unitOfWork.StoreProcedure<ResponseDto<Aire_ScrOrdConsultaClienteNicNis>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_consultar_cliente_por_nic_nis", param);
        }

        public Task<ResponseDto<FormOrdenRequest>> Guardar_Orden_Formdata(FormOrdenRequest parameters)
        {
            parameters.contratista = parameters.contratista ?? 0;            
            parameters.contratista = parameters.contratista == null ? 0 : parameters.contratista;
            parameters.contratista = parameters.contratista < 0 ? 0 : parameters.contratista;
            var param = JsonConvert.SerializeObject(parameters);

            //--Este paquete corresponde a oracle casa local y sobreescribe a PKG_G_CARLOS_VARGAS_TEST 5
            //return _unitOfWork.StoreProcedure<ResponseDto<FormOrdenRequest>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST 6_Casa.prc_registrar_orden2", param);
            //--Este paquete quedo actualizado en produccion            
            //return _unitOfWork.StoreProcedure<ResponseDto<FormOrdenRequest>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST 5.prc_registrar_orden2", param);
            return _unitOfWork.StoreProcedure<ResponseDto<FormOrdenRequest>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_registrar_orden2", param);
        }

        public Task<ResponseDto<FormOrdenRegistro>> Guardar_Orden_Excel(FormOrdenRegistro parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            return _unitOfWork.StoreProcedure<ResponseDto<FormOrdenRegistro>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_registro_ordenes_masivo_temporal", param);
        }

        public Task<ResponseDto<FormOrdenRegistro>> Guardar_Orden_ExcelBorreme(FormOrdenRegistro parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            return _unitOfWork.StoreProcedure<ResponseDto<FormOrdenRegistro>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST.prc_registro_ordenes_masivo_temporal_Borreme", param);
        }

        public Task<ResponseDto<FormdataResponseMasivo>> Procesar_Masivo_Creacion_Ordenes_SCR(QueryOp360CargueMasivoValidacion parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            //return _unitOfWork.StoreProcedure<ResponseDto<FormdataResponseMasivo>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST 6_Casa.prc_registro_ordenes_masivo_final_V2", param);
            return _unitOfWork.StoreProcedure<ResponseDto<FormdataResponseMasivo>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_registro_ordenes_masivo_final_V2", param);
            //return _unitOfWork.StoreProcedure<ResponseDto<FormdataResponseMasivo>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_registro_ordenes_masivo_final", param);
        }

        public Task<ResponseDto<FormdataResponseMasivo>> Guardar_Orden_Masivo_Final_Gos(QueryOp360CargueMasivoValidacion parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            return _unitOfWork.StoreProcedure<ResponseDto<FormdataResponseMasivo>>().ExecuteStoreProcedureAsync("aire.pkg_g_gos.prc_registro_ordenes_masivo", param);
        }

        public Task<ResponseDto<Aire_Scr_Orden_Response>> Consultar_Ordenes_Contratistas(QueryOp360OrdenesContratistas parameters, bool ExportExcel = false)
        {
            parameters.id_contratista_persona = parameters.id_contratista_persona ?? "-2";
            parameters.id_zona = parameters.id_zona ?? "-2";
            parameters.id_estado_orden = parameters.id_estado_orden ?? -2;
            parameters.codigo_estado = parameters.codigo_estado ?? "-2";
            parameters.codigo_suspencion = parameters.codigo_suspencion ?? "-2";
            parameters.id_orden = parameters.id_orden ?? -1;

            parameters.ServerSideJson = parameters.ServerSide == null ? null : JsonConvert.DeserializeObject<QueryOp360ServerSide>(parameters.ServerSide);

            if (ExportExcel)
            {
                parameters.ServerSideJson.rows = 500000;
                parameters.ServerSideJson.first = 0;
            }

            var param = JsonConvert.SerializeObject(parameters);
            //return _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_Orden_Response>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST13.prc_consultar_ordenes_contratistas", param);          //return _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_Orden_Response>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST8.prc_consultar_ordenes_contratistas", param);
            //return _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_Orden_Response>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST 4.prc_consultar_ordenes_contratistas", param);
            return _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_Orden_Response>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_consultar_ordenes_contratistas", param);
        }

        public Task<ResponseDto<Op360Contratistas>> ConsultarContratistaPorIdentificacion(QueryOp360Contratistas parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            return _unitOfWork.StoreProcedure<ResponseDto<Op360Contratistas>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_consultar_contratista_por_identificacion", param);
        }

        public Task<ResponseDto<Op360ParametrosInicialesFiltrosDto>> GetParametrosInicialesFiltros()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360ParametrosInicialesFiltrosDto>>().ExecuteStoreProcedureAsync("AIRE.pkg_g_ordenes.PRC_FILTROS_PARAMETROS_INICIALES");
        }

        public Task<ResponseDto<Op360ResumenGlobalOrdenesDto>> GetResumen_Global_Ordenes()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360ResumenGlobalOrdenesDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_resumen_global_ordenes");
        }

        public async Task<ResponseDto<IList<GetFiltroResponseDto>>> ConsultarFiltros()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<IList<GetFiltroResponseDto>>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_consultar_filtros");
        }

        public async Task<ResponseDto<CreateFiltroResponseDto>> CrearFiltro(CreateFiltroRequestDto createFiltroRequestDto)
        {
            var param = JsonConvert.SerializeObject(createFiltroRequestDto);
            return await _unitOfWork.StoreProcedure<ResponseDto<CreateFiltroResponseDto>>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_ordenes.prc_registrar_filtro", param);
        }

        public async Task<ResponseDto> ActualizarFiltro(UpdateFiltroRequestDto updateFiltroRequestDto)
        {
            var param = JsonConvert.SerializeObject(updateFiltroRequestDto);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_ordenes.prc_actualizar_filtro", param);
        }

        public async Task<ResponseDto> EliminarFiltro(DeleteFiltroRequestDto deleteFiltroRequestDto)
        {
            var param = JsonConvert.SerializeObject(deleteFiltroRequestDto);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_ordenes.prc_eliminar_filtro", param);
        }

        public async Task<ResponseDto<IList<Op360Ordenes_TrazabilidadDto>>> GetConsultar_Trazabilidad_Ordenes(QueryOp360GetOrdenes createFiltroRequestDto)
        {
            var param = JsonConvert.SerializeObject(createFiltroRequestDto);
            return await _unitOfWork.StoreProcedure<ResponseDto<IList<Op360Ordenes_TrazabilidadDto>>>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_ordenes.prc_consultar_trazabilidad_ordenes", param);
        }

        public async Task<ResponseDto<IList<Op360GeoreferenciacionFechaDto>>> GetConsultar_Georreferencia_AreaCentral(QueryOp360GetFechaGeoreferenciacion parameters)
        {
            parameters.id_contratista_persona = parameters.id_contratista_persona ?? -2;
            var param = JsonConvert.SerializeObject(parameters);
            return await _unitOfWork.StoreProcedure<ResponseDto<IList<Op360GeoreferenciacionFechaDto>>>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_ordenes.prc_consultar_georreferencia_areacentral", param);
        }

        public async Task<ResponseDto<IList<Op360GeoreferenciacionFechaDto>>> GetConsultar_Georreferencia_ContratistaPersona(QueryOp360GetFechaGeoreferenciacionContratista parameters)
        {
            parameters.id_contratista_persona = parameters.id_contratista_persona ?? -2;
            var param = JsonConvert.SerializeObject(parameters);
            return await _unitOfWork.StoreProcedure<ResponseDto<IList<Op360GeoreferenciacionFechaDto>>>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_ordenes.prc_consultar_georreferencia_contratista_persona", param);
        }

        public async Task<Op360Ordenes_ReporteDto> GetData_Ordenes(QueryActaOsPdf parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            //return await _unitOfWork.StoreProcedure<Op360Ordenes_ReporteDto>().ExecuteStoreProcedureNonQueryAsync("aire.PKG_G_CARLOS_VARGAS_TEST2.prc_data_reporte_ordenes", param);
            return await _unitOfWork.StoreProcedure<Op360Ordenes_ReporteDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_ordenes.prc_data_reporte_ordenes", param);
        }

        public async Task<ResponseDto> CerrarOrdenesManualmente(QueryOp360CerrarOrdenes parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_ordenes.prc_cerrar_ordenes_manual", param);
        }
        
        public async Task<ResponseDto<FileResponsePlantillasOracleBase64Dto>> DescargarArhivoGnlPlantillaToBase64(QueryOp360ObtenerGnlPlantilla parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            return await _unitOfWork.StoreProcedure<ResponseDto<FileResponsePlantillasOracleBase64Dto>>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_ordenes.prc_descargar_archivo_gnl_plantilla", param);
        }

        public async Task<ResponseDto<FileResponseReportesJasper>> DescargarArhivoGnlPlantilla(QueryOp360ObtenerGnlPlantilla parameters)
        {
            try
            {
                var param = JsonConvert.SerializeObject(parameters);
                var password = Funciones.Base64DecodeWithKey(parameters.contrasena, _parametrosOptions.key16128);
                var user = Funciones.Base64DecodeWithKey(parameters.usuario, _parametrosOptions.key16128);
                
                if (user == _parametrosOptions.UsuarioArchivos && password == _parametrosOptions.ContrasenaArchivos)
                {
                    ResponseDto<FileResponseReportesJasper> tmpresponse = await _unitOfWork.StoreProcedure<ResponseDto<FileResponseReportesJasper>>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_ordenes.prc_descargar_archivo_gnl_plantilla", param);
                    tmpresponse.Datos.archivobyte = Convert.FromBase64String(tmpresponse.Datos.archivobase64);
                    return tmpresponse;
                }
                else
                {
                    ResponseDto<FileResponseReportesJasper> tmpresponse = new ResponseDto<FileResponseReportesJasper>
                    {
                        Codigo = 401,
                        Mensaje = "Usuario o contraseña incorrectos",
                        Datos = new FileResponseReportesJasper()
                        {
                            nombre_archivo = "claveincorrecta.png",
                            typemime = "image/png",
                            archivobase64 = "iVBORw0KGgoAAAANSUhEUgAAAIAAAACACAMAAAD04JH5AAABrVBMVEVHcEwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADOAP7yAAAAjnRSTlMAmb8B3yDvT5+vNIZvz0IG8veCW4/2Nf4CHdXMyM4UgEy7OqHK+O1NEyzUFfocU/DuLeR7qtMEo0YNEjZaX+WDtqbegfsXH8P5iQNxC64O12k9rSH9BUi0Uti5ms2zUZRW3Ho8CEu3oqfqy6xK9HMR2ngbwIdJELWW2Zs5pQqk0bySvuwx6X7njWXJuDexYUnKywAAAt5JREFUeNrt22dbGkEQAOA5AYEjJ4KIIohERVFsaJoxUVM01Zjee++9N9N77jeHnYVrwMOX9TYxM5+GuXluX7nj3IU7AAoKCgqKdL9fWvSnAZqadYnR3AQdutRYC3LH19tkA/S/BOCX8fHzE4AAFYCN0fWbSxvjiqLyzNPSs+2pO4DwYDGL4jY1ouuROKZdxWLnOlcAG1g2itsUliqY9rG06f8AWA5BnB0C1e1DYD0JVUWJmyfhPboOEGDlA2hGRADpgJCMdVmIrgMEIAABCEAAAhCAAAQgAAFWBmBy0i1AeyzAImQpDXf1ZnV96HQ0Yeu8jY2BgmBAhq8lTYDnbmt5gRl54LFIX2Ct741gwGpsHBo2CjHrGvec2XjFKRUDGGjDxtdGYcS+ym6pLRUDeMnHUY03eqsdMH3ELn0r+CRM8+O9wygc5ePemprawrPjpQ2nHFJBgAm+2xHHGXDjMkB4GtPnvJ5zSsUAwuPY5t1tVK5i4RlLz2B6ktcvOqViAJv4bveYlSQWrrG0B9POGlIxgF7+U/c+s3IfKwGWBjC9UEMqBNDNdxuzlLxOgLeGVAhgDTYdPlQXUEUqApDYjk0H4Xsji/e1AYb0I2scmxME2MX/rgOwyhy1KsCQlr7+9IkB7B3DniTUBRhSsYD9xtW+DsCUCgV4TmDLeLguwJQKBczznTVqmvYVs1HtG5TufbEAFgF+8s5BTVviWUcyLwDgq/yGuwEghcl11sD/G/0AaKjs9C0bYAGTR+YE4LfLgC+YZB8CzN0pz4BcBRR45p2ZKc1Mfi0bIBc0YhGbPwcniuUP9pFSbNJgNB7jxXfBnNh1Af8YBjEvtFrH3/nJ3in0QlQdAGezlvHPg4uAhdIr9WZ5fO0VVAXMiwYk8MemAeMa2R2aTaVmH1/yVHQ+wbsF22l1TAACEIAABCAAAQhAgH8HQDezyXzIRfpjPtIfdIJ8Rt6jXpk8UFBQUFD8AcYFAV/dK38aAAAAAElFTkSuQmCC"
                        }
                    };
                    tmpresponse.Datos.archivobyte = Convert.FromBase64String(tmpresponse.Datos.archivobase64);
                    return tmpresponse;
                }
            }
            catch (Exception e)
            {
                ResponseDto<FileResponseReportesJasper> tmpresponse = new ResponseDto<FileResponseReportesJasper>
                {
                    Codigo = 400,
                    Mensaje = e.Message,
                    Datos = new FileResponseReportesJasper()
                    {
                        nombre_archivo = "error.png",
                        typemime = "image/png",
                        archivobase64 = "iVBORw0KGgoAAAANSUhEUgAAAIAAAACACAMAAAD04JH5AAAA0lBMVEVHcEwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA+Wzu+AAAARXRSTlMAG5ayTQHbT6/5tYYuyNf6RfQVtFsU7VIZQvUcXwIWoc/UEOgkZwVIuTiAZgSpCUxRf2XrCLdVNp0mrI0RtoJjaEmMoIGSnxwWAAAB0ElEQVR42u3a15KCQBAFUHaBZUEFxZzWsDnnnEP//y+tjKCEeaWH0nufmimr+pSOjRajaQiCIAiCrH2auk3KYutNTSel0TVbLcDWSHEKAihZClKKASwV3z8LgKICfKsclWXLD6uDbveMCVCtEb3Oyx+iWlVUb2Oi8ZQH4AbX4j0oB5UrFj+C8o8H4C2uxQs8sTgKyhEPwJld1naCamf2YZAjFhvBixo8gI7judVwO7ie0xGVedJqXZuYAwCsMqCyrSCVIv0iAkA5QPkmxBwAAAAAAAAAAAAAAAAAAABYQcDwcNBPLd21G1M2wPCeqHeZWNqYEO3ucQG+xRMxI95/K1h64gJ8Ukow70/vXIB+LykI+9M+2yY07Lgg6r9p8n0N4wJp/9znwFIg75//IIoEN/L+DJPQSDwYT/fnGMVxQaY/y71gKcj257kZnYb966aau2G0/5NTmQ+w6C8V5A+I9ZcJcgdE/eu2XJA3YDn/DLkgZ0B8/soF+QKS818qyBVwMUnOn0jwyAV4SM+/UHDOBRhk5u9cUOcCNG8z818Irtg24fPv0Vdq/h+/OG38NQMAAAAAKCxA+aHWtT7Cofxot/LD7b7a4/2+hiAIgiDI2ucfAPcie+7ygJ4AAAAASUVORK5CYII="
                    }
                };
                tmpresponse.Datos.archivobyte = Convert.FromBase64String(tmpresponse.Datos.archivobase64);
                return tmpresponse;
            }
        }

        public async Task<ResponseDto<FileResponseReportesJasper>> DescargarArhivoGnlSoporte(QueryOp360ObtenerGnlPlantilla parameters)
        {
            var password = Funciones.Base64DecodeWithKey(parameters.contrasena, _parametrosOptions.key16128);
            var user = Funciones.Base64DecodeWithKey(parameters.usuario, _parametrosOptions.key16128);

            if (user == _parametrosOptions.UsuarioArchivos && password == _parametrosOptions.ContrasenaArchivos)
            {
                gnl_soportes rootFileSoportes = await _unitOfWork.Gnl_SoportesOracleRepository.GetById((int)parameters.id_soporte);
                int IdRutaArchivo = _parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? (int)Enum_RutasArchivos.ScrOrdenesMovileLocal : (int)Enum_RutasArchivos.ScrOrdenesMovile;
                gnl_rutas_archivo_servidor rootFileServidor = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById(IdRutaArchivo);
                
                var response = new ResponseDto<FileResponseReportesJasper>()
                {
                    Codigo = 0,
                    Mensaje = "OK",
                    Datos = new FileResponseReportesJasper()
                    {
                        nombre_archivo = rootFileSoportes.NOMBRE,
                        typemime = rootFileSoportes.FORMATO,
                        archivobase64 = "",
                        id_plantilla = rootFileSoportes.Id,
                        url_interna = System.IO.Path.Combine(rootFileServidor.RUTA_RED, rootFileSoportes.NOMBRE)
                    }
                };
                //-- Si no existe se devuelve un archivo por defecto
                if (!File.Exists(response.Datos.url_interna))
                {
                    response = new ResponseDto<FileResponseReportesJasper>
                    {
                        Codigo = 400,
                        Mensaje = "El archivo no existe.",
                        Datos = new FileResponseReportesJasper()
                        {
                            nombre_archivo = "error.png",
                            typemime = "image/png",
                            archivobase64 = "iVBORw0KGgoAAAANSUhEUgAAAIAAAACACAMAAAD04JH5AAAA0lBMVEVHcEwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA+Wzu+AAAARXRSTlMAG5ayTQHbT6/5tYYuyNf6RfQVtFsU7VIZQvUcXwIWoc/UEOgkZwVIuTiAZgSpCUxRf2XrCLdVNp0mrI0RtoJjaEmMoIGSnxwWAAAB0ElEQVR42u3a15KCQBAFUHaBZUEFxZzWsDnnnEP//y+tjKCEeaWH0nufmimr+pSOjRajaQiCIAiCrH2auk3KYutNTSel0TVbLcDWSHEKAihZClKKASwV3z8LgKICfKsclWXLD6uDbveMCVCtEb3Oyx+iWlVUb2Oi8ZQH4AbX4j0oB5UrFj+C8o8H4C2uxQs8sTgKyhEPwJld1naCamf2YZAjFhvBixo8gI7judVwO7ie0xGVedJqXZuYAwCsMqCyrSCVIv0iAkA5QPkmxBwAAAAAAAAAAAAAAAAAAABYQcDwcNBPLd21G1M2wPCeqHeZWNqYEO3ucQG+xRMxI95/K1h64gJ8Ukow70/vXIB+LykI+9M+2yY07Lgg6r9p8n0N4wJp/9znwFIg75//IIoEN/L+DJPQSDwYT/fnGMVxQaY/y71gKcj257kZnYb966aau2G0/5NTmQ+w6C8V5A+I9ZcJcgdE/eu2XJA3YDn/DLkgZ0B8/soF+QKS818qyBVwMUnOn0jwyAV4SM+/UHDOBRhk5u9cUOcCNG8z818Irtg24fPv0Vdq/h+/OG38NQMAAAAAKCxA+aHWtT7Cofxot/LD7b7a4/2+hiAIgiDI2ucfAPcie+7ygJ4AAAAASUVORK5CYII="
                        }
                    };
                    response.Datos.archivobyte = Convert.FromBase64String(response.Datos.archivobase64);
                }
                return response;
            }
            else
            {
                //--En caso de error se retorna un archivo por defecto
                ResponseDto<FileResponseReportesJasper> response = new ResponseDto<FileResponseReportesJasper>
                {
                    Codigo = 401,
                    Mensaje = "Usuario o contraseña incorrectos",
                    Datos = new FileResponseReportesJasper()
                    {
                        nombre_archivo = "claveincorrecta.png",
                        typemime = "image/png",
                        archivobase64 = "iVBORw0KGgoAAAANSUhEUgAAAIAAAACACAMAAAD04JH5AAABrVBMVEVHcEwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADOAP7yAAAAjnRSTlMAmb8B3yDvT5+vNIZvz0IG8veCW4/2Nf4CHdXMyM4UgEy7OqHK+O1NEyzUFfocU/DuLeR7qtMEo0YNEjZaX+WDtqbegfsXH8P5iQNxC64O12k9rSH9BUi0Uti5ms2zUZRW3Ho8CEu3oqfqy6xK9HMR2ngbwIdJELWW2Zs5pQqk0bySvuwx6X7njWXJuDexYUnKywAAAt5JREFUeNrt22dbGkEQAOA5AYEjJ4KIIohERVFsaJoxUVM01Zjee++9N9N77jeHnYVrwMOX9TYxM5+GuXluX7nj3IU7AAoKCgqKdL9fWvSnAZqadYnR3AQdutRYC3LH19tkA/S/BOCX8fHzE4AAFYCN0fWbSxvjiqLyzNPSs+2pO4DwYDGL4jY1ouuROKZdxWLnOlcAG1g2itsUliqY9rG06f8AWA5BnB0C1e1DYD0JVUWJmyfhPboOEGDlA2hGRADpgJCMdVmIrgMEIAABCEAAAhCAAAQgAAFWBmBy0i1AeyzAImQpDXf1ZnV96HQ0Yeu8jY2BgmBAhq8lTYDnbmt5gRl54LFIX2Ct741gwGpsHBo2CjHrGvec2XjFKRUDGGjDxtdGYcS+ym6pLRUDeMnHUY03eqsdMH3ELn0r+CRM8+O9wygc5ePemprawrPjpQ2nHFJBgAm+2xHHGXDjMkB4GtPnvJ5zSsUAwuPY5t1tVK5i4RlLz2B6ktcvOqViAJv4bveYlSQWrrG0B9POGlIxgF7+U/c+s3IfKwGWBjC9UEMqBNDNdxuzlLxOgLeGVAhgDTYdPlQXUEUqApDYjk0H4Xsji/e1AYb0I2scmxME2MX/rgOwyhy1KsCQlr7+9IkB7B3DniTUBRhSsYD9xtW+DsCUCgV4TmDLeLguwJQKBczznTVqmvYVs1HtG5TufbEAFgF+8s5BTVviWUcyLwDgq/yGuwEghcl11sD/G/0AaKjs9C0bYAGTR+YE4LfLgC+YZB8CzN0pz4BcBRR45p2ZKc1Mfi0bIBc0YhGbPwcniuUP9pFSbNJgNB7jxXfBnNh1Af8YBjEvtFrH3/nJ3in0QlQdAGezlvHPg4uAhdIr9WZ5fO0VVAXMiwYk8MemAeMa2R2aTaVmH1/yVHQ+wbsF22l1TAACEIAABCAAAQhAgH8HQDezyXzIRfpjPtIfdIJ8Rt6jXpk8UFBQUFD8AcYFAV/dK38aAAAAAElFTkSuQmCC"
                    }
                };
                response.Datos.archivobyte = Convert.FromBase64String(response.Datos.archivobase64);
                return response;
            }
        }

        public async Task<ResponseDto<FormdataResponseMasivo>> Procesar_Masivo_Cierre_Ordenes_SCR(QueryOp360CargueMasivoValidacion parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            //return await _unitOfWork.StoreProcedure<ResponseDto<FormdataResponseMasivo>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST12.prc_cierre_ordenes_masivo", param);
            return await _unitOfWork.StoreProcedure<ResponseDto<FormdataResponseMasivo>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_cierre_ordenes_masivo", param);
        }

        public async Task<ResponseDto<FormdataResponseMasivo>> Procesar_Masivo_Reasignar_Ordenes_SCR(QueryOp360CargueMasivoValidacion parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            return await _unitOfWork.StoreProcedure<ResponseDto<FormdataResponseMasivo>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_reasignacion_ordenes_masivo", param);
        }

        public async Task<ResponseDto<FormdataResponseMasivo>> Procesar_Masivo_Reasignar_Ordenes2_SCR(QueryOp360CargueMasivoValidacion parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            return await _unitOfWork.StoreProcedure<ResponseDto<FormdataResponseMasivo>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_reasignacion_ordenes_masivo2", param);
        }

        public async Task<ResponseDto<FormdataResponseMasivo>> Procesar_Masivo_Asignar_Tecnicos_SCR(QueryOp360CargueMasivoValidacionContratistas parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            //return await _unitOfWork.StoreProcedure<ResponseDto<FormdataResponseMasivo>>().ExecuteStoreProcedureAsync("aire.pkg_g_carlos_vargas_test7.prc_asignar_ordenes_masivo_tecnico", param);
            //return await _unitOfWork.StoreProcedure<ResponseDto<FormdataResponseMasivo>>().ExecuteStoreProcedureAsync("aire.pkg_g_carlos_vargas_test 8.prc_asignar_ordenes_masivo_tecnico", param);
            return await _unitOfWork.StoreProcedure<ResponseDto<FormdataResponseMasivo>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_asignar_ordenes_masivo_tecnico", param);
        }

        public async Task<ResponseDto<FormdataResponseMasivo>> Procesar_Masivo_DesAsignar_Tecnicos_SCR(QueryOp360CargueMasivoValidacionContratistas parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            //return await _unitOfWork.StoreProcedure<ResponseDto<FormdataResponseMasivo>>().ExecuteStoreProcedureAsync("aire.pkg_g_carlos_vargas_test 7.prc_desasignar_ordenes_masivo_tecnico", param);
            return await _unitOfWork.StoreProcedure<ResponseDto<FormdataResponseMasivo>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_desasignar_ordenes_masivo_tecnico", param);
        }

        public async Task<Op360_ListarFiltrosResponseDto> Listar_Filtros()
        {
            return await _unitOfWork.StoreProcedure<Op360_ListarFiltrosResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_jesus_gamez.prc_carga_inicial_de_los_filtros");
        }

        public async Task<ResponseDto> Editar_Filtros(Op360_EditarFiltrosDto parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);
        }

        public async Task<ResponseDto> DescomprometerOrdenesManualmente(QueryOp360DescomprometerOrdenes parameters)
        {
            var param = JsonConvert.SerializeObject(parameters);
            //return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_carlos_vargas_test8.prc_descomprometer_ordenes", param);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_ordenes.prc_descomprometer_ordenes", param);
        }

        //cascarones
        public Task<ResponseDto<Aire_Scr_reporte_ejecutados_Response>> Consultar_Reporte_Ejecutados_Scr(QueryOp360OreportesScr parameters, bool ExportExcel = false)
        {
            parameters.ServerSideJson = parameters.ServerSide == null ? null : JsonConvert.DeserializeObject<QueryOp360ServerSide>(parameters.ServerSide);

            if (ExportExcel)
            {
                parameters.ServerSideJson.rows = 500000;
                parameters.ServerSideJson.first = 0;
            }

            var param = JsonConvert.SerializeObject(parameters);

            return _unitOfWork.StoreProcedure<ResponseDto<Aire_Scr_reporte_ejecutados_Response>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp dummi por el momento
        }

        public Task<ResponseDto<ObtenerReporteLogLegalizacionDto>> ObtenerReporteLogLegalizacion(QueryOp360LogLegalizacion parameters, bool ExportExcel = false)
        {            
            //parameters.id_ruta_archivo_servidor = _parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? (int)Enum_RutasArchivos.ExcelScrMasivOrdBase64Local : (int)Enum_RutasArchivos.ExcelScrMasivOrdBase64;
            parameters.ServerSideJson = parameters.ServerSide == null ? null : JsonConvert.DeserializeObject<QueryOp360ServerSide>(parameters.ServerSide);
            parameters.fechaJson = parameters.fecha == null ? null : JsonConvert.DeserializeObject<DateTime[]>(parameters.fecha.Replace("null", ""));

            if (ExportExcel)
            {
                parameters.ServerSideJson.rows = 500000;
                parameters.ServerSideJson.first = 0;
            }

            var param = JsonConvert.SerializeObject(parameters);
            //return _unitOfWork.StoreProcedure<ResponseDto<ObtenerReporteLogLegalizacionDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_carlos_vargas_test10.prc_consultar_reportes_log_legalizacion", param);
            return _unitOfWork.StoreProcedure<ResponseDto<ObtenerReporteLogLegalizacionDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_consultar_reportes_log_legalizacion", param);
        }

        public Task<ResponseDto<ObtenerReporteLogLegalizacionDto>> ObtenerReporteLogLegalizacioContratista(QueryOp360LogLegalizacionContratistas parameters, bool ExportExcel = false)
        {
            parameters.id_contratista_persona = parameters.id_contratista_persona ?? -2;
            parameters.id_zona = parameters.id_zona ?? -2;
            parameters.id_ruta_archivo_servidor = _parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? (int)Enum_RutasArchivos.ExcelScrMasivOrdBase64Local : (int)Enum_RutasArchivos.ExcelScrMasivOrdBase64;
            parameters.ServerSideJson = parameters.ServerSide == null ? null : JsonConvert.DeserializeObject<QueryOp360ServerSide>(parameters.ServerSide);
            parameters.fechaJson = parameters.fecha == null ? null : JsonConvert.DeserializeObject<DateTime[]>(parameters.fecha.Replace("null", ""));

            if (ExportExcel)
            {
                parameters.ServerSideJson.rows = 500000;
                parameters.ServerSideJson.first = 0;
            }

            var param = JsonConvert.SerializeObject(parameters);
            //return _unitOfWork.StoreProcedure<ResponseDto<ObtenerReporteLogLegalizacionDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_carlos_vargas_test10.prc_consultar_reportes_log_legalizacion_contratista", param);
            return _unitOfWork.StoreProcedure<ResponseDto<ObtenerReporteLogLegalizacionDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_consultar_reportes_log_legalizacion_contratista", param);
        }

        public async Task<ResponseDto<ObtenerReporteTrazaDto>> GetReporteTrazaAreaCentralAsync(OrdenesRequestDto parameters, bool ExportExcel = false)
        {
            parameters.id_contratista = parameters.id_contratista ?? "-2";
            parameters.id_zona = parameters.id_zona ?? "-2";
            //parameters.id_ruta_archivo_servidor = _parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? (int)Enum_RutasArchivos.ExcelScrMasivOrdBase64Local : (int)Enum_RutasArchivos.ExcelScrMasivOrdBase64;
            parameters.ServerSideJson = parameters.ServerSide == null ? null : JsonConvert.DeserializeObject<QueryOp360ServerSide>(parameters.ServerSide);
            parameters.fechaJson = parameters.fecha == null ? null : JsonConvert.DeserializeObject<DateTime[]>(parameters.fecha.Replace("null", ""));

            if (ExportExcel)
            {
                parameters.ServerSideJson.rows = 500000;
                parameters.ServerSideJson.first = 0;
            }

            var param = JsonConvert.SerializeObject(parameters);
            //return await _unitOfWork.StoreProcedure<ResponseDto<ObtenerReporteTrazaDto>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST11.prc_consultar_reportes_traza", param);
            return await _unitOfWork.StoreProcedure<ResponseDto<ObtenerReporteTrazaDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_consultar_reportes_traza", param);
        }

        public async Task<ResponseDto<ObtenerReporteTrazaDto>> GetReporteTrazaContratistaAsync(OrdenesContratistaRequestDto parameters, bool ExportExcel = false)
        {
            parameters.id_contratista_persona = parameters.id_contratista_persona ?? "-2";
            parameters.id_zona = parameters.id_zona ?? "-2";
            //parameters.id_ruta_archivo_servidor = _parametrosOptions.Ambiente.ToLower() == "desarrollolocal" ? (int)Enum_RutasArchivos.ExcelScrMasivOrdBase64Local : (int)Enum_RutasArchivos.ExcelScrMasivOrdBase64;
            parameters.ServerSideJson = parameters.ServerSide == null ? null : JsonConvert.DeserializeObject<QueryOp360ServerSide>(parameters.ServerSide);
            parameters.fechaJson = parameters.fecha == null ? null : JsonConvert.DeserializeObject<DateTime[]>(parameters.fecha.Replace("null", ""));

            if (ExportExcel)
            {
                parameters.ServerSideJson.rows = 500000;
                parameters.ServerSideJson.first = 0;
            }

            var param = JsonConvert.SerializeObject(parameters);
            //return await _unitOfWork.StoreProcedure<ResponseDto<ObtenerReporteTrazaDto>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST11.prc_consultar_reportes_traza_contratista", param);
            return await _unitOfWork.StoreProcedure<ResponseDto<ObtenerReporteTrazaDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_consultar_reportes_traza_contratista", param);
        }

        public async Task<ResponseDto> Procesar_Crear_Tabla_Tmp_SCR(QueryOp360CargueMasivoData parameters)
        {
            return _unitOfWork.Op360Repository.Procesar_Crear_Tabla_Tmp_SCR(parameters);
            //var param = JsonConvert.SerializeObject(parameters);
            //return await _unitOfWork.StoreProcedure<ResponseDto<FormdataResponseMasivo>>().ExecuteStoreProcedureAsync("aire.PKG_G_CARLOS_VARGAS_TEST12.prc_crear_tbl_temporal", param);
            //return await _unitOfWork.StoreProcedure<ResponseDto<FormdataResponseMasivo>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_crear_tbl_temporal", param);
        }
        #endregion
    }
}