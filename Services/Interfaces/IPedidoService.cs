using BrasaBot.Models;
using BrasaBot.ViewModels;

namespace BrasaBot.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoIndexViewModel> ObterIndexAsync();
        Task<Pedido?> AbrirOuObterPedidoMesaAsync(int mesaId, int funcionarioId);
        Task<Pedido?> ObterPedidoAbertoPorMesaAsync(int mesaId);
        Task<bool> RemoverItemAsync(int itemId, int pedidoId);
        Task<Pedido?> AdicionarProdutoAsync(int pedidoId, int produtoId, int quantidade);
        Task<Pedido?> ObterPorIdAsync(int pedidoId);
        Task<bool> FecharPedidoAsync(int pedidoId, Dictionary<string, decimal> pagamentos, decimal desconto);
    }
}