using BrasaBot.Data;
using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrasaBot.Repositories
{
    public class CaixaRepository : ICaixaRepository
    {
        private readonly AppDbContext _context;

        public CaixaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Caixa>> ObterTodosAsync()
        {
            return await _context.Caixas
                .Include(c => c.Funcionario)
                .OrderByDescending(c => c.DataAbertura)
                .ToListAsync();
        }

        public async Task<Caixa?> ObterPorIdAsync(int id)
        {
            return await _context.Caixas
                .Include(c => c.Funcionario)
                .Include(c => c.MovimentacoesCaixa)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Caixa?> ObterCaixaAbertoAsync()
        {
            return await _context.Caixas
                .Include(c => c.Funcionario)
                .FirstOrDefaultAsync(c => c.Status == "Aberto");
        }

        public async Task<Caixa?> ObterCaixaPorNumeroAbertoAsync(int numeroCaixa)
        {
            return await _context.Caixas
                .FirstOrDefaultAsync(c => c.NumeroCaixa == numeroCaixa && c.Status == "Aberto");
        }

        public async Task AdicionarAsync(Caixa caixa)
        {
            await _context.Caixas.AddAsync(caixa);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Caixa caixa)
        {
            _context.Caixas.Update(caixa);
            await _context.SaveChangesAsync();
        }
    }
}