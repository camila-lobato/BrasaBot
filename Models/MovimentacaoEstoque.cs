using System.ComponentModel.DataAnnotations;

namespace BrasaBot.Models
{
    public class MovimentacaoEstoque
    {
        public int Id { get; set; }

        public DateTime DataHora { get; set; }

        [Required]
        [StringLength(20)]
        public string Tipo { get; set; } = string.Empty;

        public decimal Quantidade { get; set; }

        [StringLength(255)]
        public string? Observacao { get; set; }

        public int InsumoId { get; set; }

        public int FuncionarioId { get; set; }

        public virtual Insumo Insumo { get; set; } = null!;

        public virtual Funcionario Funcionario { get; set; } = null!;
    }
}