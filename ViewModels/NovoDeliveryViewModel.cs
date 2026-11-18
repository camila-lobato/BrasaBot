using BrasaBot.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BrasaBot.ViewModels
{
    public class NovoDeliveryViewModel
    {
        public int PedidoId { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNome { get; set; } = string.Empty;
        public string ClienteCpf { get; set; } = string.Empty;
        public string ClienteTelefone { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string CEP { get; set; } = string.Empty;
        public decimal TaxaEntrega { get; set; }
        public decimal Total { get; set; }
        public List<ItemPedido> Itens { get; set; } = new();
        public IEnumerable<SelectListItem> Clientes { get; set; } = new List<SelectListItem>();
        public bool TemItens => Itens.Any();
    }
}