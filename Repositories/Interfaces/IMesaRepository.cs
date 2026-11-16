using BrasaBot.Models;

namespace BrasaBot.Repositories.Interfaces
{
    public interface IMesaRepository
    {
        Task<List<Mesa>> ObterTodasAsync();
        Task<List<Mesa>> ObterPorStatusAsync(string status);
        Task<Mesa?> ObterPorIdAsync(int id);
        Task AtualizarAsync(Mesa mesa);
    }
}