using BrasaBot.Models;

namespace BrasaBot.Repositories.Interfaces
{
    public interface IFornecedorRepository
    {
        Task<List<Fornecedor>> ObterTodosAsync();
        Task<Fornecedor?> ObterPorIdAsync(int id);
        Task AdicionarAsync(Fornecedor fornecedor);
        Task AtualizarAsync(Fornecedor fornecedor);
        Task ExcluirAsync(int id);
        Task<bool> ExisteCnpjAsync(string cnpj);
    }
}