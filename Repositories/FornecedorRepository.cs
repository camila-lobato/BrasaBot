using BrasaBot.Data;
using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrasaBot.Repositories
{
    public class FornecedorRepository : IFornecedorRepository
    {
        private readonly AppDbContext _context;

        public FornecedorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Fornecedor>> ObterTodosAsync()
        {
            return await _context.Fornecedores
                .Where(f => f.Ativo)
                .OrderBy(f => f.Nome)
                .ToListAsync();
        }

        public async Task<Fornecedor?> ObterPorIdAsync(int id)
        {
            return await _context.Fornecedores
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task AdicionarAsync(Fornecedor fornecedor)
        {
            await _context.Fornecedores.AddAsync(fornecedor);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Fornecedor fornecedor)
        {
            _context.Fornecedores.Update(fornecedor);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirAsync(int id)
        {
            var fornecedor = await ObterPorIdAsync(id);
            if (fornecedor == null)
                return;

            fornecedor.Ativo = false;
            _context.Fornecedores.Update(fornecedor);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteCnpjAsync(string cnpj)
        {
            return await _context.Fornecedores
                .AnyAsync(f => f.CNPJ == cnpj && f.Ativo);
        }
    }
}