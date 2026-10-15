using BrasaBot.Models;

namespace BrasaBot.Services.Interfaces
{
    public interface IFornecedorService
    {
        Task<List<Fornecedor>> ObterTodosAsync();
        Task<List<Fornecedor>> BuscarAsync(string termo);
        Task<Fornecedor?> ObterPorIdAsync(int id);
        Task<bool> CadastrarAsync(Fornecedor fornecedor);
        Task<bool> AtualizarAsync(Fornecedor fornecedor);
        Task<bool> ExcluirAsync(int id);
    }
}