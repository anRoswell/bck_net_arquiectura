using Core.Entities;
using Core.Interfaces;
using Core.ModelResponse;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public class NoticiasDocService : INoticiasDocService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NoticiasDocService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<NoticiasDoc>> GetDocumentosNoticia(int idNoticia)
        {
            return await _unitOfWork.NoticiasDocRepository.GetDocumentosNoticia(idNoticia);
        }

        public async Task<List<ResponseActionUrl>> AgregarImagenNoticia(NoticiasDoc entity)
        {
            return await _unitOfWork.NoticiasDocRepository.AgregarImagenNoticia(entity);
        }

        public async Task<List<ResponseAction>> DeleteImagenNoticia(QueryDeleteImagenNoticia query)
        {
            return await _unitOfWork.NoticiasDocRepository.DeleteImagenNoticia(query);
        }
    }
}
