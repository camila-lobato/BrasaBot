using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using BrasaBot.Services.Interfaces;

namespace BrasaBot.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<List<Cliente>> ObterTodosAsync()
        {
            return await _clienteRepository.ObterTodosAsync();
        }

        public async Task<List<Cliente>> BuscarAsync(string termo)
        {
            var todos = await ObterTodosAsync();

            if (string.IsNullOrWhiteSpace(termo))
                return todos;

            termo = termo.ToLower();
            return todos.Where(c =>
                c.Nome.ToLower().Contains(termo) ||
                (c.Telefone != null && c.Telefone.Contains(termo))
            ).ToList();
        }

        public async Task<Cliente?> ObterPorIdAsync(int id)
        {
            return await _clienteRepository.ObterPorIdAsync(id);
        }

        public async Task<bool> CadastrarAsync(Cliente cliente)
        {
            if (!string.IsNullOrWhiteSpace(cliente.CPF) &&
                await _clienteRepository.ExisteCpfAsync(cliente.CPF))
                return false;

            await _clienteRepository.AdicionarAsync(cliente);
            return true;
        }

        public async Task<bool> AtualizarAsync(Cliente cliente)
        {
            await _clienteRepository.AtualizarAsync(cliente);
            return true;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            await _clienteRepository.ExcluirAsync(id);
            return true;
        }
    }
}