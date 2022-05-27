using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Prov.Business.Interfaces;
using Prov.Business.Models;
using Prov.Data.Context;

namespace Prov.Data.Repository
{
    public abstract class Repository<TEntity> : IReposiytory<TEntity> where TEntity : Entidade
    {
        protected readonly ProvidersDbContext providersDbContext;
        protected readonly DbSet<TEntity> dbSet;
        protected readonly EntityEntry<TEntity> dbEntry;

        public Repository(ProvidersDbContext db)
        {
            providersDbContext = db;
            dbSet = db.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicade)
        {
            return await dbSet.AsNoTracking().Where(predicade).ToListAsync();
        }
        public virtual async Task<TEntity> GetFromId(Guid id)
        {
            return await dbSet.FindAsync(id);
        }
        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task Add(TEntity entity)
        {
            dbSet.Add(entity);
            await SaveChanges();
        }

        public virtual async Task Delete(Guid id)
        {
            dbSet.Remove(await dbSet.FindAsync(id));
            await SaveChanges();
        }
     
        public virtual async Task Update(TEntity entity)
        {
            providersDbContext.Entry(entity).State = EntityState.Modified;
            await SaveChanges();
        }
        public async Task<int> SaveChanges()
        {
            return await providersDbContext.SaveChangesAsync();
        }

        public virtual void Dispose()
        {
            providersDbContext?.DisposeAsync();
        }
    }
}
