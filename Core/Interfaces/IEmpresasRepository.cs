using Core.Entities;
using Core.ModelResponse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IEmpresasRepository
    {
        Task<List<Empresa>> GetEmpresas();
        Task<List<Empresa>> GetListEmpresas();
        Task<List<ResponseAction>> PostCrear(Empresa empresa);
        Task<List<ResponseAction>> PutEmpresa(Empresa empresa);
        Task<List<ResponseAction>> DeleteEmpresa(Empresa empresa);


    }
}
