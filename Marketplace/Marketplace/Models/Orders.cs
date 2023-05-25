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

        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }

        public ICollection<OrderItems> OrderItems { get; set; }
    }
}
