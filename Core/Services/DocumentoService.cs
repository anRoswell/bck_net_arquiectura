using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.ModelResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public class DocumentoService : IDocumentoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DocumentoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Documento>> GetDocumentos()
        {
            return await _unitOfWork.DocumentoRepository.GetDocumentos();
        }

        public async Task<List<Documento>> GetDocumento(int id)
        {
           return await _unitOfWork.DocumentoRepository.GetDocumento(id);
        }

        public async Task<List<ResponseAction>> PostCrear(DocumentoDto doc)
        {
           return await _unitOfWork.DocumentoRepository.PostCrear(doc);
        }

        public async Task<List<ResponseAction>> PutActualizar(DocumentoDto doc)
        {
           return await _unitOfWork.DocumentoRepository.PutActualizar(doc);
        }

        public async Task<List<ResponseAction>> Delete(DocumentoDto doc)
        {
           return await _unitOfWork.DocumentoRepository.Delete(doc);
        }
    }
}
