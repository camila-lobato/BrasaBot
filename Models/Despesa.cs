using System.ComponentModel.DataAnnotations;

namespace BrasaBot.Models
{
    public class Despesa
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Descricao { get; set; } = string.Empty;

        public decimal Valor { get; set; }

        public DateTime Vencimento { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        [StringLength(20)]
        public string? FormaPagamento { get; set; }

        [StringLength(255)]
        public string? Observacao { get; set; }

        public int FornecedorId { get; set; }

        public virtual Fornecedor Fornecedor { get; set; } = null!;
    }
}