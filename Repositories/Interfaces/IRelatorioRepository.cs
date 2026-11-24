using BrasaBot.Models;
using BrasaBot.ViewModels;

namespace BrasaBot.Repositories.Interfaces
{
    public interface IRelatorioRepository
    {
        Task<List<RelatorioEmitido>> ObterHistoricoAsync(int limite = 10);
        Task AdicionarHistoricoAsync(RelatorioEmitido relatorio);
        Task<RelatorioVendasViewModel> ObterRelatorioVendasAsync(DateTime dataInicial, DateTime dataFinal, string filtro);
        Task<RelatorioCaixaViewModel> ObterRelatorioCaixaAsync(DateTime dataInicial, DateTime dataFinal, string filtro);
        Task<RelatorioDespesasViewModel> ObterRelatorioDespesasAsync(DateTime dataInicial, DateTime dataFinal, string filtro);
        Task<RelatorioFinanceiroViewModel> ObterRelatorioFinanceiroAsync(DateTime dataInicial, DateTime dataFinal);
        Task<RelatorioProdutosViewModel> ObterRelatorioProdutosAsync(DateTime dataInicial, DateTime dataFinal, string filtro);
        Task<RelatorioEstoqueViewModel> ObterRelatorioEstoqueAsync(string filtro);
    }
}