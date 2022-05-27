using Prov.Business.Models;

namespace Prov.Business.Services
{
    public interface IFornecedorService : IDisposable
    {
        Task Add(Fornecedor fornecedor);
        Task Update(Fornecedor fornecedor);
        Task Remove(Guid id);
        Task AtualizarEndereco (Endereco endereco);
    }
}