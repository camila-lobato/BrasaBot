using BrasaBot.Models;

namespace BrasaBot.Repositories.Interfaces
{
    public interface ICaixaRepository
    {
        Task<List<Caixa>> ObterTodosAsync();
        Task<Caixa?> ObterPorIdAsync(int id);
        Task<Caixa?> ObterCaixaAbertoAsync();
        Task<Caixa?> ObterCaixaPorNumeroAbertoAsync(int numeroCaixa);
        Task AdicionarAsync(Caixa caixa);
        Task AtualizarAsync(Caixa caixa);
    }
}