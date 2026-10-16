using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BrasaBot.ViewModels
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        [StringLength(100)]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Nível de Acesso é obrigatório.")]
        [Display(Name = "Nível de Acesso")]
        public string NivelAcesso { get; set; } = string.Empty;

        [Display(Name = "Foto")]
        public IFormFile? ArquivoFoto { get; set; }

        public string? FotoAtual { get; set; }

        [Required(ErrorMessage = "O campo Funcionário é obrigatório.")]
        [Display(Name = "Funcionário")]
        public int FuncionarioId { get; set; }

        public bool Ativo { get; set; } = true;

        // Lista para combobox
        public IEnumerable<SelectListItem>? Funcionarios { get; set; }
    }
}