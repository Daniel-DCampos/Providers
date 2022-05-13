using Fornecedores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prov.Business.Interfaces
{
    public interface IFornecedorRepository : IReposiytory<Fornecedor>
    {
        Task<Fornecedor> GetFornecedorEndereco(Guid id);
        Task<Fornecedor> GetFornecedorProdutosEndereco(Guid id);
    }
}
