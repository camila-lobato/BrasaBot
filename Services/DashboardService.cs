using BrasaBot.ViewModels;
using BrasaBot.Services.Interfaces;

namespace BrasaBot.Services
{
    public class DashboardService : IDashboardService
    {
        public Task<DashboardViewModel> ObterDadosAsync()
        {
            var vm = new DashboardViewModel();

            // TODO: quando o módulo de Pedidos estiver implementado, preencher com dados reais
            // Por enquanto, deixa zerado

            // Meses dos últimos 6
            var hoje = DateTime.Now;
            for (int i = 5; i >= 0; i--)
            {
                var mes = hoje.AddMonths(-i);
                vm.MesesUltimos6[5 - i] = mes.ToString("MMM/yy");
                vm.VendasMensais[5 - i] = 0m;
            }

            return Task.FromResult(vm);
        }
    }
}