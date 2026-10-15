using System.ComponentModel.DataAnnotations;

namespace BrasaBot.Models
{
    public class Funcionario
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(14)]
        public string CPF { get; set; } = string.Empty;

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

        [Required]
        [StringLength(50)]
        public string Cargo { get; set; } = string.Empty;

        [Required]
        public DateTime DataAdmissao { get; set; }

        public bool Ativo { get; set; } = true;

        public virtual Usuario? Usuario { get; set; }

        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

        public virtual ICollection<Caixa> Caixas { get; set; } = new List<Caixa>();

        public virtual ICollection<MovimentacaoEstoque> MovimentacoesEstoque { get; set; } = new List<MovimentacaoEstoque>();
    }
}