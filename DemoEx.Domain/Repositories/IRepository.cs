using DemoEx.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEx.Domain.Repositories
{
    public interface IRepository<T> where T: class, IEntity
    {
        public IQueryable<T> Items { get; }
        public Task<T> Get(int id);
        public Task<T> Add(T item);
        public void Delete(int id);
        public void Update(T item);
    }
}
