using BrasaBot.Data;
using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrasaBot.Repositories
{
    public class InsumoRepository : IInsumoRepository
    {
        private readonly AppDbContext _context;

        public InsumoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Insumo>> ObterTodosAsync()
        {
            return await _context.Insumos
                .Include(i => i.Fornecedor)
                .Where(i => i.Ativo)
                .OrderBy(i => i.Nome)
                .ToListAsync();
        }

        public async Task<Insumo?> ObterPorIdAsync(int id)
        {
            return await _context.Insumos
                .Include(i => i.Fornecedor)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AdicionarAsync(Insumo insumo)
        {
            await _context.Insumos.AddAsync(insumo);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Insumo insumo)
        {
            _context.Insumos.Update(insumo);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirAsync(int id)
        {
            var insumo = await ObterPorIdAsync(id);
            if (insumo == null)
                return;

            insumo.Ativo = false;
            _context.Insumos.Update(insumo);
            await _context.SaveChangesAsync();
        }
    }
}