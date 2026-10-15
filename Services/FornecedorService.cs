using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using BrasaBot.Services.Interfaces;

namespace BrasaBot.Services
{
    public class FornecedorService : IFornecedorService
    {
        private readonly IFornecedorRepository _fornecedorRepository;

        public FornecedorService(IFornecedorRepository fornecedorRepository)
        {
            _fornecedorRepository = fornecedorRepository;
        }

        public async Task<List<Fornecedor>> ObterTodosAsync()
        {
            return await _fornecedorRepository.ObterTodosAsync();
        }

        public async Task<List<Fornecedor>> BuscarAsync(string termo)
        {
            var todos = await ObterTodosAsync();

            if (string.IsNullOrWhiteSpace(termo))
                return todos;

            termo = termo.ToLower();
            return todos.Where(f =>
                f.Nome.ToLower().Contains(termo) ||
                (f.CNPJ != null && f.CNPJ.Contains(termo)) ||
                (f.Email != null && f.Email.ToLower().Contains(termo))
            ).ToList();
        }

        public async Task<Fornecedor?> ObterPorIdAsync(int id)
        {
            return await _fornecedorRepository.ObterPorIdAsync(id);
        }

        public async Task<bool> CadastrarAsync(Fornecedor fornecedor)
        {
            if (!string.IsNullOrWhiteSpace(fornecedor.CNPJ) &&
                await _fornecedorRepository.ExisteCnpjAsync(fornecedor.CNPJ))
                return false;

            await _fornecedorRepository.AdicionarAsync(fornecedor);
            return true;
        }

        public async Task<bool> AtualizarAsync(Fornecedor fornecedor)
        {
            await _fornecedorRepository.AtualizarAsync(fornecedor);
            return true;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            await _fornecedorRepository.ExcluirAsync(id);
            return true;
        }
    }
}