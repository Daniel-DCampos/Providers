using Prov.Business.Interfaces;
using Prov.Business.Models;
using Prov.Business.Services_Interfaces;
using Prov.Business.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prov.Business.Services
{
    public class FornecedorService : BaseService, IFornecedorService
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IEnderecoRepository _enderecoRepository;
        public FornecedorService(IFornecedorRepository fornecedor, IEnderecoRepository enderecoRepository, INotificador notificador) : base(notificador)
        {
            _fornecedorRepository = fornecedor;
            _enderecoRepository = enderecoRepository;
        }
        public async Task Add(Fornecedor fornecedor)
        {
            //Validar o estado da entidade

            var validator = new FornecedorValidation();
            var Endvalidator = new EnderecoValidation();

            if (!ExecutarValidacao(validator, fornecedor) || !ExecutarValidacao(Endvalidator, fornecedor.Endereco)) return;

            if (_fornecedorRepository.Find(f => f.Documento == fornecedor.Documento).Result.Any()) 
            { 
                Notificar("Já existe um fornecedor com este documento"); 
                return; 
            }

            await _fornecedorRepository.Add(fornecedor);
        }

        public async Task Remove(Guid id)
        {
            if (_fornecedorRepository.GetFornecedorProdutosEndereco(id).Result.Produtos.Any())
            {
                Notificar("O fornecedor possui produtos cadastrados!");
                return;
            }

            await _fornecedorRepository.Delete(id);
        }

        public async Task Update(Fornecedor fornecedor)
        {
            var validator = new FornecedorValidation();
            var Endvalidator = new EnderecoValidation();

            if (!ExecutarValidacao(validator, fornecedor) || !ExecutarValidacao(Endvalidator, fornecedor.Endereco)) return;

            if (_fornecedorRepository.Find(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id).Result.Any())
            {
                Notificar("Já existe um fornecedor com este documento");
                return;
            }

           await _fornecedorRepository.Update(fornecedor);
        }

        public async Task AtualizarEndereco(Endereco endereco)
        {
            var validator = new FornecedorValidation();
            var Endvalidator = new EnderecoValidation();

            if (!ExecutarValidacao(Endvalidator, endereco)) return;

            await _enderecoRepository.Update(endereco);
        }

        public void Dispose()
        {
            _fornecedorRepository?.Dispose();
        }
    }
}
