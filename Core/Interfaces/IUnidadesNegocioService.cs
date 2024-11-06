using Core.DTOs;
using Core.ModelResponse;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnidadesNegocioService
    {
        Task<List<ResponseAction>> Crear(UnidadesNegocioDto unidadesNegocio);
        Task<List<ResponseAction>> Editar(UnidadesNegocioDto unidadesNegocio);
        Task<List<ResponseAction>> Eliminar(int id);
        Task<UnidadesNegocioDto> BuscarPorId(int id);
        Task<List<UnidadesNegocioDto>> BuscarTodos();
    }
}
