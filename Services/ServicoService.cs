using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using BrasaBot.Services.Interfaces;

namespace BrasaBot.Services
{
    public class ServicoService : IServicoService
    {
        private readonly IRepository<Servico> _servicoRepository;

        public ServicoService(IRepository<Servico> servicoRepository)
        {
            _servicoRepository = servicoRepository;
        }

        public async Task<List<Servico>> ObterTodosAsync()
        {
            var todos = await _servicoRepository.ObterTodosAsync();
            return todos.Where(s => s.Ativo).OrderBy(s => s.Nome).ToList();
        }

        public async Task<List<Servico>> BuscarAsync(string termo)
        {
            var todos = await ObterTodosAsync();

            if (string.IsNullOrWhiteSpace(termo))
                return todos;

            termo = termo.ToLower();
            return todos.Where(s => s.Nome.ToLower().Contains(termo)).ToList();
        }

        public async Task<Servico?> ObterPorIdAsync(int id)
        {
            return await _servicoRepository.ObterPorIdAsync(id);
        }

        public async Task<bool> CadastrarAsync(Servico servico)
        {
            await _servicoRepository.AdicionarAsync(servico);
            return true;
        }

        public async Task<bool> AtualizarAsync(Servico servico)
        {
            await _servicoRepository.AtualizarAsync(servico);
            return true;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            await _servicoRepository.ExcluirAsync(id);
            return true;
        }
    }
}