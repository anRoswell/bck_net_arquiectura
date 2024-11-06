using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;

namespace Infrastructure.Repositories
{
    public class BaseRepositoryOracle2<T> : IRepositoryOracle2<T> where T : BaseEntityOracle2
    {
        // Colocar como private, cuando sea necesario, pero este proceso arrojaría errores.
        // Se coloca protected para poder utilizar para ejecutar querys que contengan funciones, procedimientos almacenados, etc.
        protected readonly DbAireContext _contextaire;
        protected readonly DbSet<T> _entitiesOracle;

        public BaseRepositoryOracle2(DbAireContext contextaire)
        {
            _contextaire = contextaire;
            _entitiesOracle = contextaire.Set<T>();
        }

        public async Task<List<T>> GetAll()
        {
            return await _entitiesOracle.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _entitiesOracle.FindAsync(id);
        }

        public async Task<T> GetByCodigox(string Codigox)
        {
            return await _entitiesOracle.FindAsync(Codigox);
        }

        public async Task Add(T entity)
        {
            await _entitiesOracle.AddAsync(entity);
            await Commit();
        }

        public async Task Update(T entity)
        {
            _entitiesOracle.Update(entity);
            await Commit();
        }

        public async Task Delete(int id)
        {
            T entity = await GetById(id);
            _entitiesOracle.Remove(entity);

            await Commit();
        }

        public async Task Commit()
        {
            await _contextaire.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool isTracking = false, params Expression<Func<T, object>>[] includeObjectProperties)
        {
            IQueryable<T> query = _contextaire.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeObjectProperties != null)
            {
                foreach (Expression<Func<T, object>> include in includeObjectProperties)
                {
                    query = query.Include(include);
                }
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return (!isTracking) ? await query.AsNoTracking().ToListAsync() : await query.ToListAsync();
        }
    }
}
