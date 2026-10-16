using BrasaBot.Models;

namespace BrasaBot.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> ObterTodosAsync();
        Task<List<Usuario>> BuscarAsync(string termo);
        Task<Usuario?> ObterPorIdAsync(int id);
        Task<bool> CadastrarAsync(Usuario usuario);
        Task<bool> AtualizarAsync(Usuario usuario);
        Task<bool> ExcluirAsync(int id);
        Task<string?> SalvarImagemAsync(IFormFile arquivo);

      Task<List<Funcionario>> ObterFuncionariosSemUsuarioAsync(int? ignorarUsuarioId = null);
    }
}