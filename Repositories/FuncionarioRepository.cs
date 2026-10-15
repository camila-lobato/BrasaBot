using BrasaBot.Data;
using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrasaBot.Repositories
{
    public class FuncionarioRepository : IFuncionarioRepository
    {
        private readonly AppDbContext _context;

        public FuncionarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Funcionario>> ObterTodosAsync()
        {
            return await _context.Funcionarios
                .Where(f => f.Ativo)
                .OrderBy(f => f.Nome)
                .ToListAsync();
        }

        public async Task<Funcionario?> ObterPorIdAsync(int id)
        {
            return await _context.Funcionarios
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task AdicionarAsync(Funcionario funcionario)
        {
            await _context.Funcionarios.AddAsync(funcionario);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Funcionario funcionario)
        {
            _context.Funcionarios.Update(funcionario);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirAsync(int id)
        {
            var funcionario = await ObterPorIdAsync(id);

            if (funcionario == null)
                return;

            funcionario.Ativo = false;
            _context.Funcionarios.Update(funcionario);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteCpfAsync(string cpf)
        {
            return await _context.Funcionarios
                .AnyAsync(f => f.CPF == cpf && f.Ativo);
        }
    }
}