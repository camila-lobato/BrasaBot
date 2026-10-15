using BrasaBot.Models;

namespace BrasaBot.Repositories.Interfaces
{
    public interface IProdutoRepository
    {
        Task<List<Produto>> ObterTodosAsync();
        Task<Produto?> ObterPorIdAsync(int id);
        Task AdicionarAsync(Produto produto);
        Task AtualizarAsync(Produto produto);
        Task ExcluirAsync(int id);
    }
}