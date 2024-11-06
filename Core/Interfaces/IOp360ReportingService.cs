namespace Core.Interfaces
{
    using Core.DTOs;
    using Core.DTOs.FilesDto;
    using Core.DTOs.Gos.Web.ConsultaOrdenesFechaDto;
    using Core.DTOs.Gos.Web.ConsultarOrdenesGestion;
    using Core.DTOs.ObtenerReporteTraza;
    using Core.QueryFilters;
    using System.Threading.Tasks;

    public interface IOp360ReportingService
    {
        Task<byte[]> ActaOsPdf(QueryActaOsPdf parameters);
        Task<FileResponsePdfDto> ObtenerJasperReportPdf(QueryJasperReportsParams parameters);
        Task<ResponseDto<(FileResponseOracleBase64, byte[])>> Consultar_Ordenes_Area_Central_Excel(QueryOp360Ordenes parameters, string format);
        Task<ResponseDto<(FileResponseOracleBase64, byte[])>> Consultar_Ordenes_Contratistas_Excel(QueryOp360OrdenesContratistas parameters, string format);
        Task<ResponseDto<(FileResponseOracleBase64, byte[])>> Consultar_Archivos_Instancia_Excel(QueryOp360ArchivosInstancia parameters, string format);
        Task<ResponseDto<(FileResponseOracleBase64, byte[])>> Consultar_Archivos_Instancia_Detalle_Excel(QueryOp360ArchivosInstanciaDetalle parameters, string format);
        Task<ResponseDto<(FileResponseOracleBase64, byte[])>> ConsultarReporteEjecutadosScrExcel(QueryOp360ReporteEjecutados parameters, string format);
        Task<ResponseDto<(FileResponseOracleBase64, byte[])>> PruebaServerSideExcel(QueryOp360ServerSideTest parameters, string format);
        //ejemplo
        Task<ResponseDto<(FileResponseOracleBase64, byte[])>> ConsultarReporteEjecutadosContratistasScrExcel(QueryOp360ReporteEjecutadosContratistas parameters, string format);
        Task<ResponseDto<(FileResponseOracleBase64, byte[])>> Consultar_Log_LegalizacionScr_Excel(QueryOp360LogLegalizacion parameters, string format);
        Task<ResponseDto<(FileResponseOracleBase64, byte[])>> Consultar_Log_LegalizacionScr_Excel_Contratista(QueryOp360LogLegalizacionContratistas parameters, string format);
        Task<ResponseDto<(FileResponseOracleBase64, byte[])>> Consultar_Reporte_TrazaScr_AreaCentral_Excel(OrdenesRequestDto parameters, string format);
        Task<ResponseDto<(FileResponseOracleBase64, byte[])>> Consultar_Reporte_TrazaScr_Contratistas_Excel(OrdenesContratistaRequestDto parameters, string format);

        #region Gos
        Task<ResponseDto<(FileResponseOracleBase64, byte[])>> ConsultarOrdenesAreaCentralGosExcel(ConsultaOrdenesFechaRequestDto parameters, string format);
        Task<ResponseDto<(FileResponseOracleBase64, byte[])>> ConsultaOrdenesGestionGosExcel(ConsultaOrdenesGestionDto parameters);
        #endregion
    }
}