using Prov.Business.Models;
using Prov.Business.Interfaces;
using Prov.Business.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prov.Business.Services_Interfaces;

namespace Prov.Business.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository, INotificador notificador) : base(notificador)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task Add(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;

            await _produtoRepository.Add(produto);
        }

        public async Task Remove(Guid id)
        {
            await _produtoRepository.Delete(id);
        }

        public async Task Update(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;

            await _produtoRepository.Update(produto);
        }
        public void Dispose()
        {
            _produtoRepository?.Dispose();
        }
    }
}
