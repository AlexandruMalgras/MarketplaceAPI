using System.ComponentModel.DataAnnotations;

namespace Marketplace.Models
{
    public class Orders
    {
        public Orders()
        {
            this.OrderItems = new List<OrderItems>();
        }

        [Key]
        public int OrderId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        public ICollection<OrderItems> OrderItems { get; set; }
    }
}
