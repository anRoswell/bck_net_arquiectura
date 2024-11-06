using System.Threading.Tasks;
using Api.Responses;
using Core.QueryFilters;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.ModelProcess;
using System.Collections.Generic;
using Core.DTOs.FilesDto;

namespace Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class FilesController : ControllerBase
    {
        private readonly IFilesProcess _filesProcess;

        public FilesController(IFilesProcess filesProcess)
        {
            _filesProcess = filesProcess;
        }

        /// <summary>
        /// Almacenar archivos en File Server
        /// </summary>
        /// <param name="data">Archivos a guardar</param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post([FromForm] FormDataImagen data)
        {
            if (data.Files != null && data.IdPathFileServer > 0)
            {
                if (data.Files.Count > 1)
                {
                    //List<FileResponse> fileList = await _filesProcess.GetFilesCreated(data);
                    List<FileResponse> fileList = new() { };
                    var response = new ApiResponse<List<FileResponse>>(fileList, 200);
                    return Ok(response);
                }
                else
                {
                    //FileResponse file = await _filesProcess.GetFileCreated(data);
                    //var response = new ApiResponse<FileResponse>(file, 200);
                    //return Ok(response);
                    return Ok();
                }
            }
            else if (data.Files is null)
            {
                // HttpContext.Response.StatusCode = StatusCodes.
                return BadRequest("Parámetro inválido detectado, al menos debe haber un archivo.");
            }
            else
            {
                return BadRequest("Parámetro inválido detectado, el IdRuta no puede ser 0.");
            }
        }
    }
}