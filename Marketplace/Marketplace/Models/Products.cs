using System.ComponentModel.DataAnnotations;

namespace Marketplace.Models
{
    public class Products
    {
        public Products() 
        {
            this.Reviews = new List<Reviews>();
            this.OrderItems = new List<OrderItems>();
        }

        [Key]
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public ICollection<Reviews> Reviews { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }
    }
}
