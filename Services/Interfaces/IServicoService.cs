using BrasaBot.Models;

namespace BrasaBot.Services.Interfaces
{
    public interface IServicoService
    {
        Task<List<Servico>> ObterTodosAsync();
        Task<List<Servico>> BuscarAsync(string termo);
        Task<Servico?> ObterPorIdAsync(int id);
        Task<bool> CadastrarAsync(Servico servico);
        Task<bool> AtualizarAsync(Servico servico);
        Task<bool> ExcluirAsync(int id);
    }
}