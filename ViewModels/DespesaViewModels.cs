using BrasaBot.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BrasaBot.ViewModels
{
    public class DespesaIndexViewModel
    {
        public List<Despesa> Despesas { get; set; } = new();
        public decimal TotalDespesas { get; set; }
        public decimal TotalPendente { get; set; }
        public decimal TotalPago { get; set; }

        public IEnumerable<SelectListItem>? Fornecedores { get; set; }
        public IEnumerable<SelectListItem>? Caixas { get; set; }
    }

    public class NovaDespesaViewModel
    {
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime Vencimento { get; set; } = DateTime.Today;
        public string Status { get; set; } = "A Pagar";
        public string? FormaPagamento { get; set; }
        public string? Observacao { get; set; }
        public int? FornecedorId { get; set; }

        public IEnumerable<SelectListItem>? Fornecedores { get; set; }
    }

    public class PagarDespesaViewModel
    {
        public int DespesaId { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime Vencimento { get; set; }
        public string FormaPagamento { get; set; } = "Dinheiro";
        public int CaixaId { get; set; }

        public IEnumerable<SelectListItem>? Caixas { get; set; }
    }
}