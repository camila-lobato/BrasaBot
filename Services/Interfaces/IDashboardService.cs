using BrasaBot.ViewModels;

namespace BrasaBot.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> ObterDadosAsync();
    }
}