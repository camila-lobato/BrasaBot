using BrasaBot.Models;

namespace BrasaBot.Repositories.Interfaces
{
    public interface IFuncionarioRepository
    {
        Task<List<Funcionario>> ObterTodosAsync();
        Task<Funcionario?> ObterPorIdAsync(int id);
        Task AdicionarAsync(Funcionario funcionario);
        Task AtualizarAsync(Funcionario funcionario);
        Task ExcluirAsync(int id);
        Task<bool> ExisteCpfAsync(string cpf);
    }
}