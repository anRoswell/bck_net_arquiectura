using Core.Entities;
using Core.Interfaces;
using Core.ModelResponse;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class ReqQuestionAnswerService: IReqQuestionAnswerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReqQuestionAnswerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ReqQuestionAnswer>> GetMensajes(int id, int idRequerimientos, int esProveedor)
        {
            return await _unitOfWork.ReqQuestionAnswerRepository.GetMensajes(id, idRequerimientos, esProveedor);
        }

        public async Task<List<ResponseAction>> PostCrear(ReqQuestionAnswer reqQuestionAnswer)
        {
            return await _unitOfWork.ReqQuestionAnswerRepository.PostCrear(reqQuestionAnswer);
        }
    }
}
