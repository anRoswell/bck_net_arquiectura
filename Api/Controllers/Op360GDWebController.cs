using Api.Responses;
using Core.DTOs;
using Core.DTOs.ASEGREDWebDto;
using Core.DTOs.GDWebDto;
using Core.Entities.GDWebEntities;
using Core.Exceptions;
using Core.Interfaces;
using Core.QueryFilters;
using Core.QueryFilters.QueryFiltersGDWeb;
using Core.Services;
using Infrastructure.Validators.GDWebValidators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]

    public class Op360GDWebController : ControllerBase
    {
        private readonly IOp360GDWebService _GdWebService;

        public Op360GDWebController(IOp360GDWebService gdWebService)
        {
            _GdWebService = gdWebService;
        }

        #region End-pointListos

        #endregion

        #region CascaronesConModelo

        #endregion

        #region CacaronesSinModelo

        #region CargaInicial
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("CargaInicialGDWeb", Name = "CargaInicialGDWeb")]
        [Consumes("application/json")]
        public async Task<IActionResult> CargaInicialGDWeb()
        {
            try
            {
                var data = await _GdWebService.CargaInicialAsync();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_CargaInicialDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region ConsultaDeOrdenes
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("GetResumenGlobalOrdenes", Name = "GetResumenGlobalOrdenes")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetResumenGlobalOrdenesGD()
        {
            try
            {
                var listTest = await _GdWebService.GetResumenGlobalOrdenes();
                var response = new ApiResponse<Op360GD_ResumenGlobalOrdenesDto>(listTest.Datos, 200, listTest.Mensaje);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("GetAllOrdenesTrabajoContratistasGD", Name = "GetAllOrdenesTrabajoContratistasGD")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetAllOrdenesTrabajoContratistasGD([FromQuery] QueryOp360GD_OrdenesContratistas parameters)
        {
            try
            {
                /*
                // Intenta obtener el valor del "id_contratista" del diccionario HttpContext.Items y lo convierte a entero.
                int.TryParse(HttpContext.Items["id_contratista"]?.ToString() ?? "0", out int vidContratista);
                parameters.id_contratista = vidContratista; // Asigna el valor obtenido o predeterminado (0) a la propiedad id_contratista del objeto parameters.
                var validator2 = new QueryOp360GD_OrdenesContratistasValidator(); // Crea una instancia del validador QueryOp360OrdenesContratistasValidator.
                var validationResult2 = validator2.Validate(parameters); // Valida el objeto parameters utilizando el validador.
                if (!validationResult2.IsValid) // Verifica si la validación no es exitosa.
                {
                    return BadRequest(ErrorResponse.Op360ErrorTemplate(400, validationResult2.ToString("|"), 400));
                }
                else
                {
                    var lisTest = await _GdWebService.Consultar_Ordenes_Contratistas(parameters);
                    // Crea una instancia de GD_Orden_Response e inicializa sus propiedades con valores de listTest.Datos.
                    GD_Orden_ResponseDto orden_Response = new()
                    {
                        ordenes = lisTest.Datos.ordenes,
                        grafica_asignacion = lisTest.Datos.grafica_asignacion,
                        ArrayIdOrdenesFiltradas = lisTest.Datos.ArrayIdOrdenesFiltradas
                    };

                    var response = new ApiResponse<GD_Orden_ResponseDto>(orden_Response, 200, lisTest.Mensaje)
                    {
                        TotalRecords = lisTest?.Datos?.RegistrosTotales ?? 0,
                        Meta = new Metadata2()
                        {
                            RegistrosTotales = lisTest?.Datos?.RegistrosTotales ?? 0,
                            NoPagina = parameters.pageNumber ?? 0,
                            RegistrosPorPagina = parameters.pageSize ?? 0
                        }
                    };
                    return Ok(response);
                }*/

                var listTest = await _GdWebService.Consultar_Ordenes_Contratistas(parameters);
                var response = new ApiResponse<Op360GD_Orden_Response>(listTest.Datos, 200, listTest.Mensaje);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ObtenerOrdenesTrabajoAgrupadasOficinaCentral", Name = "GetAllOrdenesTrabajoAgrupadasGD")]
        [Consumes("application/json")]
        public async Task<IActionResult> GetAllOrdenesTrabajoAgrupadasGD([FromQuery] QueryOp360GDOrdenesAgrupada parameters)
        {
            try
            {
                parameters.id_contratista = parameters.id_contratista ?? -2;
                var listTest = await _GdWebService.ConsultarOrdenesAgrupadasAreaCentral(parameters);
                var response = new ApiResponse<Op360GD_Orden_Agrupada_Response>(listTest.Datos, 200, listTest.Mensaje);
                response.TotalRecords = listTest?.Datos?.ordenes_agrupadas?.Length ?? 0;
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ObtenerOrdenesTrabajoxIdGD", Name = "ObtenerOrdenesTrabajoxIdGD")]
        [Consumes("application/json")]
        public async Task<IActionResult> ObtenerOrdenesTrabajoxIdGD([FromQuery] QueryOp360GD_Orden parameters)
        {
            try
            {
                var listTest = await _GdWebService.ConsultarOrdenXId(parameters);
                var response = new ApiResponse<Op360GD_OrdenById_Response>(listTest.Datos, 200, listTest.Mensaje);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
        #endregion

        #region OrdenTrabajo
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPost("generarOT", Name = "generarOT")]
        [Consumes("application/json")]
        public async Task<IActionResult> generarOT([FromBody] Op360GD_GenerarOTDto parameters)
        {
            try
            {
                var data = await _GdWebService.GenerarOT(parameters);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPost("asignarTareaAOT", Name = "asignarTareaAOT")]
        [Consumes("application/json")]
        public async Task<IActionResult> asignarTareaAOT([FromBody] Op360GD_asignarTareaAOTDto parameters)
        {
            try
            {
                var data = await _GdWebService.asignarTareaAOT(parameters);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(parameters);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("certificarOrdenTrabajo", Name = "certificarOrdenTrabajo")]
        [Consumes("application/json")]
        public async Task<IActionResult> certificarOrdenTrabajo([FromBody] Op360GD_certificarOTDto parameters)
        {
            try
            {
                var data = await _GdWebService.certificarOT(parameters);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(parameters);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("CerrarOrdenTrabajo", Name = "CerrarOrdenTrabajo")]
        [Consumes("application/json")]
        public async Task<IActionResult> CerrarOrdenTrabajo([FromBody] Op360GD_CerrarOrdenTrabajoDto op360GD_CerrarOrdenTrabajo)
        {
            try
            {
                var data = await _GdWebService.CerrarOrdenTrabajo(op360GD_CerrarOrdenTrabajo);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ObtenerInfoOTxTareas", Name = "ObtenerInfoOTxTareas")]
        [Consumes("application/json")]
        public async Task<IActionResult> ObtenerInfoOTxTareas()
        {
            try
            {
                var data = await _GdWebService.ObtenerInfoOTxTareas();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_ObtenerInfoOTxTareasDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("AvisoPendienteOrdenTrabajo", Name = "AvisoPendienteOrdenTrabajo")]
        [Consumes("application/json")]
        public async Task<IActionResult> AvisoPendienteOrdenTrabajo([FromBody] Op360GD_AvisoPendienteOrdenTrabajoDto op360GD_AvisoPendienteOrdenTrabajo)
        {
            try
            {
                var data = await _GdWebService.AvisoPendienteOrdenTrabajo(op360GD_AvisoPendienteOrdenTrabajo);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("AvisoImprocedenteOrdenTrabajo", Name = "AvisoImprocedenteOrdenTrabajo")]
        [Consumes("application/json")]
        public async Task<IActionResult> AvisoImprocedenteOrdenTrabajo([FromBody] Op360GD_AvisoImprocedenteOrdenTrabajoDto op360GD_AvisoImprocedenteOrdenTrabajo)
        {
            try
            {
                var data = await _GdWebService.AvisoImprocedenteOrdenTrabajo(op360GD_AvisoImprocedenteOrdenTrabajo);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region RechazarSolicitud
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("RechazarSolicitud", Name = "RechazarSolicitud")]
        [Consumes("application/json")]
        public async Task<IActionResult> RechazarSolicitud([FromBody] Op360GD_RechazarSolicitudDto parameters)
        {
            try
            {
                var data = await _GdWebService.RechazarSolicitud(parameters);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(parameters);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region Circuitos
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ObtenerListaCircuitos", Name = "ObtenerListaCircuitos")]
        [Consumes("application/json")]
        public async Task<IActionResult> ObtenerListaCircuitos()
        {
            try
            {
                var data = await _GdWebService.ObtenerListaCircuitos();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_ObtenerListaCircuitosDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPost("CrearCircuitos", Name = "CrearCircuitos")]
        [Consumes("application/json")]
        public async Task<IActionResult> CrearCircuitos([FromBody] Op360GD_CrearCircuitoDto op360GD_CrearCircuito)
        {
            try
            {
                var data = await _GdWebService.CrearCircuito(op360GD_CrearCircuito);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("ActualizarCircuitos", Name = "ActualizarCircuitos")]
        [Consumes("application/json")]
        public async Task<IActionResult> ActualizarCircuitos([FromBody] Op360GD_ActualizarCircuitoDto op360GD_ActualizarCircuito)
        {
            try
            {
                var data = await _GdWebService.ActualizarCircuito(op360GD_ActualizarCircuito);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region SubEstacion
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ObtenerListaSubEstacion", Name = "ObtenerListaSubEstacion")]
        [Consumes("application/json")]
        public async Task<IActionResult> ObtenerListaSubEstacion()
        {
            try
            {
                var data = await _GdWebService.ObtenerListaSubEstacion();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_ObtenerListaSubEstacionDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPost("CrearSubEstacion", Name = "CrearSubEstacion")]
        [Consumes("application/json")]
        public async Task<IActionResult> CrearSubEstacion([FromBody] Op360GD_CrearSubEstacionDto op360GD_CrearSubEstacion)
        {
            try
            {
                var data = await _GdWebService.CrearSubEstacion(op360GD_CrearSubEstacion);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("ActualizarSubEstacion", Name = "ActualizarSubEstacion")]
        [Consumes("application/json")]
        public async Task<IActionResult> ActualizarSubEstacion([FromBody] Op360GD_ActualizarSubEstacionDto op360GD_ActualizarSubEstacion)
        {
            try
            {
                var data = await _GdWebService.ActualizarSubEstacion(op360GD_ActualizarSubEstacion);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region ListaReferenciaIconos
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ObtenerListaReferenciaIconos", Name = "ObtenerListaReferenciaIconos")]
        [Consumes("application/json")]
        public async Task<IActionResult> ObtenerListaReferenciaIconos()
        {
            try
            {
                var data = await _GdWebService.GetListaReferenciaIconos();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_GetListaReferenciaIconosDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region TipoTrabajo
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ObtenerListaTiposTrabajo", Name = "ObtenerListaTiposTrabajo")]
        [Consumes("application/json")]
        public async Task<IActionResult> ObtenerListaTiposTrabajo()
        {
            try
            {
                var data = await _GdWebService.GetListaTiposTrabajo();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_GetListaTiposTrabajoDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPost("CrearTipoTrabajo", Name = "CrearTipoTrabajo")]
        [Consumes("application/json")]
        public async Task<IActionResult> CrearTipoTrabajo([FromBody] Op360GD_CrearTipoTrabajoDto op360GD_CrearTipoTrabajo)
        {
            try
            {
                var data = await _GdWebService.CrearTipoTrabajo(op360GD_CrearTipoTrabajo);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("ActualizarTipoTrabajo", Name = "ActualizarTipoTrabajo")]
        [Consumes("application/json")]
        public async Task<IActionResult> ActualizarTipoTrabajo([FromBody] Op360GD_ActualizarTipoTrabajoDto op360GD_ActualizarTipoTrabajo)
        {
            try
            {
                var data = await _GdWebService.ActualizarTipoTrabajo(op360GD_ActualizarTipoTrabajo);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region TipoAvisos
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ObtenerListaTipoAvisos", Name = "ObtenerListaTipoAvisos")]
        [Consumes("application/json")]
        public async Task<IActionResult> ObtenerListaTipoAvisos()
        {
            try
            {
                var data = await _GdWebService.GetListaTipoAvisos();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_GetListaTipoAvisosDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPost("CrearTipoAviso", Name = "CrearTipoAviso")]
        [Consumes("application/json")]
        public async Task<IActionResult> CrearTipoAviso([FromBody] Op360GD_CrearTipoAvisoDto op360GD_CrearTipoAviso)
        {
            try
            {
                var data = await _GdWebService.CrearTipoAviso(op360GD_CrearTipoAviso);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("ActualizarTipoAviso", Name = "ActualizarTipoAviso")]
        [Consumes("application/json")]
        public async Task<IActionResult> ActualizarTipoAviso([FromBody] Op360GD_ActualizarTipoAvisoDto op360GD_ActualizarTipoAviso)
        {
            try
            {
                var data = await _GdWebService.ActualizarTipoAviso(op360GD_ActualizarTipoAviso);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region Ordenes
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ObtenerOrdenesDashBoard", Name = "ObtenerOrdenesDashBoard")]
        [Consumes("application/json")]
        public async Task<IActionResult> ObtenerOrdenesDashBoard()
        {
            try
            {
                var data = await _GdWebService.ObtenerOrdenesDashBoard();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_ObtenerOrdenesDashBoardDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ObtenerReporteOrdenesTiempos", Name = "ObtenerReporteOrdenesTiempos")]
        [Consumes("application/json")]
        public async Task<IActionResult> ObtenerReporteOrdenesTiempos()
        {
            try
            {
                var data = await _GdWebService.ObtenerReporteOrdenesTiempos();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_ObtenerReporteOrdenesTiemposDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ObtenerOrdenesAdministrar", Name = "ObtenerOrdenesAdministrar")]
        [Consumes("application/json")]
        public async Task<IActionResult> ObtenerOrdenesAdministrar()
        {
            try
            {
                var data = await _GdWebService.ObtenerOrdenesAdministrar();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_ObtenerOrdenesAdministrarDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region PdfActaCierre
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("GenerarPdfActaCierre", Name = "GenerarPdfActaCierre")]
        [Consumes("application/json")]
        public async Task<IActionResult> GenerarPdfActaCierre()
        {
            try
            {
                var data = await _GdWebService.GenerarPdfActaCierre();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_GenerarPdfActaCierreDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region RecorridoxBrigada
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ObtenerRecorridoxBrigadaRangoTiempo", Name = "ObtenerRecorridoxBrigadaRangoTiempo")]
        [Consumes("application/json")]
        public async Task<IActionResult> ObtenerRecorridoxBrigadaRangoTiempo()
        {
            try
            {
                var data = await _GdWebService.ObtenerRecorridoxBrigadaRangoTiempo();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_ObtenerRecorridoxBrigadaRangoTiempoDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region TurnoBrigada
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("CerrarTurnoBrigada", Name = "CerrarTurnoBrigada")]
        [Consumes("application/json")]
        public async Task<IActionResult> CerrarTurnoBrigada([FromBody] Op360GD_CerrarTurnoBrigadaDto op360GD_CerrarTurnoBrigada)
        {
            try
            {
                var data = await _GdWebService.CerrarTurnoBrigada(op360GD_CerrarTurnoBrigada);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region InfoAviso
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ObtenerInfoAvisoBasicoGeoreferenciado", Name = "ObtenerInfoAvisoBasicoGeoreferenciado")]
        [Consumes("application/json")]
        public async Task<IActionResult> ObtenerInfoAvisoBasicoGeoreferenciado()
        {
            try
            {
                var data = await _GdWebService.ObtenerInfoAvisoBasicoGeoreferenciado();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_ObtenerInfoAvisoBasicoGeoreferenciadoDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ObtenerInfoAvisoCompleto", Name = "ObtenerInfoAvisoCompleto")]
        [Consumes("application/json")]
        public async Task<IActionResult> ObtenerInfoAvisoCompleto()
        {
            try
            {
                var data = await _GdWebService.ObtenerInfoAvisoCompleto();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_ObtenerInfoAvisoCompletoDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ObtenerInfoAvisoBitacora", Name = "ObtenerInfoAvisoBitacora")]
        [Consumes("application/json")]
        public async Task<IActionResult> ObtenerInfoAvisoBitacora()
        {
            try
            {
                var data = await _GdWebService.ObtenerInfoAvisoBitacora();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_ObtenerInfoAvisoBitacoraDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ObtenerInfoAvisoCertificado", Name = "ObtenerInfoAvisoCertificado")]
        [Consumes("application/json")]
        public async Task<IActionResult> ObtenerInfoAvisoCertificado()
        {
            try
            {
                var data = await _GdWebService.ObtenerInfoAvisoCertificado();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_ObtenerInfoAvisoCertificadoDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region GestionarPeligro
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPost("GestionarPeligro", Name = "GestionarPeligro")]
        [Consumes("application/json")]
        public async Task<IActionResult> GestionarPeligro([FromBody] Op360GD_GestionarPeligroDto op360GD_GestionarPeligro)
        {
            try
            {
                var data = await _GdWebService.GestionarPeligro(op360GD_GestionarPeligro);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region CambiarSector
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("CambiarSector", Name = "CambiarSector")]
        [Consumes("application/json")]
        public async Task<IActionResult> CambiarSector([FromBody] Op360GD_CambiarSectorDto op360GD_CambiarSector)
        {
            try
            {
                var data = await _GdWebService.CambiarSector(op360GD_CambiarSector);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region DeclararImprocedente
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPut("DeclararImprocedente", Name = "DeclararImprocedente")]
        [Consumes("application/json")]
        public async Task<IActionResult> DeclararImprocedente([FromBody] Op360GD_DeclararImprocedenteDto op360GD_DeclararImprocedente)
        {
            try
            {
                var data = await _GdWebService.DeclararImprocedente(op360GD_DeclararImprocedente);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region CargueParamCertiAvisos
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPost("CargueParamCertiAvisos", Name = "CargueParamCertiAvisos")]
        [Consumes("application/json")]
        public async Task<IActionResult> CargueParamCertiAvisos([FromBody] Op360GD_CargueParamCertiAvisosDto op360GD_CargueParamCertiAvisos)
        {
            try
            {
                var data = await _GdWebService.CargueParamCertiAvisos(op360GD_CargueParamCertiAvisos);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
        #endregion

        #region Turnos
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPost("CargueTurnos", Name = "CargueTurnos")]
        [Consumes("application/json")]
        public async Task<IActionResult> CargueTurnos([FromBody] Op360GD_CargueTurnosDto op360GD_CargueTurnos)
        {
            try
            {
                var data = await _GdWebService.CargueTurnos(op360GD_CargueTurnos);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ConsultaDeTurnos", Name = "ConsultaDeTurnos")]
        [Consumes("application/json")]
        public async Task<IActionResult> ConsultaDeTurnos()
        {
            try
            {
                var data = await _GdWebService.ConsultaDeTurnos();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_ConsultadeTurnosDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpPost("CrearTurno", Name = "CrearTurno")]
        [Consumes("application/json")]
        public async Task<IActionResult> CrearTurno([FromBody] Op360GD_CrearTurnoDto op360GD_CrearTurno)
        {
            try
            {
                var data = await _GdWebService.CrearTurno(op360GD_CrearTurno);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpDelete("EliminarTurno/{id}", Name = "EliminarTurno")]
        [Consumes("application/json")]
        public async Task<IActionResult> EliminarTurno([FromRoute] Op360GD_EliminarTurnoDto op360GD_EliminarTurno)
        {
            try
            {
                var data = await _GdWebService.EliminarTurno(op360GD_EliminarTurno);
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<ResponseDto>(data, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #region ReporteLiquidacion
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ReporteLiquidacionxCumplimientoTurnos", Name = "ReporteLiquidacionxCumplimientoTurnos")]
        [Consumes("application/json")]
        public async Task<IActionResult> ReporteLiquidacionxCumplimientoTurnos()
        {
            try
            {
                var data = await _GdWebService.ReporteLiquidacionxCumplimientoTurnos();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_ReporteLiquidacionxCumplimientoTurnosDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ReporteLiquidacionxBrigada", Name = "ReporteLiquidacionxBrigada")]
        [Consumes("application/json")]
        public async Task<IActionResult> ReporteLiquidacionxBrigada()
        {
            try
            {
                var data = await _GdWebService.ReporteLiquidacionxBrigada();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_ReporteLiquidacionxBrigadaDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }

        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ReporteLiquidacionConsolidado", Name = "ReporteLiquidacionConsolidado")]
        [Consumes("application/json")]
        public async Task<IActionResult> ReporteLiquidacionConsolidado()
        {
            try
            {
                var data = await _GdWebService.ReporteLiquidacionConsolidado();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360GD_ReporteLiquidacionConsolidadoDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch(Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

        #endregion

    }
}
