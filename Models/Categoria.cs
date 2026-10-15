using System.ComponentModel.DataAnnotations;

namespace BrasaBot.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;

        public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}