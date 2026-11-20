using BrasaBot.Models;

namespace BrasaBot.Repositories.Interfaces
{
    public interface IMovimentacaoEstoqueRepository
    {
        Task<List<MovimentacaoEstoque>> ObterUltimasAsync(int limite = 50);
        Task<List<MovimentacaoEstoque>> ObterPorInsumoAsync(int insumoId);
        Task<MovimentacaoEstoque?> ObterUltimaPorInsumoAsync(int insumoId);
        Task AdicionarAsync(MovimentacaoEstoque movimentacao);
    }
}