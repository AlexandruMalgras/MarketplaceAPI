using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketplace.Models
{
    public class OrderItems
    {
        [Key]
        public int OrderItemId { get; set; }

        [ForeignKey("Orders")]
        public int OrderId { get; set; }
        public Orders Order { get; set; }

        [ForeignKey("Products")]
        public int ProductId { get; set; }
        public Products Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
