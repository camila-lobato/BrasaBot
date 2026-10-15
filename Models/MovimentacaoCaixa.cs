using System.ComponentModel.DataAnnotations;

namespace BrasaBot.Models
{
    public class MovimentacaoCaixa
    {
        public int Id { get; set; }

        public DateTime DataHora { get; set; }

        public decimal Valor { get; set; }

        [Required]
        [StringLength(20)]
        public string Tipo { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string FormaPagamento { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Observacao { get; set; }

        public int CaixaId { get; set; }
        public virtual Caixa Caixa { get; set; } = null!;
    }
}