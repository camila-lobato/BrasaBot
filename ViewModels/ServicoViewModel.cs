using System.ComponentModel.DataAnnotations;

namespace BrasaBot.ViewModels
{
    public class ServicoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Nome deve ter no máximo 100 caracteres.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Valor é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O Valor deve ser maior que zero.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Valor")]
        public decimal Valor { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;
    }
}