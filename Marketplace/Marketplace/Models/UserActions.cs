using System.ComponentModel.DataAnnotations;

namespace Marketplace.Models
{
    public class UserActions
    {
        [Key]
        public int UserActionId { get; set; }

        public string Method { get; set; }

        public string Uri { get; set; }

        public int StatusCode { get; set; }

        public string? Exception { get; set; }

        public DateTime ExecutionTime { get; set; }
    }
}
