using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;

namespace BrasaBot.Services
{
    public class CaixaService : ICaixaService
    {
        private readonly ICaixaRepository _caixaRepository;
        private readonly IMovimentacaoCaixaRepository _movimentacaoRepository;

        public CaixaService(
            ICaixaRepository caixaRepository,
            IMovimentacaoCaixaRepository movimentacaoRepository)
        {
            _caixaRepository = caixaRepository;
            _movimentacaoRepository = movimentacaoRepository;
        }

        public async Task<Caixa?> ObterCaixaAbertoAsync()
        {
            return await _caixaRepository.ObterCaixaAbertoAsync();
        }

        public async Task<Caixa?> ObterPorIdAsync(int id)
        {
            return await _caixaRepository.ObterPorIdAsync(id);
        }

        public async Task<List<MovimentacaoCaixa>> ObterMovimentacoesAsync(int caixaId)
        {
            return await _movimentacaoRepository.ObterPorCaixaAsync(caixaId);
        }

        public async Task<Caixa> AbrirCaixaAsync(int numeroCaixa, decimal saldoInicial, string? observacao, int funcionarioId)
        {
            // Verifica se o caixa específico já está aberto
            var existente = await _caixaRepository.ObterCaixaPorNumeroAbertoAsync(numeroCaixa);
            if (existente != null)
                throw new InvalidOperationException($"O Caixa {numeroCaixa:00} já está aberto.");

            var caixa = new Caixa
            {
                NumeroCaixa = numeroCaixa,
                SaldoInicial = saldoInicial,
                TotalEntradas = 0,
                TotalSaidas = 0,
                Total = saldoInicial,
                Status = "Aberto",
                Observacao = observacao,
                DataAbertura = DateTime.Now,
                FuncionarioId = funcionarioId
            };

            await _caixaRepository.AdicionarAsync(caixa);
            return caixa;
        }

        public async Task InserirDinheiroAsync(int caixaId, decimal valor, string? observacao)
        {
            var caixa = await _caixaRepository.ObterPorIdAsync(caixaId);
            if (caixa == null || caixa.Status != "Aberto")
                throw new InvalidOperationException("O caixa não está aberto.");

            var mov = new MovimentacaoCaixa
            {
                CaixaId = caixaId,
                DataHora = DateTime.Now,
                Valor = valor,
                Tipo = "Entrada",
                FormaPagamento = "Dinheiro",
                Observacao = observacao
            };

            await _movimentacaoRepository.AdicionarAsync(mov);
            await AtualizarTotaisAsync(caixaId);
        }

        public async Task RetirarDinheiroAsync(int caixaId, decimal valor, string? observacao)
        {
            var caixa = await _caixaRepository.ObterPorIdAsync(caixaId);
            if (caixa == null || caixa.Status != "Aberto")
                throw new InvalidOperationException("O caixa não está aberto.");

            var mov = new MovimentacaoCaixa
            {
                CaixaId = caixaId,
                DataHora = DateTime.Now,
                Valor = valor,
                Tipo = "Saida",
                FormaPagamento = "Dinheiro",
                Observacao = observacao
            };

            await _movimentacaoRepository.AdicionarAsync(mov);
            await AtualizarTotaisAsync(caixaId);
        }

        public async Task<bool> ExcluirMovimentacaoAsync(int movimentacaoId)
        {
            var mov = await _movimentacaoRepository.ObterPorIdAsync(movimentacaoId);
            if (mov == null)
                return false;

            var caixaId = mov.CaixaId;
            await _movimentacaoRepository.ExcluirAsync(movimentacaoId);
            await AtualizarTotaisAsync(caixaId);
            return true;
        }

        public async Task FecharCaixaAsync(int caixaId, FechamentoCaixaViewModel vm)
        {
            var caixa = await _caixaRepository.ObterPorIdAsync(caixaId);
            if (caixa == null)
                throw new InvalidOperationException("Caixa não encontrado.");

            // Ajustar o Total considerando a conferência do operador
            // O valor real é a soma dos valores conferidos por forma de pagamento
            // (Simplicidade: registrar nas observações)
            var obsFechamento = $"Fechamento - Dinheiro: {vm.ValorDinheiro:C}, Crédito: {vm.ValorCredito:C}, Débito: {vm.ValorDebito:C}, PIX: {vm.ValorPix:C}";
            if (!string.IsNullOrWhiteSpace(vm.Observacao))
                obsFechamento += $" | {vm.Observacao}";

            caixa.Status = "Fechado";
            caixa.DataFechamento = DateTime.Now;
            caixa.Observacao = (caixa.Observacao ?? "") + " | " + obsFechamento;

            await _caixaRepository.AtualizarAsync(caixa);
        }

        public async Task AtualizarTotaisAsync(int caixaId)
        {
            var caixa = await _caixaRepository.ObterPorIdAsync(caixaId);
            if (caixa == null)
                return;

            var movs = await _movimentacaoRepository.ObterPorCaixaAsync(caixaId);

            caixa.TotalEntradas = movs.Where(m => m.Tipo == "Entrada").Sum(m => m.Valor);
            caixa.TotalSaidas = movs.Where(m => m.Tipo == "Saida").Sum(m => m.Valor);
            caixa.Total = caixa.SaldoInicial + caixa.TotalEntradas - caixa.TotalSaidas;

            await _caixaRepository.AtualizarAsync(caixa);
        }
    }
}