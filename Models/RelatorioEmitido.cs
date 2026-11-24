using System.ComponentModel.DataAnnotations;

namespace BrasaBot.Models
{
    public class RelatorioEmitido
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoRelatorio { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Filtro { get; set; }

        public DateTime DataInicial { get; set; }

        public DateTime DataFinal { get; set; }

        public DateTime DataEmissao { get; set; } = DateTime.Now;

        public int UsuarioId { get; set; }

        public virtual Usuario Usuario { get; set; } = null!;
    }
}