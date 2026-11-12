using System.ComponentModel.DataAnnotations;

namespace BrasaBot.Models
{
    public class Cargo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        public bool Ativo { get; set; } = true;
    }
}