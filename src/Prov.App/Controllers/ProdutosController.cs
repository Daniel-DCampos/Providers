#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Prov.App.DTO;
using Prov.App.Data;
using Prov.Business.Interfaces;
using AutoMapper;
using Prov.Business.Models;
using Prov.Business.Services;
using Prov.Business.Services_Interfaces;

namespace Prov.App.Controllers
{
    public class ProdutosController : BaseController
    {

        private readonly IProdutoRepository _produtoRepository;
        private readonly IFornecedorRepository _fornecedoreRepository;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;
        public ProdutosController(IProdutoRepository produtoRepository, IFornecedorRepository fornecedor, IMapper mapper, IProdutoService produtoService, INotificador notificador) : base(notificador)
        {
            _produtoRepository = produtoRepository;
            _fornecedoreRepository = fornecedor;
            _produtoService = produtoService;
            _mapper = mapper;
        }

        [Route("lista-produto")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProdutoDTO>>(await _produtoRepository.GetAllProdutos()));
        }

        [Route("detalhes-produto/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var produtoDTO = await GetProduto(id);

            if (produtoDTO == null)
            {
                return NotFound();
            }

            return View(produtoDTO);
        }

        [Route("novo-produto")]
        public async Task<IActionResult> Create()
        {
            ProdutoDTO produto = await PopularFornecedores(new ProdutoDTO());
            return View(produto);
        }

        [Route("novo-produto")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoDTO produtoDTO)
        {
            produtoDTO = await PopularFornecedores(produtoDTO);

            if (!ModelState.IsValid)
                return View(produtoDTO);

            var imgPref = Guid.NewGuid() + "_" + produtoDTO.imagemUpload.FileName;


            if (!await UploadArquivo(produtoDTO.imagemUpload, imgPref))
                return View(produtoDTO);

            produtoDTO.imagem = imgPref;
            await _produtoService.Add(_mapper.Map<Produto>(produtoDTO));

            if (!OperacaoValida()) return View(produtoDTO);

            return RedirectToAction("Index");

        }


        [Route("editar-produto/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var produto = await GetProduto(id);

            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }

        [Route("editar-produto/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.id)
            {
                return NotFound();
            }

            var produtoAtualizaco = await GetProduto(id);
            produtoDTO.Fornecedor = produtoAtualizaco.Fornecedor;
            produtoDTO.imagem = produtoAtualizaco.imagem;

            if (!ModelState.IsValid)
                return View(produtoDTO);

            if (produtoDTO.imagemUpload != null)
            {
                var imgPref = Guid.NewGuid() + "_" + produtoDTO.imagemUpload.FileName;

                if (!await UploadArquivo(produtoDTO.imagemUpload, imgPref))
                    return View(produtoDTO);

                produtoAtualizaco.imagem = imgPref; 
            }

            produtoAtualizaco.Nome = produtoDTO.Nome;
            produtoAtualizaco.Descricao = produtoDTO.Descricao;
            produtoAtualizaco.Valor = produtoDTO.Valor;
            produtoAtualizaco.Ativo = produtoDTO.Ativo;

            await _produtoService.Update(_mapper.Map<Produto>(produtoAtualizaco));

            if (!OperacaoValida()) return View(produtoDTO);

            return RedirectToAction("Index");
        }

        [Route("remover-produto/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var produto = await GetProduto(id);

            if (produto == null)
            {
                return NotFound();
            }


            return View(produto);
        }

        [Route("remover-produto/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produto = await GetProduto(id);

            if (produto == null)
                return NotFound();

            await _produtoService.Remove(id);

            if (!OperacaoValida()) return View(produto);

            TempData["Sucesso"] = "produto excluído com sucesso!";

            return RedirectToAction("Index");
        }

        private async Task<ProdutoDTO> GetProduto(Guid id)
        {
            var produto = _mapper.Map<ProdutoDTO>(await _produtoRepository.GetProdutoFornecedor(id));
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorDTO>>(await _fornecedoreRepository.GetAll());
            return produto;
        }
        private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPref)
        {
            if (arquivo.Length <= 0) 
                return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", imgPref);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }

        private async Task<ProdutoDTO> PopularFornecedores(ProdutoDTO produto)
        {
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorDTO>>(await _fornecedoreRepository.GetAll());
            return produto;
        }
    }
}
