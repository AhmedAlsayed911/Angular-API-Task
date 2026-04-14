using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Madrasty_API.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(30)]
        public string FirstName { get; set; }
        [MaxLength(30)]
        public string LastName { get; set; }
    }
}
