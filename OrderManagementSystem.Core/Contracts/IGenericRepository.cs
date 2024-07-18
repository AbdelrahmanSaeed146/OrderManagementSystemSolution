using OrderManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core.Contracts
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T?>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        Task<IReadOnlyList<T>> GetAllWithFilters(Expression<Func<T, bool>> predicate);
      
    }
}
