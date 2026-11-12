using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using BrasaBot.Services.Interfaces;

namespace BrasaBot.Services
{
    public class CargoService : ICargoService
    {
        private readonly IRepository<Cargo> _cargoRepository;

        public CargoService(IRepository<Cargo> cargoRepository)
        {
            _cargoRepository = cargoRepository;
        }

        public async Task<List<Cargo>> ObterTodosAsync()
        {
            var todos = await _cargoRepository.ObterTodosAsync();
            return todos.Where(c => c.Ativo).OrderBy(c => c.Nome).ToList();
        }

        public async Task<List<Cargo>> BuscarAsync(string termo)
        {
            var todos = await ObterTodosAsync();

            if (string.IsNullOrWhiteSpace(termo))
                return todos;

            termo = termo.ToLower();
            return todos.Where(c => c.Nome.ToLower().Contains(termo)).ToList();
        }

        public async Task<Cargo?> ObterPorIdAsync(int id)
        {
            return await _cargoRepository.ObterPorIdAsync(id);
        }

        public async Task<bool> CadastrarAsync(Cargo cargo)
        {
            await _cargoRepository.AdicionarAsync(cargo);
            return true;
        }

        public async Task<bool> AtualizarAsync(Cargo cargo)
        {
            await _cargoRepository.AtualizarAsync(cargo);
            return true;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            await _cargoRepository.ExcluirAsync(id);
            return true;
        }
    }
}