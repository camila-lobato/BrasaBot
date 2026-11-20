using BrasaBot.Models;
using BrasaBot.ViewModels;

namespace BrasaBot.Services.Interfaces
{
    public interface IEstoqueService
    {
        Task<EstoqueIndexViewModel> ObterIndexAsync();
        Task<bool> RegistrarMovimentacaoAsync(int insumoId, string tipo, decimal quantidade, string? observacao, int funcionarioId);
    }
}