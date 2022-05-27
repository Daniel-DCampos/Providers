using Prov.Business.Models;
using Prov.Business.Models;

namespace Prov.Business.Services
{
    public interface IProdutoService : IDisposable
    {
        Task Add(Produto produto);
        Task Update(Produto produto);
        Task Remove(Guid id);
    }
}