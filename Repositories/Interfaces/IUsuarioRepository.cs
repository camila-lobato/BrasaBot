using BrasaBot.Models;

namespace BrasaBot.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> ObterTodosAsync();
        Task<Usuario?> ObterPorIdAsync(int id);
        Task AdicionarAsync(Usuario usuario);
        Task AtualizarAsync(Usuario usuario);
        Task ExcluirAsync(int id);
        Task<bool> ExisteEmailAsync(string email);
        Task<bool> ExisteFuncionarioVinculadoAsync(int funcionarioId, int? ignorarUsuarioId = null);
    }
}