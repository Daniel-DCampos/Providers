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
using Microsoft.AspNetCore.Authorization;
using Prov.App.Extensions;

namespace Prov.App.Controllers
{
    [Authorize]
    public class FornecedoresController : BaseController
    {
        private readonly IFornecedorRepository _fornecedoreRepository;
        private readonly IFornecedorService _fornecedorService;
        private readonly IMapper _mapper;
        public FornecedoresController(IFornecedorRepository fornecedor, IEnderecoRepository enderecoRepository, IMapper mapper, IFornecedorService fornecedorService, INotificador notificador) : base(notificador)
        {
            _fornecedoreRepository = fornecedor;
            _fornecedorService  = fornecedorService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("lista-fornecedores")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<FornecedorDTO>>(await _fornecedoreRepository.GetAll()));
        }

        [AllowAnonymous]
        [Route("detalhes-fornecedor/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var fornecedorDTO = await GetForncecedorEndereco(id);
            if (fornecedorDTO == null)
            {
                return NotFound();
            }

            return View(fornecedorDTO);
        }

        [ClaimsAuthorize("Fornecedor", "Criar")]
        [Route("novo-fornecedor")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("novo-fornecedor")]
        [HttpPost]
        public async Task<IActionResult> Create(FornecedorDTO fornecedorDTO)
        {
            if (ModelState.IsValid)
            {
                await _fornecedorService.Add(_mapper.Map<Fornecedor>(fornecedorDTO));
                
                if (!OperacaoValida()) return View(fornecedorDTO);

                return RedirectToAction(nameof(Index));
            }
            return View(fornecedorDTO);
        }

        [ClaimsAuthorize("Fornecedor", "Editar")]
        [Route("editar-fornecedor/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var fornecedorDTO = await GetForncecedorProdutosEndereco(id);

            if (fornecedorDTO == null)
            {
                return NotFound();
            }
            return View(fornecedorDTO);
        }

        [Route("editar-fornecedor/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, FornecedorDTO fornecedorDTO)
        {
            if (id != fornecedorDTO.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(fornecedorDTO);

            await _fornecedorService.Update(_mapper.Map<Fornecedor>(fornecedorDTO));

            if (!OperacaoValida()) return View(await GetForncecedorProdutosEndereco(id));

            return RedirectToAction(nameof(Index));

        }

        [ClaimsAuthorize("Fornecedor", "Excluir")]
        [Route("remover-fornecedor/{id:guid}")]
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

        [Route("remover-fornecedor/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fornecedorDTO = await GetForncecedorEndereco(id);
            if(fornecedorDTO == null)
            {
                return NotFound();
            }
            await _fornecedorService.Remove(id);

            if (!OperacaoValida()) return View(await GetForncecedorProdutosEndereco(id));

            TempData["Sucesso"] = "produto excluído com sucesso!";

            return RedirectToAction(nameof(Index));
        }

        [Route("obter-endereco-fornecedor/{id:guid}")]
        [HttpGet]
        public async Task<IActionResult> ObterEndereco(Guid id)
        {
            var fornecedorDTO = await GetForncecedorEndereco(id);

            if (fornecedorDTO == null)
            {
                return NotFound();
            }

            return PartialView("_DetalhesEndereco", fornecedorDTO);
        }

        [Route("atualizar-endereco/{id:guid}")]
        [HttpGet]
        public async Task<IActionResult> AtualizarEndereco(Guid id)
        {
            var fornecedorDTO = await GetForncecedorEndereco(id);

            if (fornecedorDTO == null)
            {
                return NotFound();
            }

            return PartialView("_AtualizarEndereco", new FornecedorDTO() { Endereco = fornecedorDTO.Endereco });
        }

        [Route("atualizar-endereco/{id:guid}")]
        [HttpPost]
        
        public async Task<IActionResult> AtualizarEndereco(FornecedorDTO fornecedorDTO)
        {
            ModelState.Remove("Nome");
            ModelState.Remove("Documento");

            if(!ModelState.IsValid) return PartialView("_AtualizarEndereco", fornecedorDTO);

            await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(fornecedorDTO.Endereco));

            if (!OperacaoValida()) return PartialView("_AtualizarEndereco", fornecedorDTO);

            var url = Url.Action("ObterEndereco", "Fornecedores", new { id = fornecedorDTO.Endereco.FornecedorId });

            return Json(new { success = true, url });
        }



        private async Task<FornecedorDTO> GetForncecedorEndereco(Guid id)
        {
            return _mapper.Map<FornecedorDTO>(await _fornecedoreRepository.GetFornecedorEndereco(id));
        }

        private async Task<FornecedorDTO> GetForncecedorProdutosEndereco(Guid id)
        {
            return _mapper.Map<FornecedorDTO>(await _fornecedoreRepository.GetFornecedorProdutosEndereco(id));
        }
    }
}
