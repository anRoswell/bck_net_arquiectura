using Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepresentantesLegalEmpresaService
    {
        public Task<RepresentantesLegalEmpresaDto> Add(RepresentantesLegalEmpresaDto dto);
        public Task<RepresentantesLegalEmpresaDto> Edit(RepresentantesLegalEmpresaDto dto);
        public Task<List<RepresentantesLegalEmpresaDto>> Get();
        public Task<RepresentantesLegalEmpresaDto> Get(int id);

        public Task<List<RepresentantesLegalEmpresaDto>> GetByEmpresa(int idEmpresa);
        public Task Delete(int id);
    }
}
