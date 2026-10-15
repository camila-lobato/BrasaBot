using System.ComponentModel.DataAnnotations;

namespace BrasaBot.Models
{
    public class ItemPedido
    {
        public int Id { get; set; }
        public int Quantidade { get; set; }

        public decimal ValorUnitario { get; set; }

        public decimal SubTotal { get; set; }

        public int PedidoId { get; set; }

        public int ProdutoId { get; set; }

        public virtual Pedido Pedido { get; set; } = null!;

        public virtual Produto Produto { get; set; } = null!;
    }
}