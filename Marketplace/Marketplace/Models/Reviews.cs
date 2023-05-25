using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketplace.Models
{
    public class Reviews
    {
        [Key]
        public int ReviewId { get; set; }

        public int Rating { get; set; }

        public string? Comment { get; set; }
    }
}
