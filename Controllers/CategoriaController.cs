using BrasaBot.Models;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BrasaBot.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        public async Task<IActionResult> Index(string? busca)
        {
            var categorias = await _categoriaService.BuscarAsync(busca ?? string.Empty);
            ViewBag.Busca = busca;
            return View(categorias);
        }

        public IActionResult Create()
        {
            return View(new CategoriaViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoriaViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var categoria = new Categoria
            {
                Nome = vm.Nome,
                Ativo = vm.Ativo
            };

            await _categoriaService.CadastrarAsync(categoria);

            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var categoria = await _categoriaService.ObterPorIdAsync(id);
            if (categoria == null)
                return NotFound();

            var vm = new CategoriaViewModel
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                Ativo = categoria.Ativo
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoriaViewModel vm)
        {
            if (id != vm.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(vm);

            var categoria = await _categoriaService.ObterPorIdAsync(id);
            if (categoria == null)
                return NotFound();

            categoria.Nome = vm.Nome;
            categoria.Ativo = vm.Ativo;

            await _categoriaService.AtualizarAsync(categoria);

            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var categoria = await _categoriaService.ObterPorIdAsync(id);
            if (categoria == null)
                return NotFound();

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoriaService.ExcluirAsync(id);
            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}