using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;

namespace BrasaBot.Services
{
    public class RelatorioService : IRelatorioService
    {
        private readonly IRelatorioRepository _relatorioRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public RelatorioService(
            IRelatorioRepository relatorioRepository,
            IUsuarioRepository usuarioRepository)
        {
            _relatorioRepository = relatorioRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<RelatorioIndexViewModel> ObterIndexAsync()
        {
            return new RelatorioIndexViewModel
            {
                Historico = await _relatorioRepository.ObterHistoricoAsync(10),
                TiposRelatorio = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
                {
                    new() { Value = "Vendas", Text = "Relatório de Vendas" },
                    new() { Value = "Caixa", Text = "Relatório de Caixa" },
                    new() { Value = "Despesas", Text = "Relatório de Despesas" },
                    new() { Value = "Financeiro", Text = "Relatório Financeiro" },
                    new() { Value = "Produtos", Text = "Relatório de Produtos" },
                    new() { Value = "Estoque", Text = "Relatório de Estoque" }
                }
            };
        }

        public async Task<RelatorioVendasViewModel> ObterRelatorioVendasAsync(DateTime dataInicial, DateTime dataFinal, string filtro, int usuarioId)
        {
            var relatorio = await _relatorioRepository.ObterRelatorioVendasAsync(dataInicial, dataFinal, filtro);

            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
            relatorio.UsuarioEmissor = usuario?.Funcionario?.Nome ?? "Sistema";

            // Registra no histórico
            await _relatorioRepository.AdicionarHistoricoAsync(new RelatorioEmitido
            {
                TipoRelatorio = "Vendas",
                Filtro = filtro,
                DataInicial = dataInicial,
                DataFinal = dataFinal,
                DataEmissao = DateTime.Now,
                UsuarioId = usuarioId
            });

            return relatorio;
        }
        public async Task<RelatorioCaixaViewModel> ObterRelatorioCaixaAsync(DateTime dataInicial, DateTime dataFinal, string filtro, int usuarioId)
        {
            var relatorio = await _relatorioRepository.ObterRelatorioCaixaAsync(dataInicial, dataFinal, filtro);
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
            relatorio.UsuarioEmissor = usuario?.Funcionario?.Nome ?? "Sistema";

            await _relatorioRepository.AdicionarHistoricoAsync(new RelatorioEmitido
            {
                TipoRelatorio = "Caixa",
                Filtro = filtro,
                DataInicial = dataInicial,
                DataFinal = dataFinal,
                DataEmissao = DateTime.Now,
                UsuarioId = usuarioId
            });

            return relatorio;
        }

        public async Task<RelatorioDespesasViewModel> ObterRelatorioDespesasAsync(DateTime dataInicial, DateTime dataFinal, string filtro, int usuarioId)
        {
            var relatorio = await _relatorioRepository.ObterRelatorioDespesasAsync(dataInicial, dataFinal, filtro);
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
            relatorio.UsuarioEmissor = usuario?.Funcionario?.Nome ?? "Sistema";

            await _relatorioRepository.AdicionarHistoricoAsync(new RelatorioEmitido
            {
                TipoRelatorio = "Despesas",
                Filtro = filtro,
                DataInicial = dataInicial,
                DataFinal = dataFinal,
                DataEmissao = DateTime.Now,
                UsuarioId = usuarioId
            });

            return relatorio;
        }

        public async Task<RelatorioFinanceiroViewModel> ObterRelatorioFinanceiroAsync(DateTime dataInicial, DateTime dataFinal, int usuarioId)
        {
            var relatorio = await _relatorioRepository.ObterRelatorioFinanceiroAsync(dataInicial, dataFinal);
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
            relatorio.UsuarioEmissor = usuario?.Funcionario?.Nome ?? "Sistema";

            await _relatorioRepository.AdicionarHistoricoAsync(new RelatorioEmitido
            {
                TipoRelatorio = "Financeiro",
                Filtro = "Todos",
                DataInicial = dataInicial,
                DataFinal = dataFinal,
                DataEmissao = DateTime.Now,
                UsuarioId = usuarioId
            });

            return relatorio;
        }
        public async Task<RelatorioProdutosViewModel> ObterRelatorioProdutosAsync(DateTime dataInicial, DateTime dataFinal, string filtro, int usuarioId)
        {
            var relatorio = await _relatorioRepository.ObterRelatorioProdutosAsync(dataInicial, dataFinal, filtro);
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
            relatorio.UsuarioEmissor = usuario?.Funcionario?.Nome ?? "Sistema";

            await _relatorioRepository.AdicionarHistoricoAsync(new RelatorioEmitido
            {
                TipoRelatorio = "Produtos",
                Filtro = filtro,
                DataInicial = dataInicial,
                DataFinal = dataFinal,
                DataEmissao = DateTime.Now,
                UsuarioId = usuarioId
            });

            return relatorio;
        }

        public async Task<RelatorioEstoqueViewModel> ObterRelatorioEstoqueAsync(string filtro, int usuarioId)
        {
            var relatorio = await _relatorioRepository.ObterRelatorioEstoqueAsync(filtro);
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
            relatorio.UsuarioEmissor = usuario?.Funcionario?.Nome ?? "Sistema";

            await _relatorioRepository.AdicionarHistoricoAsync(new RelatorioEmitido
            {
                TipoRelatorio = "Estoque",
                Filtro = filtro,
                DataInicial = DateTime.Today.AddYears(-1),
                DataFinal = DateTime.Today,
                DataEmissao = DateTime.Now,
                UsuarioId = usuarioId
            });

            return relatorio;
        }
    }
}