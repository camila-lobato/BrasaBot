using BrasaBot.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BrasaBot.ViewModels
{
    public class EstoqueIndexViewModel
    {
        public List<InsumoComEstoqueAnterior> Insumos { get; set; } = new();
        public List<Insumo> InsumosBaixos { get; set; } = new();
        public List<MovimentacaoEstoque> Movimentacoes { get; set; } = new();

        public IEnumerable<SelectListItem> InsumosSelectList { get; set; } = new List<SelectListItem>();
    }

    public class InsumoComEstoqueAnterior
    {
        public Insumo Insumo { get; set; } = null!;
        public decimal EstoqueAnterior { get; set; }
    }

    public class RegistrarMovimentacaoViewModel
    {
        public string Tipo { get; set; } = string.Empty;
        public int InsumoId { get; set; }
        public decimal Quantidade { get; set; }
        public string? Observacao { get; set; }
    }
}