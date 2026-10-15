using BrasaBot.Models;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BrasaBot.Controllers
{
    public class FornecedorController : Controller
    {
        private readonly IFornecedorService _fornecedorService;

        public FornecedorController(IFornecedorService fornecedorService)
        {
            _fornecedorService = fornecedorService;
        }

        public async Task<IActionResult> Index(string? busca)
        {
            var fornecedores = await _fornecedorService.BuscarAsync(busca ?? string.Empty);
            ViewBag.Busca = busca;
            return View(fornecedores);
        }

        public IActionResult Create()
        {
            return View(new FornecedorViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FornecedorViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var fornecedor = new Fornecedor
            {
                Nome = vm.Nome,
                CNPJ = vm.CNPJ,
                Email = vm.Email,
                Telefone = vm.Telefone,
                Logradouro = vm.Logradouro,
                Numero = vm.Numero,
                Bairro = vm.Bairro,
                CEP = vm.CEP,
                Ativo = true
            };

            var sucesso = await _fornecedorService.CadastrarAsync(fornecedor);

            if (!sucesso)
            {
                ModelState.AddModelError("CNPJ", "Já existe um fornecedor cadastrado com este CNPJ.");
                return View(vm);
            }

            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var fornecedor = await _fornecedorService.ObterPorIdAsync(id);
            if (fornecedor == null)
                return NotFound();

            var vm = new FornecedorViewModel
            {
                Id = fornecedor.Id,
                Nome = fornecedor.Nome,
                CNPJ = fornecedor.CNPJ,
                Email = fornecedor.Email,
                Telefone = fornecedor.Telefone,
                Logradouro = fornecedor.Logradouro,
                Numero = fornecedor.Numero,
                Bairro = fornecedor.Bairro,
                CEP = fornecedor.CEP,
                Ativo = fornecedor.Ativo
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FornecedorViewModel vm)
        {
            if (id != vm.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(vm);

            var fornecedor = await _fornecedorService.ObterPorIdAsync(id);
            if (fornecedor == null)
                return NotFound();

            fornecedor.Nome = vm.Nome;
            fornecedor.CNPJ = vm.CNPJ;
            fornecedor.Email = vm.Email;
            fornecedor.Telefone = vm.Telefone;
            fornecedor.Logradouro = vm.Logradouro;
            fornecedor.Numero = vm.Numero;
            fornecedor.Bairro = vm.Bairro;
            fornecedor.CEP = vm.CEP;

            await _fornecedorService.AtualizarAsync(fornecedor);

            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var fornecedor = await _fornecedorService.ObterPorIdAsync(id);
            if (fornecedor == null)
                return NotFound();

            return View(fornecedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _fornecedorService.ExcluirAsync(id);
            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}