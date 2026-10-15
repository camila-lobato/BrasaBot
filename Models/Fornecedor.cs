using System.ComponentModel.DataAnnotations;

namespace BrasaBot.Models
{
    public class Fornecedor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(18)]
        public string? CNPJ { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

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

        public virtual ICollection<Despesa> Despesas { get; set; } = new List<Despesa>();
    }
}