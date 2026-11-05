using BrasaBot.Data;
using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrasaBot.Repositories
{
    public class MovimentacaoCaixaRepository : IMovimentacaoCaixaRepository
    {
        private readonly AppDbContext _context;

        public MovimentacaoCaixaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MovimentacaoCaixa>> ObterPorCaixaAsync(int caixaId)
        {
            return await _context.MovimentacoesCaixa
                .Where(m => m.CaixaId == caixaId)
                .OrderByDescending(m => m.DataHora)
                .ToListAsync();
        }

        public async Task<MovimentacaoCaixa?> ObterPorIdAsync(int id)
        {
            return await _context.MovimentacoesCaixa.FindAsync(id);
        }

        public async Task AdicionarAsync(MovimentacaoCaixa movimentacao)
        {
            await _context.MovimentacoesCaixa.AddAsync(movimentacao);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirAsync(int id)
        {
            var mov = await ObterPorIdAsync(id);
            if (mov == null)
                return;

            _context.MovimentacoesCaixa.Remove(mov);
            await _context.SaveChangesAsync();
        }
    }
}