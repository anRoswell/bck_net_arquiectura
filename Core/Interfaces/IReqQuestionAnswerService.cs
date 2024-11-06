using Core.Entities;
using Core.ModelResponse;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IReqQuestionAnswerService
    {
        Task<List<ReqQuestionAnswer>> GetMensajes(int id, int idRequerimientos, int esProveedor);
        Task<List<ResponseAction>> PostCrear(ReqQuestionAnswer reqQuestionAnswer);

    }
}
