using BrasaBot.Data;
using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrasaBot.Repositories
{
    public class DespesaRepository : IDespesaRepository
    {
        private readonly AppDbContext _context;

        public DespesaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Despesa>> ObterTodosAsync()
        {
            return await _context.Despesas
                .Include(d => d.Fornecedor)
                .OrderByDescending(d => d.Vencimento)
                .ToListAsync();
        }

        public async Task<Despesa?> ObterPorIdAsync(int id)
        {
            return await _context.Despesas
                .Include(d => d.Fornecedor)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AdicionarAsync(Despesa despesa)
        {
            await _context.Despesas.AddAsync(despesa);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Despesa despesa)
        {
            _context.Despesas.Update(despesa);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> ObterTotalAsync()
        {
            return await _context.Despesas.SumAsync(d => d.Valor);
        }

        public async Task<decimal> ObterTotalPendenteAsync()
        {
            return await _context.Despesas
                .Where(d => d.Status == "A Pagar")
                .SumAsync(d => d.Valor);
        }

        public async Task<decimal> ObterTotalPagoAsync()
        {
            return await _context.Despesas
                .Where(d => d.Status == "Pago")
                .SumAsync(d => d.Valor);
        }
    }
}