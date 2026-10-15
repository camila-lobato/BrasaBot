using BrasaBot.Models;

namespace BrasaBot.Services.Interfaces
{
    public interface IClienteService
    {
        Task<List<Cliente>> ObterTodosAsync();
        Task<List<Cliente>> BuscarAsync(string termo);
        Task<Cliente?> ObterPorIdAsync(int id);
        Task<bool> CadastrarAsync(Cliente cliente);
        Task<bool> AtualizarAsync(Cliente cliente);
        Task<bool> ExcluirAsync(int id);
    }
}