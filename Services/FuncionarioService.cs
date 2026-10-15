using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using BrasaBot.Services.Interfaces;

namespace BrasaBot.Services
{
    public class FuncionarioService : IFuncionarioService
    {
        private readonly IFuncionarioRepository _funcionarioRepository;

        public FuncionarioService(IFuncionarioRepository funcionarioRepository)
        {
            _funcionarioRepository = funcionarioRepository;
        }

        public async Task<List<Funcionario>> ObterTodosAsync()
        {
            return await _funcionarioRepository.ObterTodosAsync();
        }

        public async Task<List<Funcionario>> BuscarAsync(string termo)
        {
            var todos = await _funcionarioRepository.ObterTodosAsync();

            if (string.IsNullOrWhiteSpace(termo))
                return todos;

            termo = termo.ToLower();
            return todos.Where(f =>
                f.Nome.ToLower().Contains(termo) ||
                (f.Telefone != null && f.Telefone.Contains(termo))
            ).ToList();
        }

        public async Task<Funcionario?> ObterPorIdAsync(int id)
        {
            return await _funcionarioRepository.ObterPorIdAsync(id);
        }

        public async Task<bool> CadastrarAsync(Funcionario funcionario)
        {
            if (await _funcionarioRepository.ExisteCpfAsync(funcionario.CPF))
                return false;

            await _funcionarioRepository.AdicionarAsync(funcionario);
            return true;
        }

        public async Task<bool> AtualizarAsync(Funcionario funcionario)
        {
            await _funcionarioRepository.AtualizarAsync(funcionario);
            return true;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            await _funcionarioRepository.ExcluirAsync(id);
            return true;
        }
    }
}