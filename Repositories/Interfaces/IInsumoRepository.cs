using BrasaBot.Models;

namespace BrasaBot.Repositories.Interfaces
{
    public interface IInsumoRepository
    {
        Task<List<Insumo>> ObterTodosAsync();
        Task<Insumo?> ObterPorIdAsync(int id);
        Task AdicionarAsync(Insumo insumo);
        Task AtualizarAsync(Insumo insumo);
        Task ExcluirAsync(int id);
    }
}