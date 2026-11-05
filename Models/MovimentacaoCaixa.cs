using System.ComponentModel.DataAnnotations;

namespace BrasaBot.Models
{
    public class MovimentacaoCaixa
    {
        public int Id { get; set; }

        public DateTime DataHora { get; set; } = DateTime.Now;

        public decimal Valor { get; set; }

        [Required]
        [StringLength(20)]
        public string Tipo { get; set; } = string.Empty; // "Entrada", "Saida"

        [Required]
        [StringLength(20)]
        public string FormaPagamento { get; set; } = string.Empty; // "Dinheiro", "Crédito", "Débito", "PIX"

        [StringLength(255)]
        public string? Observacao { get; set; }

        public int CaixaId { get; set; }
        public virtual Caixa Caixa { get; set; } = null!;

        // Vínculo opcional com pedido (para quando o módulo de Pedidos for implementado)
        public int? PedidoId { get; set; }
        public virtual Pedido? Pedido { get; set; }
    }
}