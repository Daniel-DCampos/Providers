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
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(ProvidersDbContext providersDbContext) : base(providersDbContext) { }
        public async Task<IEnumerable<Produto>> GetAllProdutos()
        {
            return await providersDbContext.Produtos
                .AsNoTracking()
                .Include(p => p.Fornecedor)
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }

        public async Task<Produto> GetProdutoFornecedor(Guid id)
        {
            return await providersDbContext.Produtos
                .AsNoTracking()
                .Include(p => p.Fornecedor)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorFornecedor(Guid fornecedor_id)
        {
            return await Find(p => p.FornecedorId == fornecedor_id);
        }
    }
}
