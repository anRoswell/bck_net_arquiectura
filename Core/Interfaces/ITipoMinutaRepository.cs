using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ITipoMinutaRepository : IRepository<TipoMinuta>
    {
        public Task<TipoMinuta> GuardarTipoMinuta(TipoMinuta tipoMinuta);
    }
}
