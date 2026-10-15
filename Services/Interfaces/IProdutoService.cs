using BrasaBot.Models;

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

        // Método auxiliar para salvar o arquivo de imagem
        Task<string?> SalvarImagemAsync(IFormFile arquivo);
    }
}