using System.ComponentModel.DataAnnotations;

namespace BrasaBot.Models
{
    public class Servico
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        public decimal Valor { get; set; }

        public bool Ativo { get; set; } = true;
    }
}