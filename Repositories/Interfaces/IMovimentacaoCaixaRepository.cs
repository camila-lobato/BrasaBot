using BrasaBot.Models;

namespace BrasaBot.Repositories.Interfaces
{
    public interface IMovimentacaoCaixaRepository
    {
        Task<List<MovimentacaoCaixa>> ObterPorCaixaAsync(int caixaId);
        Task<MovimentacaoCaixa?> ObterPorIdAsync(int id);
        Task AdicionarAsync(MovimentacaoCaixa movimentacao);
        Task ExcluirAsync(int id);
    }
}