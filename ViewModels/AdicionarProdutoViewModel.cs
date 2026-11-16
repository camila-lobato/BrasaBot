using BrasaBot.Models;

namespace BrasaBot.ViewModels
{
    public class AdicionarProdutoViewModel
    {
        public int PedidoId { get; set; }
        public string TipoPedido { get; set; } = string.Empty;   // "Mesa", "Delivery", "Balcão"
        public int? MesaId { get; set; }

        public List<Produto> Produtos { get; set; } = new();
        public List<Categoria> Categorias { get; set; } = new();
        public List<ItemPedido> ItensAtuais { get; set; } = new();
        public decimal Total { get; set; }
    }
}