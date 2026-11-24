using BrasaBot.ViewModels;

namespace BrasaBot.Services.Interfaces
{
    public interface IRelatorioService
    {
        Task<RelatorioIndexViewModel> ObterIndexAsync();
        Task<RelatorioVendasViewModel> ObterRelatorioVendasAsync(DateTime dataInicial, DateTime dataFinal, string filtro, int usuarioId);
        Task<RelatorioCaixaViewModel> ObterRelatorioCaixaAsync(DateTime dataInicial, DateTime dataFinal, string filtro, int usuarioId);
        Task<RelatorioDespesasViewModel> ObterRelatorioDespesasAsync(DateTime dataInicial, DateTime dataFinal, string filtro, int usuarioId);
        Task<RelatorioFinanceiroViewModel> ObterRelatorioFinanceiroAsync(DateTime dataInicial, DateTime dataFinal, int usuarioId);
        Task<RelatorioProdutosViewModel> ObterRelatorioProdutosAsync(DateTime dataInicial, DateTime dataFinal, string filtro, int usuarioId);
        Task<RelatorioEstoqueViewModel> ObterRelatorioEstoqueAsync(string filtro, int usuarioId);
    }
}