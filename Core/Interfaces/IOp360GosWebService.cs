namespace Core.Interfaces
{
    using System.Threading.Tasks;
    using Core.DTOs;
    using Core.DTOs.AsignarDesasignarOrdenesAGestoresDto;
    using Core.DTOs.ObtenerOrdenesTrabajoOficinaCentralDto;
    using Core.DTOs.CargaInicialGosDto;
    using Core.DTOs.Gos.GetOrdenById;
    using Core.DTOs.Gos.UpdateOrden;
    using Core.DTOs.Gos.CrearComentario;
    using Core.DTOs.Gos.CambiarEstadoOrden;
    using Core.DTOs.Gos.ObtenerArchivosInstancia;
    using Core.DTOs.Gos.ObtenerArchivosInstanciaDetalle;
    using Core.DTOs.Gos.ObtenerOrdenesTrabajoOficinaCentralDto;
    using Core.DTOs.Gos.Web.ConsultaOrdenesFechaDto;
    using Core.DTOs.Gos.Web.GetSoporteById;
    using Core.DTOs.Gos.Web.ConsultarOrdenesGestion;
    using Core.DTOs.Gos.Web.Dashboard;

    public interface IOp360GosWebService
	{
        /// <summary>
        /// Carga los parametros iniciales para web y mobile.
        /// </summary>
        /// <returns>Lista de selectores.</returns>
        public Task<ResponseDto<ParametrosInicialesResponseDto>> GetParametrosInicialesAsync();

        /// <summary>
        /// Carga las ordenes por id.
        /// </summary>
        /// <param name="getOrdenByIdRequest">parametro.</param>
        /// <returns>Listado de ordenes</returns>
        public Task<ResponseDto<GetOrdenByIdResponseDto>> GetOrdenByIdAsync(GetOrdenByIdRequest getOrdenByIdRequest);

        /// <summary>
        /// Actualiza la orden.
        /// </summary>
        /// <param name="updateOrdenRequestDto">Parametros.</param>
        /// <returns></returns>
        public Task<ResponseDto> UpdateOrdenAsync(UpdateOrdenRequestDto updateOrdenRequestDto, int myUser);

        /// <summary>
        /// Crea el comentario de la orden.
        /// </summary>
        /// <param name="crearComentarioRequestDto">Parametros.</param>
        /// <returns></returns>
        public Task<ResponseDto> CrearComentarioAsync(CrearComentarioRequestDto crearComentarioRequestDto, int myUser);

        /// <summary>
        /// Obtiene las ordenes del trabajo oficina central.
        /// </summary>
        /// <param name="obtenerOrdenesTrabajoOficinaCentralRequestDto">Los Parametros.</param>
        /// <returns>Ordenes.</returns>
		public Task<ResponsePaginationDto<ObtenerOrdenesTrabajoOficinaCentralResponseDto>> ObtenerOrdenesTrabajoOficinaCentralAsync(ObtenerOrdenesTrabajoOficinaCentralRequestDto obtenerOrdenesTrabajoOficinaCentralRequestDto);

        /// <summary>
        /// Asignar o desasignar una orden segun el flag.
        /// </summary>
        /// <param name="asignarDesasignarOrdenesAGestoresRequestDto">Parametros.</param>
        /// <returns></returns>
        public Task<ResponseDto> AsignarDesasignarOrdenesAGestoresAsync(AsignarDesasignarOrdenesAGestoresRequestDto asignarDesasignarOrdenesAGestoresRequestDto);

        /// <summary>
        /// Obtiene los archivos procesados en carga masiva.
        /// </summary>
        /// <param name="obtenerArchivosInstanciaRequestDto">Los Parametros.</param>
        /// <returns>Archivos.</returns>
        public Task<ResponsePaginationDto<ObtenerArchivosInstanciaResponseDto>> ObtenerArchivosInstanciaAsync(ObtenerArchivosInstanciaRequestDto obtenerArchivosInstanciaRequestDto);

        /// <summary>
        /// Obtiene los detalles del archivo procesado en carga masiva.
        /// </summary>
        /// <param name="obtenerArchivosInstanciaDetalleRequestDto">Los Parametros.</param>
        /// <returns>detalles del archivo.</returns>
        public Task<ResponsePaginationDto<ObtenerArchivosInstanciaDetalleResponseDto>> ObtenerArchivosInstanciaDetalleAsync(ObtenerArchivosInstanciaDetalleRequestDto obtenerArchivosInstanciaDetalleRequestDto);

        /// <summary>
        /// Cambia el estado de la orden.
        /// </summary>
        /// <param name="cambiarEstadoOrdenRequestDto">Parametros.</param>
        /// <returns></returns>
        public Task<ResponseDto> CambiarEstadoOrdenAsync(CambiarEstadoOrdenRequestDto cambiarEstadoOrdenRequestDto);

        /// <summary>
        /// Obtiene el reporte de ordenes en excel.
        /// </summary>
        /// <param name="consultaOrdenesFechaRequestDto">Los Parametros.</param>
        /// <returns>Reporte.</returns>
        public Task<ResponseDto<ConsultaOrdenesFechaResponseDto>> ConsultarOrdenesAreaCentralGosExcel(ConsultaOrdenesFechaRequestDto consultaOrdenesFechaRequestDto);

        /// <summary>
        /// Obtiene el base64 del soporte consultado.
        /// </summary>
        /// <param name="getSoporteByIdRequestDto">Parametro.</param>
        /// <returns>string</returns>
        public Task<ResponseDto<GetSoporteByIdResponseDto>> GetSoporteByIdAsync(GetSoporteByIdRequestDto getSoporteByIdRequestDto);

        /// <summary>
        /// Obtiene el reporte de ordenes gestion en excel.
        /// </summary>
        /// <param name="consultaOrdenesGestionSpDto">Los Parametros.</param>
        /// <returns>Reporte.</returns>
        public Task<ResponseDto<ConsultaOrdenesGestionResponseDto>> ConsultaOrdenesGestionGosExcel(ConsultaOrdenesGestionSpDto consultaOrdenesGestionSpDto);

        /// <summary>
        /// Carga datos del dashboard gos.
        /// </summary>
        /// <returns>Reporte.</returns>
        public Task<ResponseDto<ReporteDto>> GetDashboardAsync();

    }
}