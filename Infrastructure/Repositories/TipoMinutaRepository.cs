using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TipoMinutaRepository : BaseRepository<TipoMinuta>, ITipoMinutaRepository
    {
        public TipoMinutaRepository(DbModelContext context) : base(context)
        {
        }

        public async Task<TipoMinuta> GuardarTipoMinuta(TipoMinuta tipoMinuta)
        {
            await Add(tipoMinuta);
            
            return tipoMinuta;
        }
    }
}