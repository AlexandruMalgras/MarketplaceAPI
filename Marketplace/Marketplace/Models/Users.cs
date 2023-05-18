using Microsoft.AspNetCore.Identity;

namespace Marketplace.Models
{
    public class Users : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<Reviews> Reviews { get; set; }
        public ICollection<Products> Products { get; set; }
        public ICollection<Orders> Orders { get; set; }
    }
}
