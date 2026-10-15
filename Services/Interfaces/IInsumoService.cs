using BrasaBot.Models;

namespace BrasaBot.Services.Interfaces
{
    public interface IInsumoService
    {
        Task<List<Insumo>> ObterTodosAsync();
        Task<List<Insumo>> BuscarAsync(string termo);
        Task<Insumo?> ObterPorIdAsync(int id);
        Task<bool> CadastrarAsync(Insumo insumo);
        Task<bool> AtualizarAsync(Insumo insumo);
        Task<bool> ExcluirAsync(int id);
    }
}