using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using OrderManagementSystem.Core.Contracts;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Repsitory.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace OrderManagementSystem.Repsitory
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly OrderManagementDbContext _dbContext;

        public GenericRepository(OrderManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<T?>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }
        public void Add(T entity)
        => _dbContext.Set<T>().Add(entity);

        public async Task AddAsync(T entity)
             => await _dbContext.Set<T>().AddAsync(entity);


        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public void Update(T entity)
        => _dbContext.Set<T>().Update(entity);

        public void Delete(T entity)
       => _dbContext.Set<T>().Remove(entity);

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }
        public async Task<IReadOnlyList<T>> GetAllWithFiltersAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }



        public async Task<IReadOnlyList<T>> GetAllWithFilters(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }

    

    }
}
