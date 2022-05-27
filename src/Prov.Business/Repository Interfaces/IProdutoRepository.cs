using Prov.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prov.Business.Interfaces
{
    public interface IProdutoRepository : IReposiytory<Produto>
    {
        Task<IEnumerable<Produto>> GetProdutosPorFornecedor(Guid fornecedor_id);
        Task<IEnumerable<Produto>> GetAllProdutos();
        Task<Produto> GetProdutoFornecedor(Guid id);
    }
}
