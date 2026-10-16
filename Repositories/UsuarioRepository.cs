using BrasaBot.Data;
using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrasaBot.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> ObterTodosAsync()
        {
            return await _context.Usuarios
                .Include(u => u.Funcionario)
                .Where(u => u.Ativo)
                .OrderBy(u => u.Funcionario.Nome)
                .ToListAsync();
        }

        public async Task<Usuario?> ObterPorIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Funcionario)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task AdicionarAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirAsync(int id)
        {
            var usuario = await ObterPorIdAsync(id);
            if (usuario == null)
                return;

            usuario.Ativo = false;
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteEmailAsync(string email)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Email == email && u.Ativo);
        }

        public async Task<bool> ExisteFuncionarioVinculadoAsync(int funcionarioId, int? ignorarUsuarioId = null)
        {
            var query = _context.Usuarios
                .Where(u => u.FuncionarioId == funcionarioId && u.Ativo);

            if (ignorarUsuarioId.HasValue)
                query = query.Where(u => u.Id != ignorarUsuarioId.Value);

            return await query.AnyAsync();
        }
    }
}