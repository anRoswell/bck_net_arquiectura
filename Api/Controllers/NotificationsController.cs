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
    [Authorize(Policy = "ShouldBeAnAdminOrProv")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : Controller
    {
        private readonly INotificationsService _notificationsService;

        public NotificationsController(INotificationsService notificationsServices)
        {
            _notificationsService = notificationsServices;
        }

        [HttpGet("Search", Name = "SearchNotification")]
        [Consumes("application/json")]
        public async Task<IActionResult> SearchNotification()
        {
            try
            {
                int idUser = int.Parse(HttpContext.Items["UserID"].ToString());
                var resp = await _notificationsService.GetNotificacion(idUser);           

                var response = new ApiResponse<List<Notifications>>(resp, 200);
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la búsqueda. Detalle: {e.Message}");
            }
        }

        [HttpPut("Update", Name = "UpdateNotification")]
        [Consumes("application/json")]
        public async Task<IActionResult> UpdateNotification(int id)
        {
            try
            {
                string CodUserUpdate = HttpContext.Items["UserID"]?.ToString();
                var responseAction = await _notificationsService.PutNotificacion(id, CodUserUpdate);
                var response = new ApiResponse<List<ResponseAction>>(responseAction, 200);
                
                return Ok(response);
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error en la actualización del registro. Detalle: {e.Message}");
            }
        }
    }
}


