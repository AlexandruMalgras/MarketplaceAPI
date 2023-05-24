using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public ICollection<Reviews> Reviews { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }
    }
}
