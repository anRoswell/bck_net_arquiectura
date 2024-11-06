using Core.Entities;
using Core.ModelResponse;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Core.HubConfig;
using Core.CustomEntities;
using Core.Enumerations;

namespace Core.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationsHub> _hubContext;

        public NotificationsService(IUnitOfWork unitOfWork, IHubContext<NotificationsHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        public async Task<List<Notifications>> GetNotificacion(int idUsuario)
        {
            return await _unitOfWork.NotificationsRepository.GetNotificacion(idUsuario);
        }

        public async Task<List<ResponseAction>> PutNotificacion(int id, string CodUserUpdate)
        {
            return await _unitOfWork.NotificationsRepository.PutNotificacion(id, CodUserUpdate);
        }

        public async Task PushSocket(DataNotifications signalData)
        {
            await _hubContext.Clients.All.SendAsync(
                HubConectionsMethods.Notification,
                new DataModelSignalResponse<DataNotifications>
                {
                    Id = "N/R",
                    Data = signalData
                }
            );
        }
    }
}
