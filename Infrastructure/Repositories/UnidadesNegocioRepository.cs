using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UnidadesNegocioRepository : BaseRepository<UnidadNegocio>, IUnidadesNegocioRepository
    {
        private readonly IMapper _mapper;
        public UnidadesNegocioRepository(DbModelContext context, IMapper mapper): base(context) 
        {
            _mapper = mapper;
        }

        public async Task<UnidadNegocio> BuscarPorId(int id)
        {
            return await GetById(id);
        }

        public async Task<List<UnidadNegocio>> BuscarTodos()
        {
            return await GetAll();
        }

        public async Task<UnidadNegocio> Crear(UnidadesNegocioDto unidadesNegocio)
        {
            UnidadNegocio entity = _mapper.Map<UnidadNegocio>(unidadesNegocio);

            await Add(entity);

            return entity;
        }

        public async Task<UnidadNegocio> Editar(UnidadNegocio unidadesNegocio)
        {
            await Update(unidadesNegocio);

            return unidadesNegocio;
        }

        public async Task<UnidadNegocio> Eliminar(int id)
        {
            var entity = await GetById(id);

            await Delete(id);

            return entity;
        }
    }
}
