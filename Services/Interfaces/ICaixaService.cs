using BrasaBot.Models;
using BrasaBot.ViewModels;

namespace BrasaBot.Services.Interfaces
{
    public interface ICaixaService
    {
        Task<Caixa?> ObterCaixaAbertoAsync();
        Task<Caixa?> ObterPorIdAsync(int id);
        Task<List<MovimentacaoCaixa>> ObterMovimentacoesAsync(int caixaId);
        Task<Caixa> AbrirCaixaAsync(int numeroCaixa, decimal saldoInicial, string? observacao, int funcionarioId);
        Task InserirDinheiroAsync(int caixaId, decimal valor, string? observacao);
        Task RetirarDinheiroAsync(int caixaId, decimal valor, string? observacao);
        Task<bool> ExcluirMovimentacaoAsync(int movimentacaoId);
        Task FecharCaixaAsync(int caixaId, FechamentoCaixaViewModel vm);
        Task AtualizarTotaisAsync(int caixaId);
    }
}