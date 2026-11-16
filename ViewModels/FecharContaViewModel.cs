using BrasaBot.Models;

namespace BrasaBot.ViewModels
{
    public class FecharContaViewModel
    {
        public Pedido Pedido { get; set; } = null!;
        public List<ItemPedido> Itens { get; set; } = new();

        public decimal ValorPagoDinheiro { get; set; }
        public decimal ValorPagoCredito { get; set; }
        public decimal ValorPagoDebito { get; set; }
        public decimal ValorPagoPix { get; set; }

        public decimal Desconto { get; set; }
        public decimal TotalPedido { get; set; }
        public decimal TotalPago { get; set; }
        public decimal ValorRestante { get; set; }
        public decimal Troco { get; set; }
    }
}