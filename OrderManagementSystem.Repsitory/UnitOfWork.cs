using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Core;
using OrderManagementSystem.Core.Contracts;
using OrderManagementSystem.Core.Entities;
using OrderManagementSystem.Repsitory.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Repsitory
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrderManagementDbContext _dbContext;
        private Hashtable _repositories;

        public UnitOfWork(OrderManagementDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }


        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(key))
            {
                var repository = new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(key, repository);
            }

            return _repositories[key] as IGenericRepository<TEntity>;
        }
        public async Task<int> CompleteAsync()
                => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
                => await _dbContext.DisposeAsync();


    }
}
