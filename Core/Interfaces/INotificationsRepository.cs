using Core.CustomEntities;
using Core.Entities;
using Core.ModelResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface INotificationsRepository
    {
        Task<List<Notifications>> GetNotificacion(int idUsuario);
        Task<List<ResponseAction>> PutNotificacion(int id, string CodUserUpdate);
    }
}
