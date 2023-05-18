using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketplace.Models
{
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }

        [ForeignKey("Users")]
        public string UserId { get; set; }
        public Users User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        public ICollection<OrderItems> OrderItems { get; set; }
    }
}
