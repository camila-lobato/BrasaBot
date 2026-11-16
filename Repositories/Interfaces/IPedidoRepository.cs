using BrasaBot.Models;

namespace BrasaBot.Repositories.Interfaces
{
    public interface IPedidoRepository
    {
        Task<List<Pedido>> ObterPorTipoEStatusAsync(string tipo, string status);
        Task<Pedido?> ObterPorIdComItensAsync(int id);
        Task<Pedido?> ObterPedidoAbertoPorMesaAsync(int mesaId);
        Task AdicionarAsync(Pedido pedido);
        Task AtualizarAsync(Pedido pedido);
        Task<ItemPedido?> ObterItemPorIdAsync(int itemId);
        Task RemoverItemAsync(ItemPedido item);
    }
}