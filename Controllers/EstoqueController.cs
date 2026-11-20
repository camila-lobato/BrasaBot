using System.Security.Claims;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BrasaBot.Controllers
{
    public class EstoqueController : Controller
    {
        private readonly IEstoqueService _estoqueService;
        private readonly IInsumoService _insumoService;
        private readonly IUsuarioService _usuarioService;

        public EstoqueController(
            IEstoqueService estoqueService,
            IInsumoService insumoService,
            IUsuarioService usuarioService)
        {
            _estoqueService = estoqueService;
            _insumoService = insumoService;
            _usuarioService = usuarioService;
        }

        public async Task<IActionResult> Index()
        {
            var vm = await _estoqueService.ObterIndexAsync();
            var insumos = await _insumoService.ObterTodosAsync();

            vm.InsumosSelectList = insumos.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = $"{i.Nome} ({i.EstoqueAtualQtd} {i.UnidadeMedida})"
            });

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(RegistrarMovimentacaoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Erro"] = "Dados inválidos.";
                return RedirectToAction(nameof(Index));
            }

            var funcionarioId = ObterFuncionarioId();

            var sucesso = await _estoqueService.RegistrarMovimentacaoAsync(
                vm.InsumoId, vm.Tipo, vm.Quantidade, vm.Observacao, funcionarioId);

            if (!sucesso)
            {
                TempData["Erro"] = "Não foi possível registrar a movimentação. Verifique os dados.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Sucesso"] = "Movimentação registrada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private int ObterFuncionarioId()
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(usuarioIdClaim))
                return 0;

            var usuario = _usuarioService.ObterPorIdAsync(int.Parse(usuarioIdClaim)).Result;
            return usuario?.FuncionarioId ?? 0;
        }
    }
}