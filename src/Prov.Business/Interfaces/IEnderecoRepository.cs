using Fornecedores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prov.Business.Interfaces
{
    public interface IEnderecoRepository : IReposiytory<Endereco>
    {
        Task<Endereco> GetEdnerecoPorFornecedor(Guid fornecedor_id);
    }
}
