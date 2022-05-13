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
using Fornecedores.Models;

namespace Prov.App.Controllers
{
    public class ProdutosController : BaseController
    {

        private readonly IProdutoRepository _produtoRepository;
        private readonly IFornecedorRepository _fornecedoreRepository;
        private readonly IMapper _mapper;
        public ProdutosController(IProdutoRepository produtoRepository, IFornecedorRepository fornecedor, IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _fornecedoreRepository = fornecedor;
            _mapper = mapper;
        }
        // GET: Produtos
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProdutoDTO>>(await _produtoRepository.GetAllProdutos()));
        }

        // GET: Produtos/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var produtoDTO = await GetProduto(id);

            if (produtoDTO == null)
            {
                return NotFound();
            }

            return View(produtoDTO);
        }

        // GET: Produtos/Create
        public async Task<IActionResult> Create()
        {
            ProdutoDTO produto = await PopularFornecedores(new ProdutoDTO());
            return View(produto);
        }

        // POST: Produtos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            await _produtoRepository.Add(_mapper.Map<Produto>(produtoDTO));

            return RedirectToAction("Index");

        }

       
        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var produto = await GetProduto(id);

            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProdutoDTO produtoDTO)
        {
            if (id != produtoDTO.id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return View(produtoDTO);

            await _produtoRepository.Update(_mapper.Map<Produto>(produtoDTO));

            return RedirectToAction("Index");
        }

        // GET: Produtos/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var produto = await GetProduto(id);

            if (produto == null)
            {
                return NotFound();
            }


            return View(produto);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var produto = await GetProduto(id);

            if (produto == null)
                return NotFound();

            await _produtoRepository.Delete(id);

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
