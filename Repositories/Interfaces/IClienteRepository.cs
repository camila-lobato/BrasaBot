using BrasaBot.Models;

namespace BrasaBot.Repositories.Interfaces
{
    public interface IClienteRepository
    {
        Task<List<Cliente>> ObterTodosAsync();
        Task<Cliente?> ObterPorIdAsync(int id);
        Task AdicionarAsync(Cliente cliente);
        Task AtualizarAsync(Cliente cliente);
        Task ExcluirAsync(int id);
        Task<bool> ExisteCpfAsync(string cpf);
    }
}