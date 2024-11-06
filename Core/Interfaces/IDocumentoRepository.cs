using Core.DTOs;
using Core.Entities;
using Core.ModelResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IDocumentoRepository
    {
        Task<List<Documento>> GetDocumentos();
        Task<List<Documento>> GetDocumento(int id);
        Task<List<ResponseAction>> PostCrear(DocumentoDto doc);
        Task<List<ResponseAction>> PutActualizar(DocumentoDto doc);
        Task<List<ResponseAction>> Delete(DocumentoDto doc);
    }
}
