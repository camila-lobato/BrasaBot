using BrasaBot.Data;
using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrasaBot.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cliente>> ObterTodosAsync()
        {
            return await _context.Clientes
                .Where(c => c.Ativo)
                .OrderBy(c => c.Nome)
                .ToListAsync();
        }

        public async Task<Cliente?> ObterPorIdAsync(int id)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AdicionarAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirAsync(int id)
        {
            var cliente = await ObterPorIdAsync(id);
            if (cliente == null)
                return;

            cliente.Ativo = false;
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteCpfAsync(string cpf)
        {
            return await _context.Clientes
                .AnyAsync(c => c.CPF == cpf && c.Ativo);
        }
    }
}