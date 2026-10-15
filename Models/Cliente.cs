using System.ComponentModel.DataAnnotations;

namespace BrasaBot.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(14)]
        public string? CPF { get; set; }

        [StringLength(20)]
        public string? Telefone { get; set; }

        [StringLength(100)]
        public string? Logradouro { get; set; }

        [StringLength(10)]
        public string? Numero { get; set; }

        [StringLength(50)]
        public string? Bairro { get; set; }

        [StringLength(10)]
        public string? CEP { get; set; }

        public bool Ativo { get; set; } = true;

        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}