using System.ComponentModel.DataAnnotations;

namespace BrasaBot.Models
{
    public class Caixa
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 3, ErrorMessage = "O número do caixa deve ser entre 1 e 3.")]
        public int NumeroCaixa { get; set; }

        public decimal SaldoInicial { get; set; }

        public decimal TotalEntradas { get; set; }

        public decimal TotalSaidas { get; set; }

        public decimal Total { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Fechado"; // "Aberto" ou "Fechado"

        [StringLength(255)]
        public string? Observacao { get; set; }

        public DateTime DataAbertura { get; set; }

        public DateTime? DataFechamento { get; set; }

        public int FuncionarioId { get; set; }

        public virtual Funcionario Funcionario { get; set; } = null!;

        public virtual ICollection<MovimentacaoCaixa> MovimentacoesCaixa { get; set; } = new List<MovimentacaoCaixa>();
    }
}