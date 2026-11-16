using BrasaBot.Data;
using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrasaBot.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _context;

        public PedidoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Pedido>> ObterPorTipoEStatusAsync(string tipo, string status)
        {
            return await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Mesa)
                .Include(p => p.ItensPedido)
                .Where(p => p.Tipo == tipo && p.Status == status)
                .OrderBy(p => p.DataCriacao)
                .ToListAsync();
        }

        public async Task<Pedido?> ObterPorIdComItensAsync(int id)
        {
            return await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Mesa)
                .Include(p => p.Funcionario)
                .Include(p => p.ItensPedido)
                    .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Pedido?> ObterPedidoAbertoPorMesaAsync(int mesaId)
        {
            return await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Mesa)
                .Include(p => p.ItensPedido)
                    .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(p => p.MesaId == mesaId && p.Status == "Aberto");
        }

        public async Task AdicionarAsync(Pedido pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task<ItemPedido?> ObterItemPorIdAsync(int itemId)
        {
            return await _context.ItensPedido.FindAsync(itemId);
        }

        // ✅ ESTE é o método certo do Repository — recebe ItemPedido, não (int, int)
        public async Task RemoverItemAsync(ItemPedido item)
        {
            _context.ItensPedido.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}