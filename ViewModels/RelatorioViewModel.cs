using BrasaBot.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BrasaBot.ViewModels
{
    public class RelatorioIndexViewModel
    {
        public List<RelatorioEmitido> Historico { get; set; } = new();

        // Filtros
        public DateTime DataInicial { get; set; } = DateTime.Today.AddDays(-30);
        public DateTime DataFinal { get; set; } = DateTime.Today;
        public string TipoRelatorio { get; set; } = string.Empty;
        public string Filtro { get; set; } = "Todos";

        public IEnumerable<SelectListItem>? TiposRelatorio { get; set; }
        public IEnumerable<SelectListItem>? FiltrosDisponiveis { get; set; }
    }

    public class RelatorioVendasViewModel
    {
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string Filtro { get; set; } = "Todos";
        public string UsuarioEmissor { get; set; } = string.Empty;

        public decimal TotalVendas { get; set; }
        public int TotalPedidos { get; set; }
        public decimal TicketMedio { get; set; }

        public List<ItemRelatorioVendas> Itens { get; set; } = new();
    }

    public class ItemRelatorioVendas
    {
        public DateTime Data { get; set; }
        public string Detalhes { get; set; } = string.Empty;
        public string Funcionario { get; set; } = string.Empty;
        public string FormaPagamento { get; set; } = string.Empty;
        public string TipoPedido { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public decimal Desconto { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class RelatorioCaixaViewModel
    {
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string Filtro { get; set; } = "Todos";
        public string UsuarioEmissor { get; set; } = string.Empty;

        public decimal TotalEntradas { get; set; }
        public decimal TotalSaidas { get; set; }
        public decimal SaldoFinal { get; set; }

        public List<ItemRelatorioCaixa> Itens { get; set; } = new();
    }

    public class ItemRelatorioCaixa
    {
        public DateTime Data { get; set; }
        public string Hora { get; set; } = string.Empty;
        public string Detalhes { get; set; } = string.Empty;
        public string FormaPagamento { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public decimal Desconto { get; set; }
        public string Tipo { get; set; } = string.Empty;
    }

    public class RelatorioDespesasViewModel
    {
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string Filtro { get; set; } = "Todos";
        public string UsuarioEmissor { get; set; } = string.Empty;

        public decimal TotalPeriodo { get; set; }
        public decimal TotalPago { get; set; }
        public decimal TotalPendente { get; set; }

        public List<ItemRelatorioDespesas> Itens { get; set; } = new();
    }

    public class ItemRelatorioDespesas
    {
        public DateTime Data { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public string Funcionario { get; set; } = string.Empty;
        public DateTime Vencimento { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class RelatorioFinanceiroViewModel
    {
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string Filtro { get; set; } = "Todos";
        public string UsuarioEmissor { get; set; } = string.Empty;

        public decimal FaturamentoBruto { get; set; }
        public decimal TotalDespesas { get; set; }
        public decimal LucroBruto { get; set; }
        public int TotalPedidos { get; set; }
        public decimal TicketMedio { get; set; }
    }

    public class RelatorioProdutosViewModel
    {
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string Filtro { get; set; } = "Todos";
        public string UsuarioEmissor { get; set; } = string.Empty;

        public List<ItemRelatorioProdutos> Itens { get; set; } = new();
    }

    public class ItemRelatorioProdutos
    {
        public int Id { get; set; }
        public string Produto { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int QuantidadeVendida { get; set; }
        public decimal ReceitaTotal { get; set; }
    }

    public class RelatorioEstoqueViewModel
    {
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string Filtro { get; set; } = "Todos";
        public string UsuarioEmissor { get; set; } = string.Empty;

        public List<ItemRelatorioEstoque> Itens { get; set; } = new();
    }

    public class ItemRelatorioEstoque
    {
        public string Insumo { get; set; } = string.Empty;
        public string Unidade { get; set; } = string.Empty;
        public decimal EstoqueAtual { get; set; }
        public decimal EstoqueMinimo { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}