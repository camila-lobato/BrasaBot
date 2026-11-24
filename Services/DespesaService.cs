using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;

namespace BrasaBot.Services
{
    public class DespesaService : IDespesaService
    {
        private readonly IDespesaRepository _despesaRepository;
        private readonly ICaixaService _caixaService;

        public DespesaService(IDespesaRepository despesaRepository, ICaixaService caixaService)
        {
            _despesaRepository = despesaRepository;
            _caixaService = caixaService;
        }

        public async Task<DespesaIndexViewModel> ObterIndexAsync()
        {
            return new DespesaIndexViewModel
            {
                Despesas = await _despesaRepository.ObterTodosAsync(),
                TotalDespesas = await _despesaRepository.ObterTotalAsync(),
                TotalPendente = await _despesaRepository.ObterTotalPendenteAsync(),
                TotalPago = await _despesaRepository.ObterTotalPagoAsync()
            };
        }

        public async Task<Despesa?> ObterPorIdAsync(int id)
        {
            return await _despesaRepository.ObterPorIdAsync(id);
        }

        public async Task<bool> CadastrarAsync(Despesa despesa)
        {
            await _despesaRepository.AdicionarAsync(despesa);
            return true;
        }

        public async Task<bool> PagarAsync(int despesaId, PagarDespesaViewModel vm)
        {
            var despesa = await _despesaRepository.ObterPorIdAsync(despesaId);
            if (despesa == null || despesa.Status == "Pago")
                return false;

            // Atualiza o status
            despesa.Status = "Pago";
            despesa.FormaPagamento = vm.FormaPagamento;
            await _despesaRepository.AtualizarAsync(despesa);

            // Registra saída no caixa (obrigatoriamente, pois o Controller já validou)
            await _caixaService.RegistrarMovimentacaoAsync(
                vm.CaixaId,
                despesa.Valor,
                "Saida",
                vm.FormaPagamento,
                $"Despesa: {despesa.Descricao}");

            return true;
        }
    }
}