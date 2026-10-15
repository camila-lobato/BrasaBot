using BrasaBot.Models;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BrasaBot.Controllers
{
    public class InsumoController : Controller
    {
        private readonly IInsumoService _insumoService;
        private readonly IFornecedorService _fornecedorService;

        public InsumoController(IInsumoService insumoService, IFornecedorService fornecedorService)
        {
            _insumoService = insumoService;
            _fornecedorService = fornecedorService;
        }

        public async Task<IActionResult> Index(string? busca)
        {
            var insumos = await _insumoService.BuscarAsync(busca ?? string.Empty);
            ViewBag.Busca = busca;
            return View(insumos);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new InsumoViewModel
            {
                Fornecedores = await ObterFornecedoresSelectList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InsumoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Fornecedores = await ObterFornecedoresSelectList();
                return View(vm);
            }

            var insumo = new Insumo
            {
                Nome = vm.Nome,
                EstoqueMinimo = vm.EstoqueMinimo,
                UnidadeMedida = vm.UnidadeMedida,
                EstoqueAtualQtd = vm.EstoqueAtualQtd,
                FornecedorId = vm.FornecedorId,
                Ativo = true
            };

            await _insumoService.CadastrarAsync(insumo);

            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var insumo = await _insumoService.ObterPorIdAsync(id);
            if (insumo == null)
                return NotFound();

            var vm = new InsumoViewModel
            {
                Id = insumo.Id,
                Nome = insumo.Nome,
                EstoqueMinimo = insumo.EstoqueMinimo,
                UnidadeMedida = insumo.UnidadeMedida,
                EstoqueAtualQtd = insumo.EstoqueAtualQtd,
                FornecedorId = insumo.FornecedorId,
                Fornecedores = await ObterFornecedoresSelectList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InsumoViewModel vm)
        {
            if (id != vm.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                vm.Fornecedores = await ObterFornecedoresSelectList();
                return View(vm);
            }

            var insumo = await _insumoService.ObterPorIdAsync(id);
            if (insumo == null)
                return NotFound();

            insumo.Nome = vm.Nome;
            insumo.EstoqueMinimo = vm.EstoqueMinimo;
            insumo.UnidadeMedida = vm.UnidadeMedida;
            insumo.EstoqueAtualQtd = vm.EstoqueAtualQtd;
            insumo.FornecedorId = vm.FornecedorId;

            await _insumoService.AtualizarAsync(insumo);

            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var insumo = await _insumoService.ObterPorIdAsync(id);
            if (insumo == null)
                return NotFound();

            return View(insumo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _insumoService.ExcluirAsync(id);
            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // Auxiliar: gera a lista de fornecedores para o combobox
        private async Task<IEnumerable<SelectListItem>> ObterFornecedoresSelectList()
        {
            var fornecedores = await _fornecedorService.ObterTodosAsync();
            return fornecedores.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.Nome
            });
        }
    }
}