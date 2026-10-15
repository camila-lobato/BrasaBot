using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using BrasaBot.Data;
using BrasaBot.Repositories.Interfaces;

namespace BrasaBot.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> ObterTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> ObterPorIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task AdicionarAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirAsync(int id)
        {
            var entity = await ObterPorIdAsync(id);

            if (entity == null)
                return;

            // Se a entidade tiver a propriedade "Ativo", faz soft delete
            var propAtivo = typeof(T).GetProperty("Ativo");
            if (propAtivo != null && propAtivo.PropertyType == typeof(bool))
            {
                propAtivo.SetValue(entity, false);
                _dbSet.Update(entity);
            }
            else
            {
                _dbSet.Remove(entity);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _dbSet.FindAsync(id) != null;
        }
    }
}