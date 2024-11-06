using Core.CustomEntities;
using Core.Entities;
using Core.Enumerations;
using Core.Exceptions;
using Core.HubConfig;
using Core.Interfaces;
using Core.ModelResponse;
using Infrastructure.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class NotificationsRepository : BaseRepository<Notifications>, INotificationsRepository
    {
        public NotificationsRepository(DbModelContext context) : base(context) { }

        public async Task<List<Notifications>> GetNotificacion(int idUsuario)
        {            
            try
            {
                SqlParameter[] parameters = new[] {
                new SqlParameter("@Operacion","2"),
                new SqlParameter("@notiCodUsario", idUsuario)
            };

                string sql = $"noti.SpNotifications @Operacion = @Operacion, @notiCodUsario = @notiCodUsario";

                var response = await _context.Notifications.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }            
        }

        public async Task<List<ResponseAction>> PutNotificacion(int id, string CodUserUpdate)
        {
            try
            {
                SqlParameter[] parameters = new[] {
                    new SqlParameter("@Operacion","3"),
                    new SqlParameter("@notiIdNotification", id),
                    new SqlParameter("@CodUserUpdate", CodUserUpdate ?? "")
                };

                string sql = $"noti.SpNotifications @Operacion = @Operacion, @notiIdNotification = @notiIdNotification, @CodUserUpdate = @CodUserUpdate";

                var response = await _context.ResponseActions.FromSqlRaw(sql, parameters: parameters).ToListAsync();
                return response;
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error: {e.Message}");
            }
        }
    }
}
