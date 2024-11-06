using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IEmpresasSelectedCertEspService
    {
        Task<List<EmpresasSelectedCertEsp>> GetEmpresasSelectedCertificado(int id);
    }
}
