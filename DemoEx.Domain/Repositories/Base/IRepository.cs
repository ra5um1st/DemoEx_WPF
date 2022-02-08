using DemoEx.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEx.Domain.Repositories.Base
{
    public interface IRepository<T> where T : class, IEntity
    {
        public IQueryable<T> Items { get; }
        public Task<T> GetAsync(int id);
        public Task<T> AddAsync(T item);
        public Task RemoveAsync(int id);
        public Task UpdateAsync(T item);
    }
}
