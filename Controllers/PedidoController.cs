using BrasaBot.Services;
using BrasaBot.Services.Interfaces;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BrasaBot.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoService _pedidoService;
        private readonly IUsuarioService _usuarioService;
        private readonly IProdutoService _produtoService;
        private readonly ICategoriaService _categoriaService;

        public PedidoController(
     IPedidoService pedidoService,
     IUsuarioService usuarioService,
     IProdutoService produtoService,
     ICategoriaService categoriaService)
        {
            _pedidoService = pedidoService;
            _usuarioService = usuarioService;
            _produtoService = produtoService;
            _categoriaService = categoriaService;
        }

        // GET: /Pedido
        public async Task<IActionResult> Index()
        {
            var vm = await _pedidoService.ObterIndexAsync();
            return View(vm);
        }


        // GET: /Pedido/VenderMesa/5
        public async Task<IActionResult> VenderMesa(int id)
        {
            var funcionarioId = ObterFuncionarioId();

            var pedido = await _pedidoService.AbrirOuObterPedidoMesaAsync(id, funcionarioId);
            if (pedido == null)
                return NotFound();

            var pedidoAtualizado = await _pedidoService.ObterPedidoAbertoPorMesaAsync(id);
            return View("VenderMesa", pedidoAtualizado);
        }

        // POST: /Pedido/RemoverItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoverItem(int itemId, int pedidoId, int mesaId)
        {
            var sucesso = await _pedidoService.RemoverItemAsync(itemId, pedidoId);

            if (sucesso)
                TempData["Sucesso"] = "Item removido com sucesso!";
            else
                TempData["Erro"] = "Não foi possível remover o item.";

            return RedirectToAction("VenderMesa", new { id = mesaId });
        }

        // Auxiliar: pega o FuncionarioId do usuário logado
        private int ObterFuncionarioId()
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(usuarioIdClaim))
                return 0;

            var usuario = _usuarioService.ObterPorIdAsync(int.Parse(usuarioIdClaim)).Result;
            return usuario?.FuncionarioId ?? 0;
        }

        // GET: /Pedido/AdicionarProduto?pedidoId=5&categoriaId=1
        public async Task<IActionResult> AdicionarProduto(int pedidoId, int? categoriaId)
        {
            var pedido = await _pedidoService.ObterPorIdAsync(pedidoId);
            if (pedido == null)
                return NotFound();

            var categorias = await _categoriaService.ObterTodasAsync();
            var produtos = await _produtoService.ObterTodosAsync();

            // Filtra por categoria (se selecionada)
            if (categoriaId.HasValue && categoriaId.Value > 0)
            {
                produtos = produtos.Where(p => p.CategoriaId == categoriaId.Value).ToList();
            }

            ViewBag.CategoriaSelecionada = categoriaId;

            var vm = new AdicionarProdutoViewModel
            {
                PedidoId = pedido.Id,
                TipoPedido = pedido.Tipo,
                MesaId = pedido.MesaId,
                Produtos = produtos.Where(p => p.Ativo).ToList(),
                Categorias = categorias,
                ItensAtuais = pedido.ItensPedido.ToList(),
                Total = pedido.Total
            };

            return View(vm);
        }

        // POST: /Pedido/AdicionarItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarItem(int pedidoId, int produtoId, int? categoriaId)
        {
            var pedido = await _pedidoService.AdicionarProdutoAsync(pedidoId, produtoId, 1);

            if (pedido == null)
            {
                TempData["Erro"] = "Erro ao adicionar produto.";
                return RedirectToAction("AdicionarProduto", new { pedidoId, categoriaId });
            }

            TempData["Sucesso"] = "Produto adicionado!";
            return RedirectToAction("AdicionarProduto", new { pedidoId, categoriaId });
        }

        // POST: /Pedido/RemoverItemAdicao
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoverItemAdicao(int itemId, int pedidoId, int? categoriaId)
        {
            await _pedidoService.RemoverItemAsync(itemId, pedidoId);
            TempData["Sucesso"] = "Item removido.";
            return RedirectToAction("AdicionarProduto", new { pedidoId, categoriaId });
        }

        // GET: /Pedido/FecharConta/5
        public async Task<IActionResult> FecharConta(int id)
        {
            var pedido = await _pedidoService.ObterPorIdAsync(id);
            if (pedido == null)
                return NotFound();

            if (!pedido.ItensPedido.Any())
            {
                TempData["Erro"] = "Não é possível fechar um pedido sem itens.";
                if (pedido.MesaId.HasValue)
                    return RedirectToAction("VenderMesa", new { id = pedido.MesaId.Value });
                return RedirectToAction(nameof(Index));
            }

            var vm = new FecharContaViewModel
            {
                Pedido = pedido,
                Itens = pedido.ItensPedido.ToList(),
                TotalPedido = pedido.Total,
                Desconto = 0,
                TotalPago = 0,
                ValorRestante = pedido.Total
            };

            return View(vm);
        }

        // POST: /Pedido/FecharConta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FecharConta(
            int pedidoId,
            decimal desconto,
            decimal valorDinheiro,
            decimal valorCredito,
            decimal valorDebito,
            decimal valorPix)
        {
            var pedido = await _pedidoService.ObterPorIdAsync(pedidoId);
            if (pedido == null)
                return NotFound();

            var pagamentos = new Dictionary<string, decimal>
    {
        { "Dinheiro", valorDinheiro },
        { "Crédito", valorCredito },
        { "Débito", valorDebito },
        { "PIX", valorPix }
    };

            var totalPago = pagamentos.Values.Sum();

            // Valida se o total pago cobre o pedido
            if (totalPago < pedido.Total - desconto)
            {
                TempData["Erro"] = $"O valor pago (R$ {totalPago:F2}) é menor que o total do pedido.";
                return RedirectToAction(nameof(FecharConta), new { id = pedidoId });
            }

            var sucesso = await _pedidoService.FecharPedidoAsync(pedidoId, pagamentos, desconto);

            if (!sucesso)
            {
                TempData["Erro"] = "Erro ao fechar o pedido.";
                return RedirectToAction(nameof(FecharConta), new { id = pedidoId });
            }

            TempData["Sucesso"] = "Pedido finalizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}