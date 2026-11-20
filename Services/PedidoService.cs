using BrasaBot.Data;
using BrasaBot.Models;
using BrasaBot.Repositories;
using BrasaBot.Repositories.Interfaces;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BrasaBot.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMesaRepository _mesaRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly ICaixaService _caixaService;
        private readonly IServicoService _servicoService;
        private readonly IInsumoRepository _insumoRepository;                      
        private readonly IMovimentacaoEstoqueRepository _movimentacaoEstoqueRepo;
        private readonly AppDbContext _context;                                    

        public PedidoService(
            IPedidoRepository pedidoRepository,
            IMesaRepository mesaRepository,
            IProdutoRepository produtoRepository,
            ICaixaService caixaService,
            IServicoService servicoService,
            IInsumoRepository insumoRepository,                            
            IMovimentacaoEstoqueRepository movimentacaoEstoqueRepo,          
            AppDbContext context)                                           
        {
            _pedidoRepository = pedidoRepository;
            _mesaRepository = mesaRepository;
            _produtoRepository = produtoRepository;
            _caixaService = caixaService;
            _servicoService = servicoService;
            _insumoRepository = insumoRepository;                           
            _movimentacaoEstoqueRepo = movimentacaoEstoqueRepo;             
            _context = context;                                             
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

            var existente = await _pedidoRepository.ObterPedidoAbertoPorMesaAsync(mesaId);
            if (existente != null)
                return existente;

            var novoPedido = new Models.Pedido
            {
                Tipo = "Mesa",
                Status = "Aberto",
                DataCriacao = DateTime.Now,
                ClienteId = 2,
                FuncionarioId = funcionarioId,
                MesaId = mesaId,
                Desconto = 0,
                TaxaEntrega = 0,
                Total = 0
            };

            await _pedidoRepository.AdicionarAsync(novoPedido);

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

            await _pedidoRepository.RemoverItemAsync(item);

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

            var itemExistente = pedido.ItensPedido.FirstOrDefault(i => i.ProdutoId == produtoId);

            if (itemExistente != null)
            {
                itemExistente.Quantidade += quantidade;
                itemExistente.SubTotal = itemExistente.Quantidade * itemExistente.ValorUnitario;
            }
            else
            {
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

          
            foreach (var item in pedido.ItensPedido)
            {
                var produtoInsumos = await _context.ProdutoInsumos
                    .Where(pi => pi.ProdutoId == item.ProdutoId)
                    .ToListAsync();

                foreach (var pi in produtoInsumos)
                {
                    var insumo = await _insumoRepository.ObterPorIdAsync(pi.InsumoId);
                    if (insumo != null)
                    {
                        var quantidadeBaixa = pi.Quantidade * item.Quantidade;
                        insumo.EstoqueAtualQtd -= quantidadeBaixa;
                        await _insumoRepository.AtualizarAsync(insumo);

                        await _movimentacaoEstoqueRepo.AdicionarAsync(new MovimentacaoEstoque
                        {
                            DataHora = DateTime.Now,
                            Tipo = "Saida",
                            Quantidade = quantidadeBaixa,
                            Observacao = $"Venda pedido #{pedido.Id} - {item.Produto?.Nome}",
                            InsumoId = pi.InsumoId,
                            FuncionarioId = pedido.FuncionarioId
                        });
                    }
                }
            }
            

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

        public async Task<Pedido?> CriarPedidoDeliveryAsync(int clienteId, int funcionarioId)
        {
            var servicos = await _servicoService.ObterTodosAsync();
            var servicoEntrega = servicos.FirstOrDefault(s => s.Nome.ToLower().Contains("entrega"));

            var novoPedido = new Pedido
            {
                Tipo = "Delivery",
                Status = "Em Produção",
                DataCriacao = DateTime.Now,
                ClienteId = clienteId,
                FuncionarioId = funcionarioId,
                MesaId = null,
                Desconto = 0,
                TaxaEntrega = servicoEntrega?.Valor ?? 3.00m,
                Total = servicoEntrega?.Valor ?? 3.00m
            };

            await _pedidoRepository.AdicionarAsync(novoPedido);
            return novoPedido;
        }

        public async Task<bool> EnviarParaProducaoAsync(int pedidoId)
        {
            var pedido = await _pedidoRepository.ObterPorIdComItensAsync(pedidoId);
            if (pedido == null || pedido.Tipo != "Delivery")
                return false;

            if (!pedido.ItensPedido.Any())
                return false;

            pedido.Status = "Pronto para Entrega";
            await _pedidoRepository.AtualizarAsync(pedido);
            return true;
        }

        public async Task<bool> MarcarComoEmEntregaAsync(int pedidoId)
        {
            var pedido = await _pedidoRepository.ObterPorIdComItensAsync(pedidoId);
            if (pedido == null || pedido.Tipo != "Delivery")
                return false;

            pedido.Status = "Em Entrega";
            await _pedidoRepository.AtualizarAsync(pedido);
            return true;
        }

        public async Task<bool> MudarParaBalcaoAsync(int pedidoId)
        {
            var pedido = await _pedidoRepository.ObterPorIdComItensAsync(pedidoId);
            if (pedido == null || pedido.Tipo != "Delivery")
                return false;

            pedido.Tipo = "Balcão";
            pedido.TaxaEntrega = 0;
            pedido.Total = pedido.ItensPedido.Sum(i => i.SubTotal) - pedido.Desconto;
            if (pedido.Total < 0) pedido.Total = 0;

            await _pedidoRepository.AtualizarAsync(pedido);
            return true;
        }

        public async Task<Pedido?> CriarPedidoBalcaoAsync(int funcionarioId)
        {
            var novoPedido = new Pedido
            {
                Tipo = "Balcão",
                Status = "Aberto",
                DataCriacao = DateTime.Now,
                ClienteId = 2,
                FuncionarioId = funcionarioId,
                MesaId = null,
                Desconto = 0,
                TaxaEntrega = 0,
                Total = 0
            };

            await _pedidoRepository.AdicionarAsync(novoPedido);
            return novoPedido;
        }
    }
}