using BrasaBot.Models;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BrasaBot.Controllers
{
    public class DespesaController : Controller
    {
        private readonly IDespesaService _despesaService;
        private readonly IFornecedorService _fornecedorService;
        private readonly ICaixaService _caixaService;

        public DespesaController(
            IDespesaService despesaService,
            IFornecedorService fornecedorService,
            ICaixaService caixaService)
        {
            _despesaService = despesaService;
            _fornecedorService = fornecedorService;
            _caixaService = caixaService;
        }

        public async Task<IActionResult> Index()
        {
            var vm = await _despesaService.ObterIndexAsync();
            vm.Fornecedores = await ObterFornecedoresSelectList();
            vm.Caixas = await ObterCaixasSelectList();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar(NovaDespesaViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Erro"] = "Dados inválidos.";
                return RedirectToAction(nameof(Index));
            }

            var despesa = new Despesa
            {
                Descricao = vm.Descricao,
                Valor = vm.Valor,
                Vencimento = vm.Vencimento,
                Status = vm.Status,
                FormaPagamento = vm.FormaPagamento,
                Observacao = vm.Observacao,
                FornecedorId = vm.FornecedorId
            };

            await _despesaService.CadastrarAsync(despesa);
            TempData["Sucesso"] = "Despesa cadastrada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pagar(PagarDespesaViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Erro"] = "Dados inválidos.";
                return RedirectToAction(nameof(Index));
            }

            // Verifica se há caixa aberto
            var caixaAberto = await _caixaService.ObterCaixaAbertoAsync();
            if (caixaAberto == null)
            {
                TempData["Erro"] = "Não há caixa aberto. Abra o caixa antes de pagar despesas.";
                return RedirectToAction(nameof(Index));
            }

            var sucesso = await _despesaService.PagarAsync(vm.DespesaId, vm);
            if (!sucesso)
                TempData["Erro"] = "Não foi possível registrar o pagamento.";
            else
                TempData["Sucesso"] = "Pagamento registrado com sucesso!";

            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> ObterFornecedoresSelectList()
        {
            var fornecedores = await _fornecedorService.ObterTodosAsync();
            return fornecedores.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.Nome
            });
        }

        private async Task<IEnumerable<SelectListItem>> ObterCaixasSelectList()
        {
            var caixaAberto = await _caixaService.ObterCaixaAbertoAsync();
            if (caixaAberto == null)
                return new List<SelectListItem>();

            return new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = caixaAberto.Id.ToString(),
                    Text = $"Caixa Geral"
                }
            };
        }
    }
}