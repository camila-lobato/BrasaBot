using BrasaBot.Models;

namespace BrasaBot.Services.Interfaces
{
    public interface ICategoriaService
    {
        Task<List<Categoria>> ObterTodasAsync();
        Task<List<Categoria>> BuscarAsync(string termo);
        Task<Categoria?> ObterPorIdAsync(int id);
        Task<bool> CadastrarAsync(Categoria categoria);
        Task<bool> AtualizarAsync(Categoria categoria);
        Task<bool> ExcluirAsync(int id);
    }
}