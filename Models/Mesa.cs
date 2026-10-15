using System.ComponentModel.DataAnnotations;

namespace BrasaBot.Models
{
    public class Mesa
    {
        public int Id { get; set; }

        public int Numero { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}