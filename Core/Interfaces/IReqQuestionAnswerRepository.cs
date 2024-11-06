using Core.Entities;
using Core.ModelResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IReqQuestionAnswerRepository : IRepository<ReqQuestionAnswer>
    {
        Task<List<ReqQuestionAnswer>> GetMensajes(int id, int idRequerimientos, int esProveedor);
        Task<List<ResponseAction>> PostCrear(ReqQuestionAnswer reqQuestionAnswer);
    }
}
