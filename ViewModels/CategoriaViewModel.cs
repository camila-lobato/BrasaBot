using System.ComponentModel.DataAnnotations;

namespace BrasaBot.ViewModels
{
    public class CategoriaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(50, ErrorMessage = "O Nome deve ter no máximo 50 caracteres.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;
    }
}