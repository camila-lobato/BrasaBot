using BrasaBot.ViewModels;

namespace BrasaBot.Services.Interfaces
{
    public interface IPerfilService
    {
        Task<PerfilViewModel?> ObterPerfilAsync(int usuarioId);
        Task<bool> AlterarSenhaAsync(int usuarioId, string senhaAtual, string novaSenha);
        Task<bool> EditarPerfilAsync(int usuarioId, EditarPerfilViewModel vm);
        Task<EditarPerfilViewModel?> ObterParaEdicaoAsync(int usuarioId);
    }
}