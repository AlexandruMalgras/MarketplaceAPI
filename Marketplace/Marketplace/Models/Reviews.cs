using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketplace.Models
{
    public class Reviews
    {
        [Key]
        public int ReviewId { get; set; }

        [ForeignKey("Products")]
        public int ProductId { get; set; }
        public Products Product { get; set; }

        [ForeignKey("Users")]
        public string UserId { get; set; }
        public Users User { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}
