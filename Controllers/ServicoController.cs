using BrasaBot.Models;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BrasaBot.Controllers
{
    public class ServicoController : Controller
    {
        private readonly IServicoService _servicoService;

        public ServicoController(IServicoService servicoService)
        {
            _servicoService = servicoService;
        }

        public async Task<IActionResult> Index(string? busca)
        {
            var servicos = await _servicoService.BuscarAsync(busca ?? string.Empty);
            ViewBag.Busca = busca;
            return View(servicos);
        }

        public IActionResult Create()
        {
            return View(new ServicoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServicoViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var servico = new Servico
            {
                Nome = vm.Nome,
                Valor = vm.Valor,
                Ativo = vm.Ativo
            };

            await _servicoService.CadastrarAsync(servico);

            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var servico = await _servicoService.ObterPorIdAsync(id);
            if (servico == null)
                return NotFound();

            var vm = new ServicoViewModel
            {
                Id = servico.Id,
                Nome = servico.Nome,
                Valor = servico.Valor,
                Ativo = servico.Ativo
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServicoViewModel vm)
        {
            if (id != vm.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(vm);

            var servico = await _servicoService.ObterPorIdAsync(id);
            if (servico == null)
                return NotFound();

            servico.Nome = vm.Nome;
            servico.Valor = vm.Valor;
            servico.Ativo = vm.Ativo;

            await _servicoService.AtualizarAsync(servico);

            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var servico = await _servicoService.ObterPorIdAsync(id);
            if (servico == null)
                return NotFound();

            return View(servico);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _servicoService.ExcluirAsync(id);
            TempData["Sucesso"] = "Sua ação foi concluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}