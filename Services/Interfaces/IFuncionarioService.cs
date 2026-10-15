using BrasaBot.Models;

namespace BrasaBot.Services.Interfaces
{
    public interface IFuncionarioService
    {
        Task<List<Funcionario>> ObterTodosAsync();
        Task<List<Funcionario>> BuscarAsync(string termo);
        Task<Funcionario?> ObterPorIdAsync(int id);
        Task<bool> CadastrarAsync(Funcionario funcionario);
        Task<bool> AtualizarAsync(Funcionario funcionario);
        Task<bool> ExcluirAsync(int id);
    }
}