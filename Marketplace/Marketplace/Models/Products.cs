using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketplace.Models
{
    public class Products
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [ForeignKey("Users")]
        public string UserId { get; set; }
        public Users User { get; set; }

        public ICollection<Reviews> Reviews { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }
    }
}
