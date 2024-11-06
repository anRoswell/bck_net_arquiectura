using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepositoryOracle2<T> where T : BaseEntityOracle2
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> GetByCodigox(string Codigox);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(int id);
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
             bool isTracking = false, params Expression<Func<T, object>>[] includeObjectProperties);
    }
}
