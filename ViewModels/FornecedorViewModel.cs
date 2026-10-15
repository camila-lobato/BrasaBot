using System.ComponentModel.DataAnnotations;

namespace BrasaBot.ViewModels
{
    public class FornecedorViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100)]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(18)]
        [Display(Name = "CNPJ")]
        public string? CNPJ { get; set; }

        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

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