using BrasaBot.Models;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BrasaBot.Controllers
{
    public class CargoController : Controller
    {
        private readonly ICargoService _cargoService;

        public CargoController(ICargoService cargoService)
        {
            _cargoService = cargoService;
        }

        public async Task<IActionResult> Index(string? busca)
        {
            var cargos = await _cargoService.BuscarAsync(busca ?? string.Empty);
            ViewBag.Busca = busca;
            return View(cargos);
        }

        public IActionResult Create()
        {
            return View(new CargoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CargoViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var cargo = new Cargo { Nome = vm.Nome, Ativo = vm.Ativo };
            await _cargoService.CadastrarAsync(cargo);

            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cargo = await _cargoService.ObterPorIdAsync(id);
            if (cargo == null) return NotFound();

            var vm = new CargoViewModel { Id = cargo.Id, Nome = cargo.Nome, Ativo = cargo.Ativo };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CargoViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) return View(vm);

            var cargo = await _cargoService.ObterPorIdAsync(id);
            if (cargo == null) return NotFound();

            cargo.Nome = vm.Nome;
            cargo.Ativo = vm.Ativo;
            await _cargoService.AtualizarAsync(cargo);

            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var cargo = await _cargoService.ObterPorIdAsync(id);
            if (cargo == null) return NotFound();
            return View(cargo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _cargoService.ExcluirAsync(id);
            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}