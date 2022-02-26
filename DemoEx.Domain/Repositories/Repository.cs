using DemoEx.Domain.Models;
using DemoEx.Domain.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEx.Domain.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private readonly LanguageCoursesDbContext context;
        public Repository(LanguageCoursesDbContext context)
        {
            this.context = context;
        }
        public virtual IQueryable<T> Items => context.Set<T>();
        public async Task<T> GetAsync(int id)
        {
            T item = await context.Set<T>().SingleOrDefaultAsync(item => item.Id == id);
            return item;
        }
        public async Task<T> AddAsync(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            await context.Set<T>().AddAsync(item);
            await context.SaveChangesAsync();
            return item;
        }
        public async Task RemoveAsync(int id)
        {
            var item = context.Set<T>().FirstOrDefaultAsync(item => item.Id == id).Result ?? new T { Id = id };
            context.Set<T>().Remove(item);
            await context.SaveChangesAsync();
        }
        public async Task UpdateAsync(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            context.Set<T>().Update(item);
            await context.SaveChangesAsync();
        }
    }
}
