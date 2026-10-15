using System.ComponentModel.DataAnnotations;

namespace BrasaBot.Models
{
    public class Produto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        public decimal Valor { get; set; }

        [StringLength(255)]
        public string? Foto { get; set; }

        public bool Ativo { get; set; } = true;

        public int CategoriaId { get; set; }

        public virtual Categoria Categoria { get; set; } = null!;

        public virtual ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();

        public virtual ICollection<ProdutoInsumo> ProdutoInsumos { get; set; } = new List<ProdutoInsumo>();
    }
}