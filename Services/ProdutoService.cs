using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using BrasaBot.Services.Interfaces;

namespace BrasaBot.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IWebHostEnvironment _environment;

        public ProdutoService(IProdutoRepository produtoRepository, IWebHostEnvironment environment)
        {
            _produtoRepository = produtoRepository;
            _environment = environment;
        }

        public async Task<List<Produto>> ObterTodosAsync()
        {
            return await _produtoRepository.ObterTodosAsync();
        }

        public async Task<List<Produto>> BuscarAsync(string termo)
        {
            var todos = await ObterTodosAsync();

            if (string.IsNullOrWhiteSpace(termo))
                return todos;

            termo = termo.ToLower();
            return todos.Where(p =>
                p.Nome.ToLower().Contains(termo) ||
                (p.Categoria != null && p.Categoria.Nome.ToLower().Contains(termo))
            ).ToList();
        }

        public async Task<Produto?> ObterPorIdAsync(int id)
        {
            return await _produtoRepository.ObterPorIdAsync(id);
        }

        public async Task<bool> CadastrarAsync(Produto produto)
        {
            await _produtoRepository.AdicionarAsync(produto);
            return true;
        }

        public async Task<bool> AtualizarAsync(Produto produto)
        {
            await _produtoRepository.AtualizarAsync(produto);
            return true;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            await _produtoRepository.ExcluirAsync(id);
            return true;
        }

        // Faz upload da imagem para wwwroot/uploads/produtos
        public async Task<string?> SalvarImagemAsync(IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
                return null;

            // Validação de extensão
            var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png" };
            var extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();

            if (!extensoesPermitidas.Contains(extensao))
                throw new InvalidOperationException("Formato de imagem inválido. Use JPG, JPEG ou PNG.");

            // Validação de tamanho (2 MB = 2 * 1024 * 1024 bytes)
            if (arquivo.Length > 2 * 1024 * 1024)
                throw new InvalidOperationException("A imagem deve ter no máximo 2 MB.");

            // Caminho físico: wwwroot/uploads/produtos
            var pastaUploads = Path.Combine(_environment.WebRootPath, "uploads", "produtos");
            Directory.CreateDirectory(pastaUploads);

            // Nome único para evitar conflito
            var nomeArquivo = $"{Guid.NewGuid()}{extensao}";
            var caminhoCompleto = Path.Combine(pastaUploads, nomeArquivo);

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            // Caminho relativo que será salvo no banco (para uso no <img src="">)
            return $"/uploads/produtos/{nomeArquivo}";
        }
    }
}