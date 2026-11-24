using BrasaBot.Models;
using BrasaBot.ViewModels;

namespace BrasaBot.Services.Interfaces
{
    public interface IDespesaService
    {
        Task<DespesaIndexViewModel> ObterIndexAsync();
        Task<Despesa?> ObterPorIdAsync(int id);
        Task<bool> CadastrarAsync(Despesa despesa);
        Task<bool> PagarAsync(int despesaId, PagarDespesaViewModel vm);
    }
}