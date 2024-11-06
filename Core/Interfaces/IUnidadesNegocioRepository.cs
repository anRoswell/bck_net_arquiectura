using Core.DTOs;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnidadesNegocioRepository
    {
        Task<UnidadNegocio> Crear(UnidadesNegocioDto unidadesNegocio);
        Task<UnidadNegocio> Editar(UnidadNegocio unidadesNegocio);
        Task<UnidadNegocio> Eliminar(int id);
        Task<UnidadNegocio> BuscarPorId(int id);
        Task<List<UnidadNegocio>> BuscarTodos();
    }
}
