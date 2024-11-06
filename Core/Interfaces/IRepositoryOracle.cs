﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepositoryOracle<T> where T : BaseEntityOracle
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);        
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(int id);
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
             bool isTracking = false, params Expression<Func<T, object>>[] includeObjectProperties);
    }
}
