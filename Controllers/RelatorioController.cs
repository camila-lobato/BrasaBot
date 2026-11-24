using System.Security.Claims;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrasaBot.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class RelatorioController : Controller
    {
        private readonly IRelatorioService _relatorioService;

        public RelatorioController(IRelatorioService relatorioService)
        {
            _relatorioService = relatorioService;
        }

        public async Task<IActionResult> Index()
        {
            var vm = await _relatorioService.ObterIndexAsync();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmitirVendas(DateTime dataInicial, DateTime dataFinal, string filtro)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var vm = await _relatorioService.ObterRelatorioVendasAsync(dataInicial, dataFinal, filtro ?? "Todos", usuarioId);

            return PartialView("_RelatorioVendasPartial", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmitirCaixa(DateTime dataInicial, DateTime dataFinal, string filtro)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var vm = await _relatorioService.ObterRelatorioCaixaAsync(dataInicial, dataFinal, filtro ?? "Todos", usuarioId);
            return PartialView("_RelatorioCaixaPartial", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmitirDespesas(DateTime dataInicial, DateTime dataFinal, string filtro)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var vm = await _relatorioService.ObterRelatorioDespesasAsync(dataInicial, dataFinal, filtro ?? "Todos", usuarioId);
            return PartialView("_RelatorioDespesasPartial", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmitirFinanceiro(DateTime dataInicial, DateTime dataFinal, string filtro)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var vm = await _relatorioService.ObterRelatorioFinanceiroAsync(dataInicial, dataFinal, usuarioId);
            return PartialView("_RelatorioFinanceiroPartial", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmitirProdutos(DateTime dataInicial, DateTime dataFinal, string filtro)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var vm = await _relatorioService.ObterRelatorioProdutosAsync(dataInicial, dataFinal, filtro ?? "Todos", usuarioId);
            return PartialView("_RelatorioProdutosPartial", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmitirEstoque(DateTime dataInicial, DateTime dataFinal, string filtro)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var vm = await _relatorioService.ObterRelatorioEstoqueAsync(filtro ?? "Todos", usuarioId);
            return PartialView("_RelatorioEstoquePartial", vm);
        }
    }
}