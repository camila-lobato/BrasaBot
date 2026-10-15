using System.ComponentModel.DataAnnotations;

namespace BrasaBot.ViewModels
{
    public class FuncionarioViewModel
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

        [Required(ErrorMessage = "O campo Cargo é obrigatório.")]
        [StringLength(50)]
        [Display(Name = "Cargo")]
        public string Cargo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Data de Admissão é obrigatório.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Admissão")]
        public DateTime DataAdmissao { get; set; } = DateTime.Today;

        public bool Ativo { get; set; } = true;
    }
}