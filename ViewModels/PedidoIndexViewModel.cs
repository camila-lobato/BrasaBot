namespace BrasaBot.ViewModels
{
    public class PedidoIndexViewModel
    {
        public List<BrasaBot.Models.Mesa> MesasDisponiveis { get; set; } = new();
        public List<BrasaBot.Models.Mesa> MesasOcupadas { get; set; } = new();
        public List<BrasaBot.Models.Mesa> MesasEmFechamento { get; set; } = new();

        public List<BrasaBot.Models.Pedido> PedidosDeliveryProducao { get; set; } = new();
        public List<BrasaBot.Models.Pedido> PedidosDeliveryProntos { get; set; } = new();
        public List<BrasaBot.Models.Pedido> PedidosDeliveryEmEntrega { get; set; } = new();
    }
}