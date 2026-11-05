using System.Security.Claims;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BrasaBot.Controllers
{
    public class CaixaController : Controller
    {
        private readonly ICaixaService _caixaService;
        private readonly IUsuarioService _usuarioService;

        public CaixaController(ICaixaService caixaService, IUsuarioService usuarioService)
        {
            _caixaService = caixaService;
            _usuarioService = usuarioService;
        }

        private int ObterUsuarioId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(idClaim ?? "0");
        }

        private int ObterFuncionarioId()
        {
            // Pegamos o FuncionarioId do Usuario logado
            var usuarioId = ObterUsuarioId();
            var usuario = _usuarioService.ObterPorIdAsync(usuarioId).Result;
            return usuario?.FuncionarioId ?? 0;
        }

        public async Task<IActionResult> Index()
        {
            var caixaAberto = await _caixaService.ObterCaixaAbertoAsync();

            var vm = new CaixaIndexViewModel
            {
                CaixaAberto = caixaAberto,
                Movimentacoes = caixaAberto != null
                    ? await _caixaService.ObterMovimentacoesAsync(caixaAberto.Id)
                    : new List<Models.MovimentacaoCaixa>(),
                CaixasDisponiveis = new[] { 1, 2, 3 }
                    .Select(n => new SelectListItem
                    {
                        Value = n.ToString(),
                        Text = $"Caixa {n:00}"
                    })
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Abrir(AbrirCaixaViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Erro"] = "Dados inválidos ao abrir o caixa.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _caixaService.AbrirCaixaAsync(
                    vm.NumeroCaixa, vm.SaldoInicial, vm.Observacao, ObterFuncionarioId());

                TempData["Sucesso"] = "Caixa aberto com sucesso!";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Erro"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InserirDinheiro(InserirDinheiroViewModel vm)
        {
            var caixaAberto = await _caixaService.ObterCaixaAbertoAsync();
            if (caixaAberto == null)
            {
                TempData["Erro"] = "Não há caixa aberto.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                TempData["Erro"] = "Dados inválidos.";
                return RedirectToAction(nameof(Index));
            }

            await _caixaService.InserirDinheiroAsync(caixaAberto.Id, vm.Valor, vm.Observacao);
            TempData["Sucesso"] = "Entrada registrada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RetirarDinheiro(RetirarDinheiroViewModel vm)
        {
            var caixaAberto = await _caixaService.ObterCaixaAbertoAsync();
            if (caixaAberto == null)
            {
                TempData["Erro"] = "Não há caixa aberto.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                TempData["Erro"] = "Dados inválidos.";
                return RedirectToAction(nameof(Index));
            }

            await _caixaService.RetirarDinheiroAsync(caixaAberto.Id, vm.Valor, vm.Observacao);
            TempData["Sucesso"] = "Saída registrada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirMovimentacao(ExcluirMovimentacaoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Erro"] = "Dados inválidos.";
                return RedirectToAction(nameof(Index));
            }

            // Valida que o e-mail informado é de um Administrador ativo e a senha bate
            var usuario = await _usuarioService.ValidarCredenciaisAsync(vm.EmailAdmin, vm.SenhaAdmin);
            if (usuario == null || usuario.NivelAcesso != "Administrador")
            {
                TempData["Erro"] = "Credenciais de administrador inválidas.";
                return RedirectToAction(nameof(Index));
            }

            var sucesso = await _caixaService.ExcluirMovimentacaoAsync(vm.MovimentacaoId);
            if (!sucesso)
                TempData["Erro"] = "Movimentação não encontrada.";
            else
                TempData["Sucesso"] = "Movimentação excluída com sucesso!";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Fechar()
        {
            var caixaAberto = await _caixaService.ObterCaixaAbertoAsync();
            if (caixaAberto == null)
            {
                TempData["Erro"] = "Não há caixa aberto.";
                return RedirectToAction(nameof(Index));
            }

            var vm = new FechamentoCaixaViewModel { CaixaId = caixaAberto.Id };
            ViewBag.Caixa = caixaAberto;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Fechar(FechamentoCaixaViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Erro"] = "Dados inválidos no fechamento.";
                return RedirectToAction(nameof(Index));
            }

            await _caixaService.FecharCaixaAsync(vm.CaixaId, vm);
            TempData["Sucesso"] = "Caixa fechado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}