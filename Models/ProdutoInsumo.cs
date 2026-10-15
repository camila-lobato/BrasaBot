namespace BrasaBot.Models
{
    public class ProdutoInsumo
    {
        public int ProdutoId { get; set; }

        public int InsumoId { get; set; }
        public decimal Quantidade { get; set; }

        public string Unidade { get; set; } = string.Empty;

        public virtual Produto Produto { get; set; } = null!;

        public virtual Insumo Insumo { get; set; } = null!;
    }
}