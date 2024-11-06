using Core.DTOs;
using Core.ModelResponse;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ITipoMinutaService
    {
        public Task<TipoMinutaDto> GuardarTipoDeMinuta(TipoMinutaCreateCommand command);

        public Task<ResponseAction> EliminarTipoMinuta(int idTipoMinuta);

        public Task<List<TipoMinutaDto>> ObtenerTipoMinuta();
    }
}