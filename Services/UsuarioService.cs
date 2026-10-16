using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using BrasaBot.Services.Interfaces;

namespace BrasaBot.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IWebHostEnvironment _environment;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IFuncionarioRepository funcionarioRepository,
            IWebHostEnvironment environment)
        {
            _usuarioRepository = usuarioRepository;
            _funcionarioRepository = funcionarioRepository;
            _environment = environment;
        }

        public async Task<List<Usuario>> ObterTodosAsync()
        {
            return await _usuarioRepository.ObterTodosAsync();
        }

        public async Task<List<Usuario>> BuscarAsync(string termo)
        {
            var todos = await ObterTodosAsync();

            if (string.IsNullOrWhiteSpace(termo))
                return todos;

            termo = termo.ToLower();
            return todos.Where(u =>
                u.Funcionario.Nome.ToLower().Contains(termo) ||
                u.Email.ToLower().Contains(termo)
            ).ToList();
        }

        public async Task<Usuario?> ObterPorIdAsync(int id)
        {
            return await _usuarioRepository.ObterPorIdAsync(id);
        }

        public async Task<bool> CadastrarAsync(Usuario usuario)
        {
            if (await _usuarioRepository.ExisteEmailAsync(usuario.Email))
                return false;

            if (await _usuarioRepository.ExisteFuncionarioVinculadoAsync(usuario.FuncionarioId))
                return false;

            await _usuarioRepository.AdicionarAsync(usuario);
            return true;
        }

        public async Task<bool> AtualizarAsync(Usuario usuario)
        {
            if (await _usuarioRepository.ExisteFuncionarioVinculadoAsync(usuario.FuncionarioId, usuario.Id))
                return false;

            await _usuarioRepository.AtualizarAsync(usuario);
            return true;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            await _usuarioRepository.ExcluirAsync(id);
            return true;
        }

        public async Task<string?> SalvarImagemAsync(IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
                return null;

            var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png" };
            var extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();

            if (!extensoesPermitidas.Contains(extensao))
                throw new InvalidOperationException("Formato de imagem inválido. Use JPG, JPEG ou PNG.");

            if (arquivo.Length > 2 * 1024 * 1024)
                throw new InvalidOperationException("A imagem deve ter no máximo 2 MB.");

            var pastaUploads = Path.Combine(_environment.WebRootPath, "uploads", "usuarios");
            Directory.CreateDirectory(pastaUploads);

            var nomeArquivo = $"{Guid.NewGuid()}{extensao}";
            var caminhoCompleto = Path.Combine(pastaUploads, nomeArquivo);

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return $"/uploads/usuarios/{nomeArquivo}";
        }

        public async Task<List<Funcionario>> ObterFuncionariosSemUsuarioAsync(int? ignorarUsuarioId = null)
        {
            var funcionarios = await _funcionarioRepository.ObterTodosAsync();
            var usuarios = await _usuarioRepository.ObterTodosAsync();

            var idsVinculados = usuarios
                .Where(u => !ignorarUsuarioId.HasValue || u.Id != ignorarUsuarioId.Value)
                .Select(u => u.FuncionarioId)
                .ToHashSet();

            return funcionarios.Where(f => !idsVinculados.Contains(f.Id)).ToList();
        }
    }
}