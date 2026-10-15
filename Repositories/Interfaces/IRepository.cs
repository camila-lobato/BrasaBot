using System.Linq.Expressions;

namespace BrasaBot.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> ObterTodosAsync();
        Task<T?> ObterPorIdAsync(int id);
        Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> predicate);
        Task AdicionarAsync(T entity);
        Task AtualizarAsync(T entity);
        Task ExcluirAsync(int id);
        Task<bool> ExisteAsync(int id);
    }
}