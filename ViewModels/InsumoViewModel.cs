using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BrasaBot.ViewModels
{
    public class InsumoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100)]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Display(Name = "Estoque Mínimo")]
        [Range(0, double.MaxValue, ErrorMessage = "O Estoque Mínimo não pode ser negativo.")]
        public decimal EstoqueMinimo { get; set; }

        [Required(ErrorMessage = "O campo Unidade de Medida é obrigatório.")]
        [StringLength(20)]
        [Display(Name = "Unidade de Medida")]
        public string UnidadeMedida { get; set; } = string.Empty;

        [Display(Name = "Quantidade")]
        [Range(0, double.MaxValue, ErrorMessage = "A Quantidade não pode ser negativa.")]
        public decimal EstoqueAtualQtd { get; set; }

        [Display(Name = "Fornecedor")]
        public int? FornecedorId { get; set; }

        // Lista para o combobox
        public IEnumerable<SelectListItem>? Fornecedores { get; set; }
    }
}