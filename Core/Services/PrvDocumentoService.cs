using Core.CustomEntities.Parametros;
using Core.Entities;
using Core.Interfaces;
using Core.ModelResponse;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public class PrvDocumentoService : IPrvDocumentoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PrvDocumentoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PrvDocumento>> GetPrvDocumento(int id)
        {
            return await _unitOfWork.PrvDocumentoRepository.GetPrvDocumento(id);
        }

        public async Task<List<ResponseActionUrl>> DeleteDocumentoOther(QueryDeleteDocOther documento)
        {
            return await _unitOfWork.PrvDocumentoRepository.DeleteDocumentoOther(documento);
        }

        public async Task<List<PrvDocumento>> GetDocumentosProveedorReq(ParamDocumentosPrvReq parametros)
        {
            return await _unitOfWork.PrvDocumentoRepository.GetDocumentosProveedorReq(parametros);
        }
    }
}
