using BrasaBot.Models;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BrasaBot.Controllers
{
    public class FuncionarioController : Controller
    {
        private readonly IFuncionarioService _funcionarioService;

        public FuncionarioController(IFuncionarioService funcionarioService)
        {
            _funcionarioService = funcionarioService;
        }

        // GET: /Funcionario
        public async Task<IActionResult> Index(string? busca)
        {
            var funcionarios = await _funcionarioService.BuscarAsync(busca ?? string.Empty);
            ViewBag.Busca = busca;
            return View(funcionarios);
        }

        // GET: /Funcionario/Create
        public IActionResult Create()
        {
            return View(new FuncionarioViewModel());
        }

        // POST: /Funcionario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FuncionarioViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var funcionario = new Funcionario
            {
                Nome = vm.Nome,
                CPF = vm.CPF ?? string.Empty,
                Telefone = vm.Telefone,
                Logradouro = vm.Logradouro,
                Numero = vm.Numero,
                Bairro = vm.Bairro,
                CEP = vm.CEP,
                Cargo = vm.Cargo,
                DataAdmissao = vm.DataAdmissao,
                Ativo = true
            };

            var sucesso = await _funcionarioService.CadastrarAsync(funcionario);

            if (!sucesso)
            {
                ModelState.AddModelError("CPF", "Já existe um funcionário cadastrado com este CPF.");
                return View(vm);
            }

            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Funcionario/Edit/5
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
                Cargo = funcionario.Cargo,
                DataAdmissao = funcionario.DataAdmissao,
                Ativo = funcionario.Ativo
            };

            return View(vm);
        }

        // POST: /Funcionario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FuncionarioViewModel vm)
        {
            if (id != vm.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(vm);

            var funcionario = await _funcionarioService.ObterPorIdAsync(id);
            if (funcionario == null)
                return NotFound();

            funcionario.Nome = vm.Nome;
            funcionario.CPF = vm.CPF ?? string.Empty;
            funcionario.Telefone = vm.Telefone;
            funcionario.Logradouro = vm.Logradouro;
            funcionario.Numero = vm.Numero;
            funcionario.Bairro = vm.Bairro;
            funcionario.CEP = vm.CEP;
            funcionario.Cargo = vm.Cargo;
            funcionario.DataAdmissao = vm.DataAdmissao;

            await _funcionarioService.AtualizarAsync(funcionario);

            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Funcionario/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var funcionario = await _funcionarioService.ObterPorIdAsync(id);
            if (funcionario == null)
                return NotFound();

            return View(funcionario);
        }

        // POST: /Funcionario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _funcionarioService.ExcluirAsync(id);
            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}