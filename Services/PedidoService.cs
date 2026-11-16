using BrasaBot.Models;
using BrasaBot.Repositories;
using BrasaBot.Repositories.Interfaces;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;

namespace BrasaBot.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMesaRepository _mesaRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly ICaixaService _caixaService;

        public PedidoService(IPedidoRepository pedidoRepository, IMesaRepository mesaRepository, IProdutoRepository produtoRepository, ICaixaService caixaService)
        {
            _pedidoRepository = pedidoRepository;
            _mesaRepository = mesaRepository;
            _produtoRepository = produtoRepository;
            _caixaService = caixaService;
        }

        public async Task<PedidoIndexViewModel> ObterIndexAsync()
        {
            var vm = new PedidoIndexViewModel
            {
                MesasDisponiveis = await _mesaRepository.ObterPorStatusAsync("Disponível"),
                MesasOcupadas = await _mesaRepository.ObterPorStatusAsync("Ocupada"),
                MesasEmFechamento = await _mesaRepository.ObterPorStatusAsync("Em Fechamento"),

                PedidosDeliveryProducao = await _pedidoRepository.ObterPorTipoEStatusAsync("Delivery", "Em Produção"),
                PedidosDeliveryProntos = await _pedidoRepository.ObterPorTipoEStatusAsync("Delivery", "Pronto para Entrega"),
                PedidosDeliveryEmEntrega = await _pedidoRepository.ObterPorTipoEStatusAsync("Delivery", "Em Entrega")
            };

            return vm;
        }

        public async Task<Pedido?> AbrirOuObterPedidoMesaAsync(int mesaId, int funcionarioId)
        {
            var mesa = await _mesaRepository.ObterPorIdAsync(mesaId);
            if (mesa == null)
                return null;

            // Se já tem pedido aberto, retorna
            var existente = await _pedidoRepository.ObterPedidoAbertoPorMesaAsync(mesaId);
            if (existente != null)
                return existente;

            // Cria novo pedido
            var novoPedido = new Models.Pedido
            {
                Tipo = "Mesa",
                Status = "Aberto",
                DataCriacao = DateTime.Now,
                ClienteId = 2, // Cliente padrão (o "Cliente Balcão" que criamos)
                FuncionarioId = funcionarioId,
                MesaId = mesaId,
                Desconto = 0,
                TaxaEntrega = 0,
                Total = 0
            };

            await _pedidoRepository.AdicionarAsync(novoPedido);

            // Marca a mesa como ocupada
            mesa.Status = "Ocupada";
            await _mesaRepository.AtualizarAsync(mesa);

            return novoPedido;
        }

        public async Task<Pedido?> ObterPedidoAbertoPorMesaAsync(int mesaId)
        {
            return await _pedidoRepository.ObterPedidoAbertoPorMesaAsync(mesaId);
        }

        public async Task<bool> RemoverItemAsync(int itemId, int pedidoId)
        {
            var pedido = await _pedidoRepository.ObterPorIdComItensAsync(pedidoId);
            if (pedido == null)
                return false;

            var item = pedido.ItensPedido.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
                return false;

            await _pedidoRepository.RemoverItemAsync(item);   // ← chama o método do Repository

            // Recalcula o total
            pedido.Total = pedido.ItensPedido
                .Where(i => i.Id != itemId)
                .Sum(i => i.SubTotal) + pedido.TaxaEntrega - pedido.Desconto;
            if (pedido.Total < 0) pedido.Total = 0;

            await _pedidoRepository.AtualizarAsync(pedido);
            return true;
        }

        public async Task<Pedido?> ObterPorIdAsync(int pedidoId)
        {
            return await _pedidoRepository.ObterPorIdComItensAsync(pedidoId);
        }

        public async Task<Pedido?> AdicionarProdutoAsync(int pedidoId, int produtoId, int quantidade)
        {
            var pedido = await _pedidoRepository.ObterPorIdComItensAsync(pedidoId);
            if (pedido == null)
                return null;

            var produto = await _produtoRepository.ObterPorIdAsync(produtoId);
            if (produto == null)
                return null;

            // Verifica se o item já está no pedido (mesmo produto)
            var itemExistente = pedido.ItensPedido.FirstOrDefault(i => i.ProdutoId == produtoId);

            if (itemExistente != null)
            {
                // Aumenta a quantidade
                itemExistente.Quantidade += quantidade;
                itemExistente.SubTotal = itemExistente.Quantidade * itemExistente.ValorUnitario;
            }
            else
            {
                // Cria novo item
                var novoItem = new ItemPedido
                {
                    PedidoId = pedidoId,
                    ProdutoId = produtoId,
                    Quantidade = quantidade,
                    ValorUnitario = produto.Valor,
                    SubTotal = produto.Valor * quantidade
                };
                pedido.ItensPedido.Add(novoItem);
            }

            // Recalcula o total
            pedido.Total = pedido.ItensPedido.Sum(i => i.SubTotal) + pedido.TaxaEntrega - pedido.Desconto;
            if (pedido.Total < 0) pedido.Total = 0;

            await _pedidoRepository.AtualizarAsync(pedido);
            return pedido;
        }

        public async Task<bool> FecharPedidoAsync(int pedidoId, Dictionary<string, decimal> pagamentos, decimal desconto)
        {
            var pedido = await _pedidoRepository.ObterPorIdComItensAsync(pedidoId);
            if (pedido == null || !pedido.ItensPedido.Any())
                return false;

            var subtotal = pedido.ItensPedido.Sum(i => i.SubTotal) + pedido.TaxaEntrega;
            pedido.Desconto = desconto;
            pedido.Total = Math.Max(0, subtotal - desconto);
            pedido.Status = "Fechado";

            await _pedidoRepository.AtualizarAsync(pedido);

            if (pedido.MesaId.HasValue)
            {
                var mesa = await _mesaRepository.ObterPorIdAsync(pedido.MesaId.Value);
                if (mesa != null)
                {
                    mesa.Status = "Disponível";
                    await _mesaRepository.AtualizarAsync(mesa);
                }
            }

            // Registra os pagamentos no caixa aberto
            var caixaAberto = await _caixaService.ObterCaixaAbertoAsync();
            if (caixaAberto != null)
            {
                foreach (var (forma, valor) in pagamentos)
                {
                    if (valor <= 0) continue;

                    await _caixaService.RegistrarMovimentacaoAsync(
                        caixaAberto.Id, valor, "Entrada", forma,
                        $"Pedido #{pedido.Id}");
                }
            }

            return true;
        }
    }
}