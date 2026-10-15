using System.ComponentModel.DataAnnotations;

namespace BrasaBot.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Tipo { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; }

        public decimal Desconto { get; set; }

        public decimal TaxaEntrega { get; set; } = 0m;

        public decimal Total { get; set; }

        public int ClienteId { get; set; }

        public int FuncionarioId { get; set; }

        public int? MesaId { get; set; }

        public virtual Cliente Cliente { get; set; } = null!;

        public virtual Funcionario Funcionario { get; set; } = null!;

        public virtual Mesa? Mesa { get; set; }

        public virtual ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
    }
}