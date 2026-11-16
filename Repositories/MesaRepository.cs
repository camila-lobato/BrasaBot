using BrasaBot.Data;
using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrasaBot.Repositories
{
    public class MesaRepository : IMesaRepository
    {
        private readonly AppDbContext _context;

        public MesaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Mesa>> ObterTodasAsync()
        {
            return await _context.Mesas.OrderBy(m => m.Numero).ToListAsync();
        }

        public async Task<List<Mesa>> ObterPorStatusAsync(string status)
        {
            return await _context.Mesas
                .Where(m => m.Status == status)
                .OrderBy(m => m.Numero)
                .ToListAsync();
        }

        public async Task<Mesa?> ObterPorIdAsync(int id)
        {
            return await _context.Mesas.FindAsync(id);
        }

        public async Task AtualizarAsync(Mesa mesa)
        {
            _context.Mesas.Update(mesa);
            await _context.SaveChangesAsync();
        }
    }
}