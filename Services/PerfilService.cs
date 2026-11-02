using BCrypt.Net;
using BrasaBot.Models;
using BrasaBot.Repositories.Interfaces;
using BrasaBot.Services.Interfaces;
using BrasaBot.ViewModels;

namespace BrasaBot.Services
{
    public class PerfilService : IPerfilService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IFuncionarioRepository _funcionarioRepository;

        public PerfilService(IUsuarioRepository usuarioRepository, IFuncionarioRepository funcionarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _funcionarioRepository = funcionarioRepository;
        }

        public async Task<PerfilViewModel?> ObterPerfilAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
            if (usuario == null)
                return null;

            return new PerfilViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Funcionario.Nome,
                Email = usuario.Email,
                NivelAcesso = usuario.NivelAcesso,
                Cargo = usuario.Funcionario.Cargo,
                Telefone = usuario.Funcionario.Telefone ?? string.Empty,
                DataAdmissao = usuario.Funcionario.DataAdmissao,
                UltimoAcesso = usuario.UltimoAcesso,
                Foto = usuario.Foto
            };
        }

        public async Task<bool> AlterarSenhaAsync(int usuarioId, string senhaAtual, string novaSenha)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
            if (usuario == null)
                return false;

            // Valida senha atual
            if (!BCrypt.Net.BCrypt.Verify(senhaAtual, usuario.Senha))
                return false;

            // Aplica hash na nova
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(novaSenha);
            await _usuarioRepository.AtualizarAsync(usuario);
            return true;
        }

        public async Task<EditarPerfilViewModel?> ObterParaEdicaoAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
            if (usuario == null)
                return null;

            return new EditarPerfilViewModel
            {
                Id = usuario.FuncionarioId,
                Nome = usuario.Funcionario.Nome,
                Telefone = usuario.Funcionario.Telefone,
                Logradouro = usuario.Funcionario.Logradouro,
                Numero = usuario.Funcionario.Numero,
                Bairro = usuario.Funcionario.Bairro,
                CEP = usuario.Funcionario.CEP
            };
        }

        public async Task<bool> EditarPerfilAsync(int usuarioId, EditarPerfilViewModel vm)
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(usuarioId);
            if (usuario == null)
                return false;

            var funcionario = await _funcionarioRepository.ObterPorIdAsync(usuario.FuncionarioId);
            if (funcionario == null)
                return false;

            funcionario.Nome = vm.Nome;
            funcionario.Telefone = vm.Telefone;
            funcionario.Logradouro = vm.Logradouro;
            funcionario.Numero = vm.Numero;
            funcionario.Bairro = vm.Bairro;
            funcionario.CEP = vm.CEP;

            await _funcionarioRepository.AtualizarAsync(funcionario);
            return true;
        }
    }
}