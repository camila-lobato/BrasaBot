using System.ComponentModel.DataAnnotations;

namespace BrasaBot.Models
{
    public class Insumo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        public decimal EstoqueAtualQtd { get; set; }

        public decimal EstoqueMinimo { get; set; }

        [Required]
        [StringLength(20)]
        public string UnidadeMedida { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;

        public int? FornecedorId { get; set; }

        public virtual Fornecedor? Fornecedor { get; set; }

        public virtual ICollection<ProdutoInsumo> ProdutoInsumos { get; set; } = new List<ProdutoInsumo>();

        public virtual ICollection<MovimentacaoEstoque> MovimentacoesEstoque { get; set; } = new List<MovimentacaoEstoque>();
    }
}