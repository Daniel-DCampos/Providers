using Prov.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Prov.Business.Interfaces
{
    public interface IReposiytory<TEntity> : IDisposable where TEntity : Entidade
    {
        Task Add(TEntity entity);
        Task<TEntity> GetFromId(Guid id);
        Task<IEnumerable<TEntity>> GetAll();
        Task Update(TEntity entity);
        Task Delete(Guid id);
        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicade);
        Task<int> SaveChanges();
    }
}
