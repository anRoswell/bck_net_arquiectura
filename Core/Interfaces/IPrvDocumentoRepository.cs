using Core.CustomEntities.Parametros;
using Core.Entities;
using Core.ModelResponse;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPrvDocumentoRepository
    {
        Task<List<PrvDocumento>> GetPrvDocumento(int id);
        Task<List<ResponseActionUrl>> DeleteDocumentoOther(QueryDeleteDocOther documento);
        Task<List<PrvDocumento>> GetDocumentosProveedorReq(ParamDocumentosPrvReq parametros);
    }
}
