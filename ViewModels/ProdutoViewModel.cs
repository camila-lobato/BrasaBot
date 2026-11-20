using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BrasaBot.ViewModels
{
    public class ProdutoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100)]
        [Display(Name = "Nome")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Valor é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O Valor deve ser maior que zero.")]
        [Display(Name = "Valor")]
        public decimal Valor { get; set; }

        [Display(Name = "Foto")]
        public IFormFile? ArquivoFoto { get; set; }

        public string? FotoAtual { get; set; }

        [Required(ErrorMessage = "O campo Categoria é obrigatório.")]
        [Display(Name = "Categoria")]
        public int CategoriaId { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; } = true;

        // Listas para combobox
        public IEnumerable<SelectListItem>? Categorias { get; set; }

        public List<ProdutoInsumoItemViewModel> InsumosSelecionados { get; set; } = new();
        public IEnumerable<SelectListItem>? InsumosDisponiveis { get; set; }
    }

    public class ProdutoInsumoItemViewModel
    {
        public int InsumoId { get; set; }
        public string InsumoNome { get; set; } = string.Empty;
        public decimal Quantidade { get; set; }
        public string Unidade { get; set; } = string.Empty;
    }
}