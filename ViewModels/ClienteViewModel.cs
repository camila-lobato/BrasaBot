using System.ComponentModel.DataAnnotations;

namespace BrasaBot.ViewModels
{
    public class ClienteViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Nome deve ter no máximo 100 caracteres.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(14)]
        [Display(Name = "CPF")]
        public string? CPF { get; set; }

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

        public bool Ativo { get; set; } = true;
    }
}