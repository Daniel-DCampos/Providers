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
    public class FornecedoresController : BaseController
    {
        private readonly IFornecedorRepository _fornecedoreRepository;
        private readonly IMapper _mapper;
        public FornecedoresController(IFornecedorRepository fornecedor, IMapper mapper)
        {
            _fornecedoreRepository = fornecedor;
            _mapper = mapper;
        }

        // GET: Fornecedores
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<FornecedorDTO>>(await _fornecedoreRepository.GetAll()));
        }

        // GET: Fornecedores/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var fornecedorDTO = await GetForncecedorEndereco(id);
            if (fornecedorDTO == null)
            {
                return NotFound();
            }

            return View(fornecedorDTO);
        }

        // GET: Fornecedores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fornecedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FornecedorDTO fornecedorDTO)
        {
            if (ModelState.IsValid)
            {
                await _fornecedoreRepository.Add(_mapper.Map<Fornecedor>(fornecedorDTO));
                return RedirectToAction(nameof(Index));
            }
            return View(fornecedorDTO);
        }

        // GET: Fornecedores/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var fornecedorDTO = await GetForncecedorProdutosEndereco(id);

            if (fornecedorDTO == null)
            {
                return NotFound();
            }
            return View(fornecedorDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, FornecedorDTO fornecedorDTO)
        {
            if (id != fornecedorDTO.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(fornecedorDTO);

            await _fornecedoreRepository.Update(_mapper.Map<Fornecedor>(fornecedorDTO));

            return RedirectToAction(nameof(Index));

        }

        // GET: Fornecedores/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fornecedor = await GetForncecedorEndereco(id);
            if (fornecedor == null)
            {
                return NotFound();
            }

            return View(fornecedor);
        }

        // POST: Fornecedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fornecedorDTO = GetForncecedorEndereco(id);
            if(fornecedorDTO == null)
            {
                return NotFound();
            }
            await _fornecedoreRepository.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<FornecedorDTO> GetForncecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorDTO>(_fornecedoreRepository.GetFornecedorEndereco(id));
        }

        private async Task<FornecedorDTO> GetForncecedorProdutosEndereco(Guid id)
        {
            return _mapper.Map<FornecedorDTO>(_fornecedoreRepository.GetFornecedorProdutosEndereco(id));
        }
    }
}
