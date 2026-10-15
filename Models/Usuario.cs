using System.ComponentModel.DataAnnotations;

namespace BrasaBot.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Senha { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string NivelAcesso { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Foto { get; set; }

        public DateTime? UltimoAcesso { get; set; }

        public bool Ativo { get; set; } = true;

        public int FuncionarioId { get; set; }

        public virtual Funcionario Funcionario { get; set; } = null!;
    }
}