using BrasaBot.Models;

namespace BrasaBot.Repositories.Interfaces
{
    public interface IDespesaRepository
    {
        Task<List<Despesa>> ObterTodosAsync();
        Task<Despesa?> ObterPorIdAsync(int id);
        Task AdicionarAsync(Despesa despesa);
        Task AtualizarAsync(Despesa despesa);
        Task<decimal> ObterTotalAsync();
        Task<decimal> ObterTotalPendenteAsync();
        Task<decimal> ObterTotalPagoAsync();
    }
}