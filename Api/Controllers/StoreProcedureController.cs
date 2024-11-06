namespace Api.Controllers
{
    using System.Threading.Tasks;
    using Api.Responses;
    using Core.DTOs;
    using Core.Interfaces;
    using Core.QueryFilters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class StoreProcedureController: ControllerBase
    {

        private readonly IStoreProcedureService _storeProcedureService;

        public StoreProcedureController(IStoreProcedureService storeProcedureService)
        {
            _storeProcedureService = storeProcedureService;
        }

        /// <summary>
        /// Consultar GetStoreProcedure
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetStoreProcedure", Name = "GetStoreProcedure")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStoreProcedure()
        {
            var listTest = await _storeProcedureService.Get();
            var response = new ApiResponse<ResponseDto<DatosDto>>(listTest, 200);

            return Ok(response);
        }

        /// <summary>
        /// Consultar GetStoreProcedure
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDataDummi", Name = "DataDummi")]
        [Consumes("application/json")]
        [AllowAnonymous]
        public async Task<IActionResult> DataDummi([FromQuery] QueryOp360Dummi parameter)
        {
            var listTest = await _storeProcedureService.Obtener_Datos_Dummi("aire.pkg_g_ordenes.prc_obtener_datos_dummi", parameter);
            var response = new ApiResponse<Datos_Dummi>(listTest.Datos, 200,listTest.Mensaje);

            return Ok(response);
        }
    }
}

