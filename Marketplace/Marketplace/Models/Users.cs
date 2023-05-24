using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace Marketplace.Models
{
    public class Users : IdentityUser
    {
        public Users()
        {
            this.Reviews = new List<Reviews>();
            this.Products= new List<Products>();
            this.Orders = new List<Orders>();
            this.UserActions = new List<UserActions>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime CreationTime { get; set; }

        public ICollection<Reviews> Reviews { get; set; }
        public ICollection<Products> Products { get; set; }
        public ICollection<Orders> Orders { get; set; }
        public ICollection<UserActions> UserActions { get; set; }
    }
}
