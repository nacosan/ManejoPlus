using Microsoft.AspNetCore.Identity;

namespace ManejoPlus.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string Nombre { get; set; }
    }
}
