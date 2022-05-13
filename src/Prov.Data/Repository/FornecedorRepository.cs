using Fornecedores.Models;
using Microsoft.EntityFrameworkCore;
using Prov.Business.Interfaces;
using Prov.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prov.Data.Repository
{
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
    {

        public FornecedorRepository(ProvidersDbContext providersDbContext) : base(providersDbContext) { }

        public async Task<Fornecedor> GetFornecedorEndereco(Guid id)
        {
            return await providersDbContext.Fornecedores
                .AsNoTracking()
                .Include(f => f.Endereco)
                .FirstOrDefaultAsync(f => f.Id == id) ?? new Fornecedor();
        }

        public async Task<Fornecedor> GetFornecedorProdutosEndereco(Guid id)
        {
            return await providersDbContext.Fornecedores
                 .AsNoTracking()
                 .Include(f => f.Endereco)
                 .Include(f => f.Produtos)
                 .FirstOrDefaultAsync(f => f.Id == id) ?? new Fornecedor();
        }
    }
}
