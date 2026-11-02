namespace BrasaBot.ViewModels
{
    public class DashboardViewModel
    {
        // Vendas por dia da semana (seg, ter, qua, qui, sex, sab, dom)
        public decimal[] VendasPorDia { get; set; } = new decimal[7];

        // Vendas dos últimos 6 meses
        public string[] MesesUltimos6 { get; set; } = new string[6];
        public decimal[] VendasMensais { get; set; } = new decimal[6];
    }
}