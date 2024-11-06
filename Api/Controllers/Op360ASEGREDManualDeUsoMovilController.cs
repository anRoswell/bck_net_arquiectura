using Api.Responses;
using Core.DTOs.ASEGREDMovilDto;
using Core.Interfaces;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Core.DTOs.ManualDeUsoASEGREDMovil;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class Op360ASEGREDManualDeUsoMovilController : ControllerBase
    {
        private readonly IOp360ASEGREDManualDeUsoMovilService _manualDeUsoASEGREDMovil;

        public Op360ASEGREDManualDeUsoMovilController(IOp360ASEGREDManualDeUsoMovilService manualDeUsoASEGREDMovil)
        {
            _manualDeUsoASEGREDMovil = manualDeUsoASEGREDMovil;
        }

        #region ManualDeUso
        [Authorize(Policy = "ShouldBeAnAdminContratistas")]
        [HttpGet("ManualDeUsoMovil", Name = "ManualDeUsoMovil")]
        [Consumes("application/json")]
        public async Task<IActionResult> ManualDeUsoMovil()
        {
            try
            {
                var data = await _manualDeUsoASEGREDMovil.ManualDeUso();
                var statusCode = data.Codigo == 0 ? 200 : 400;
                var response = new ApiResponse<Op360ASEGRED_ManualDeUsoDto>(data.Datos, statusCode, data.Mensaje);

                return statusCode == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error: {e.Message}");
            }
        }
        #endregion

    }
}
