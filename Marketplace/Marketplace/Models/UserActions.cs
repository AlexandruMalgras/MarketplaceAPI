using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketplace.Models
{
    public class UserActions
    {
        [Key]
        public int UserActionId { get; set; }

        [ForeignKey("Users")]
        public string UserId { get; set; }
        public Users User { get; set; }

        [Required]
        public string Action { get; set; }

        [Required]
        public string Method { get; set; }

        [Required]
        public string Uri { get; set; }

        [Required]
        public string Result { get; set; }

        [Required]
        public DateTime ExecutionTime { get; set; }
    }
}
