using BrasaBot.Models;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BrasaBot.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        public async Task<IActionResult> Index(string? busca)
        {
            var clientes = await _clienteService.BuscarAsync(busca ?? string.Empty);
            ViewBag.Busca = busca;
            return View(clientes);
        }

        public IActionResult Create()
        {
            return View(new ClienteViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var cliente = new Cliente
            {
                Nome = vm.Nome,
                CPF = vm.CPF,
                Telefone = vm.Telefone,
                Logradouro = vm.Logradouro,
                Numero = vm.Numero,
                Bairro = vm.Bairro,
                CEP = vm.CEP,
                Ativo = true
            };

            var sucesso = await _clienteService.CadastrarAsync(cliente);

            if (!sucesso)
            {
                ModelState.AddModelError("CPF", "Já existe um cliente cadastrado com este CPF.");
                return View(vm);
            }

            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _clienteService.ObterPorIdAsync(id);
            if (cliente == null)
                return NotFound();

            var vm = new ClienteViewModel
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                CPF = cliente.CPF,
                Telefone = cliente.Telefone,
                Logradouro = cliente.Logradouro,
                Numero = cliente.Numero,
                Bairro = cliente.Bairro,
                CEP = cliente.CEP,
                Ativo = cliente.Ativo
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClienteViewModel vm)
        {
            if (id != vm.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(vm);

            var cliente = await _clienteService.ObterPorIdAsync(id);
            if (cliente == null)
                return NotFound();

            cliente.Nome = vm.Nome;
            cliente.CPF = vm.CPF;
            cliente.Telefone = vm.Telefone;
            cliente.Logradouro = vm.Logradouro;
            cliente.Numero = vm.Numero;
            cliente.Bairro = vm.Bairro;
            cliente.CEP = vm.CEP;

            await _clienteService.AtualizarAsync(cliente);

            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var cliente = await _clienteService.ObterPorIdAsync(id);
            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _clienteService.ExcluirAsync(id);
            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}