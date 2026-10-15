using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using BrasaBot.Services.Interfaces;

namespace BrasaBot.Services
{
    public class InsumoService : IInsumoService
    {
        private readonly IInsumoRepository _insumoRepository;

        public InsumoService(IInsumoRepository insumoRepository)
        {
            _insumoRepository = insumoRepository;
        }

        public async Task<List<Insumo>> ObterTodosAsync()
        {
            return await _insumoRepository.ObterTodosAsync();
        }

        public async Task<List<Insumo>> BuscarAsync(string termo)
        {
            var todos = await ObterTodosAsync();

            if (string.IsNullOrWhiteSpace(termo))
                return todos;

            termo = termo.ToLower();
            return todos.Where(i =>
                i.Nome.ToLower().Contains(termo) ||
                (i.Fornecedor != null && i.Fornecedor.Nome.ToLower().Contains(termo))
            ).ToList();
        }

        public async Task<Insumo?> ObterPorIdAsync(int id)
        {
            return await _insumoRepository.ObterPorIdAsync(id);
        }

        public async Task<bool> CadastrarAsync(Insumo insumo)
        {
            await _insumoRepository.AdicionarAsync(insumo);
            return true;
        }

        public async Task<bool> AtualizarAsync(Insumo insumo)
        {
            await _insumoRepository.AtualizarAsync(insumo);
            return true;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            await _insumoRepository.ExcluirAsync(id);
            return true;
        }
    }
}