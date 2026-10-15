using BrasaBot.Data;
using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrasaBot.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Produto>> ObterTodosAsync()
        {
            return await _context.Produtos
                .Include(p => p.Categoria)
                .Where(p => p.Ativo)
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }

        public async Task<Produto?> ObterPorIdAsync(int id)
        {
            return await _context.Produtos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AdicionarAsync(Produto produto)
        {
            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Produto produto)
        {
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirAsync(int id)
        {
            var produto = await ObterPorIdAsync(id);
            if (produto == null)
                return;

            produto.Ativo = false;
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
        }
    }
}