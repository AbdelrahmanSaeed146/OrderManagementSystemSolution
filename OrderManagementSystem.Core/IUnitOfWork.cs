using OrderManagementSystem.Core.Contracts;
using OrderManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystem.Core
{
    public interface IUnitOfWork :IAsyncDisposable
    {

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

        Task<int> CompleteAsync();  

    }
}
