using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class RepresentantesLegalEmpresaReporitory : BaseRepository<RepresentantesLegalEmpresa>, IRepresentantesLegalEmpresaReporitory
    {
        public RepresentantesLegalEmpresaReporitory(DbModelContext context) : base(context)
        {
        }
    }
}
