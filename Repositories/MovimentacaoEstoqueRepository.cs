using BrasaBot.Data;
using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrasaBot.Repositories
{
    public class MovimentacaoEstoqueRepository : IMovimentacaoEstoqueRepository
    {
        private readonly AppDbContext _context;

        public MovimentacaoEstoqueRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MovimentacaoEstoque>> ObterUltimasAsync(int limite = 50)
        {
            return await _context.MovimentacoesEstoque
                .Include(m => m.Insumo)
                .Include(m => m.Funcionario)
                .OrderByDescending(m => m.DataHora)
                .Take(limite)
                .ToListAsync();
        }

        public async Task<List<MovimentacaoEstoque>> ObterPorInsumoAsync(int insumoId)
        {
            return await _context.MovimentacoesEstoque
                .Include(m => m.Funcionario)
                .Where(m => m.InsumoId == insumoId)
                .OrderByDescending(m => m.DataHora)
                .ToListAsync();
        }

        public async Task<MovimentacaoEstoque?> ObterUltimaPorInsumoAsync(int insumoId)
        {
            return await _context.MovimentacoesEstoque
                .Where(m => m.InsumoId == insumoId)
                .OrderByDescending(m => m.DataHora)
                .FirstOrDefaultAsync();
        }

        public async Task AdicionarAsync(MovimentacaoEstoque movimentacao)
        {
            await _context.MovimentacoesEstoque.AddAsync(movimentacao);
            await _context.SaveChangesAsync();
        }
    }
}