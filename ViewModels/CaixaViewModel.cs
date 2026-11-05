using BrasaBot.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BrasaBot.ViewModels
{
    public class AbrirCaixaViewModel
    {
        [Required(ErrorMessage = "Selecione o caixa.")]
        [Range(1, 3, ErrorMessage = "Escolha um caixa válido.")]
        [Display(Name = "Caixa")]
        public int NumeroCaixa { get; set; }

        [Required(ErrorMessage = "Informe o saldo inicial.")]
        [Range(0, double.MaxValue, ErrorMessage = "O saldo inicial não pode ser negativo.")]
        [Display(Name = "Saldo Inicial")]
        public decimal SaldoInicial { get; set; }

        [StringLength(255)]
        [Display(Name = "Observação")]
        public string? Observacao { get; set; }

        public IEnumerable<SelectListItem>? CaixasDisponiveis { get; set; }
    }

    public class InserirDinheiroViewModel
    {
        [Required(ErrorMessage = "Informe o valor.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        [Display(Name = "Valor")]
        public decimal Valor { get; set; }

        [StringLength(255)]
        [Display(Name = "Observação")]
        public string? Observacao { get; set; }
    }

    public class RetirarDinheiroViewModel
    {
        [Required(ErrorMessage = "Informe o valor.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        [Display(Name = "Valor")]
        public decimal Valor { get; set; }

        [StringLength(255)]
        [Display(Name = "Observação")]
        public string? Observacao { get; set; }
    }

    public class ExcluirMovimentacaoViewModel
    {
        [Required]
        public int MovimentacaoId { get; set; }

        [Required(ErrorMessage = "Informe a senha do administrador.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha do Administrador")]
        public string SenhaAdmin { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "E-mail do Administrador")]
        public string EmailAdmin { get; set; } = string.Empty;
    }

    public class FechamentoCaixaViewModel
    {
        public int CaixaId { get; set; }

        [Display(Name = "Dinheiro")]
        [Range(0, double.MaxValue)]
        public decimal ValorDinheiro { get; set; }

        [Display(Name = "Crédito")]
        [Range(0, double.MaxValue)]
        public decimal ValorCredito { get; set; }

        [Display(Name = "Débito")]
        [Range(0, double.MaxValue)]
        public decimal ValorDebito { get; set; }

        [Display(Name = "PIX")]
        [Range(0, double.MaxValue)]
        public decimal ValorPix { get; set; }

        [StringLength(255)]
        [Display(Name = "Observações adicionais")]
        public string? Observacao { get; set; }
    }

    public class CaixaIndexViewModel
    {
        public Caixa? CaixaAberto { get; set; }
        public List<MovimentacaoCaixa> Movimentacoes { get; set; } = new();
        public IEnumerable<SelectListItem> CaixasDisponiveis { get; set; } = new List<SelectListItem>();
    }
}