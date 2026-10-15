using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using BrasaBot.Services.Interfaces;

namespace BrasaBot.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IRepository<Categoria> _categoriaRepository;

        public CategoriaService(IRepository<Categoria> categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<List<Categoria>> ObterTodasAsync()
        {
            var todas = await _categoriaRepository.ObterTodosAsync();
            return todas.Where(c => c.Ativo).OrderBy(c => c.Nome).ToList();
        }

        public async Task<List<Categoria>> BuscarAsync(string termo)
        {
            var todas = await ObterTodasAsync();

            if (string.IsNullOrWhiteSpace(termo))
                return todas;

            termo = termo.ToLower();
            return todas.Where(c => c.Nome.ToLower().Contains(termo)).ToList();
        }

        public async Task<Categoria?> ObterPorIdAsync(int id)
        {
            return await _categoriaRepository.ObterPorIdAsync(id);
        }

        public async Task<bool> CadastrarAsync(Categoria categoria)
        {
            await _categoriaRepository.AdicionarAsync(categoria);
            return true;
        }

        public async Task<bool> AtualizarAsync(Categoria categoria)
        {
            await _categoriaRepository.AtualizarAsync(categoria);
            return true;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            await _categoriaRepository.ExcluirAsync(id);
            return true;
        }
    }
}