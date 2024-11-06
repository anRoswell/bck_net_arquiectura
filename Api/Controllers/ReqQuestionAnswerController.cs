using Api.Responses;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.ModelResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
   // [Authorize(Policy = "ShouldBeAnAdmin")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class ReqQuestionAnswerController : ControllerBase
    {
        private readonly IReqQuestionAnswerService _reqQuestionAnswerService;

        public ReqQuestionAnswerController(IReqQuestionAnswerService reqQuestionAnswerService)
        {
            _reqQuestionAnswerService = reqQuestionAnswerService;
        }

        /// <summary>
        /// Consultar mensajes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idRequerimientos"></param>
        /// <param name="esProveedor"></param>
        /// <returns></returns>
        [HttpGet("Search", Name = "SearchMensajes")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchMensajes(int id, int idRequerimientos, int esProveedor)
        {
            try
            {
                List<ReqQuestionAnswer> resp = await _reqQuestionAnswerService.GetMensajes(id, idRequerimientos, esProveedor);

                var response = new ApiResponse<List<ReqQuestionAnswer>>(resp, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        /// <summary>
        /// Crear Mensaje
        /// </summary>
        /// <param name="reqQuestionAnswer"></param>
        /// <returns></returns>
        [HttpPost("Create", Name = "CreateMensaje")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateMensaje([FromBody] ReqQuestionAnswer reqQuestionAnswer)
        {
            try
            {
                var responseAction = await _reqQuestionAnswerService.PostCrear(reqQuestionAnswer);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                if (!responseAction[0].estado)
                {
                    response.Status = 400;
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error al intentar insertar registro. Detalle: {e.Message}");
            }
        }

    }
}
