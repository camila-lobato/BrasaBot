using System.ComponentModel.DataAnnotations;

namespace BrasaBot.ViewModels
{
    public class PerfilViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NivelAcesso { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public DateTime DataAdmissao { get; set; }
        public DateTime? UltimoAcesso { get; set; }
        public string? Foto { get; set; }
    }

    public class AlterarSenhaViewModel
    {
        [Required(ErrorMessage = "Informe a senha atual.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha Atual")]
        public string SenhaAtual { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a nova senha.")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "A nova senha deve ter no mínimo 6 caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Nova Senha")]
        public string NovaSenha { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirme a nova senha.")]
        [DataType(DataType.Password)]
        [Compare("NovaSenha", ErrorMessage = "As senhas não coincidem.")]
        [Display(Name = "Confirmar Nova Senha")]
        public string ConfirmarNovaSenha { get; set; } = string.Empty;
    }

    public class EditarPerfilViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100)]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Telefone é obrigatório.")]
        [StringLength(20)]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "O campo Logradouro é obrigatório.")]
        [StringLength(100)]
        [Display(Name = "Logradouro")]
        public string? Logradouro { get; set; }

        [StringLength(10)]
        [Display(Name = "Número")]
        public string? Numero { get; set; }

        [Required(ErrorMessage = "O campo Bairro é obrigatório.")]
        [StringLength(50)]
        [Display(Name = "Bairro")]
        public string? Bairro { get; set; }

        [StringLength(10)]
        [Display(Name = "CEP")]
        public string? CEP { get; set; }
    }
}