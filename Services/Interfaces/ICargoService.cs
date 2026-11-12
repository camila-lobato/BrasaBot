using BrasaBot.Models;

namespace BrasaBot.Services.Interfaces
{
    public interface ICargoService
    {
        Task<List<Cargo>> ObterTodosAsync();
        Task<List<Cargo>> BuscarAsync(string termo);
        Task<Cargo?> ObterPorIdAsync(int id);
        Task<bool> CadastrarAsync(Cargo cargo);
        Task<bool> AtualizarAsync(Cargo cargo);
        Task<bool> ExcluirAsync(int id);
    }
}