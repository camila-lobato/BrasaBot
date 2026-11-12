using BrasaBot.Models;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BrasaBot.Controllers
{
    public class FuncionarioController : Controller
    {
        private readonly IFuncionarioService _funcionarioService;
        private readonly ICargoService _cargoService;

        public FuncionarioController(IFuncionarioService funcionarioService, ICargoService cargoService)
        {
            _funcionarioService = funcionarioService;
            _cargoService = cargoService;
        }

        public async Task<IActionResult> Index(string? busca)
        {
            var funcionarios = await _funcionarioService.BuscarAsync(busca ?? string.Empty);
            ViewBag.Busca = busca;
            return View(funcionarios);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new FuncionarioViewModel { Cargos = await ObterCargosSelectList() };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FuncionarioViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Cargos = await ObterCargosSelectList();
                return View(vm);
            }

            var funcionario = new Funcionario
            {
                Nome = vm.Nome,
                CPF = vm.CPF ?? string.Empty,
                Telefone = vm.Telefone,
                Logradouro = vm.Logradouro,
                Numero = vm.Numero,
                Bairro = vm.Bairro,
                CEP = vm.CEP,
                CargoId = vm.CargoId,
                DataAdmissao = vm.DataAdmissao,
                Ativo = true
            };

            var sucesso = await _funcionarioService.CadastrarAsync(funcionario);

            if (!sucesso)
            {
                ModelState.AddModelError("CPF", "Já existe um funcionário cadastrado com este CPF.");
                vm.Cargos = await ObterCargosSelectList();
                return View(vm);
            }

            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var funcionario = await _funcionarioService.ObterPorIdAsync(id);
            if (funcionario == null)
                return NotFound();

            var vm = new FuncionarioViewModel
            {
                Id = funcionario.Id,
                Nome = funcionario.Nome,
                CPF = funcionario.CPF,
                Telefone = funcionario.Telefone,
                Logradouro = funcionario.Logradouro,
                Numero = funcionario.Numero,
                Bairro = funcionario.Bairro,
                CEP = funcionario.CEP,
                CargoId = funcionario.CargoId,
                DataAdmissao = funcionario.DataAdmissao,
                Ativo = funcionario.Ativo,
                Cargos = await ObterCargosSelectList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FuncionarioViewModel vm)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                vm.Cargos = await ObterCargosSelectList();
                return View(vm);
            }

            var funcionario = await _funcionarioService.ObterPorIdAsync(id);
            if (funcionario == null) return NotFound();

            funcionario.Nome = vm.Nome;
            funcionario.CPF = vm.CPF ?? string.Empty;
            funcionario.Telefone = vm.Telefone;
            funcionario.Logradouro = vm.Logradouro;
            funcionario.Numero = vm.Numero;
            funcionario.Bairro = vm.Bairro;
            funcionario.CEP = vm.CEP;
            funcionario.CargoId = vm.CargoId;
            funcionario.DataAdmissao = vm.DataAdmissao;

            await _funcionarioService.AtualizarAsync(funcionario);

            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var funcionario = await _funcionarioService.ObterPorIdAsync(id);
            if (funcionario == null) return NotFound();
            return View(funcionario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _funcionarioService.ExcluirAsync(id);
            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> ObterCargosSelectList()
        {
            var cargos = await _cargoService.ObterTodosAsync();
            return cargos.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nome
            });
        }
    }
}