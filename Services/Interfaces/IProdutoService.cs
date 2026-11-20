using BrasaBot.Models;
using Microsoft.AspNetCore.Http;

namespace BrasaBot.Services.Interfaces
{
    public interface IProdutoService
    {
        Task<List<Produto>> ObterTodosAsync();
        Task<List<Produto>> BuscarAsync(string termo);
        Task<Produto?> ObterPorIdAsync(int id);
        Task<bool> CadastrarAsync(Produto produto);
        Task<bool> AtualizarAsync(Produto produto);
        Task<bool> ExcluirAsync(int id);
        Task<string?> SalvarImagemAsync(IFormFile arquivo);
        Task<List<ProdutoInsumo>> ObterInsumosDoProdutoAsync(int produtoId);
        Task SalvarInsumosDoProdutoAsync(int produtoId, List<ProdutoInsumo> insumos);
    }
}