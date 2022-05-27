using Prov.Business.Models;
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
    public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(ProvidersDbContext providersDbContext) : base(providersDbContext) { }

        public async Task<Endereco> GetEdnerecoPorFornecedor(Guid fornecedor_id)
        {
            return await providersDbContext.Enderecos
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.FornecedorId == fornecedor_id);
        }
    }
}
