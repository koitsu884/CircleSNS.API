using Microsoft.AspNetCore.Identity;

namespace CircleSNS.API.Models
{
    public class AppUser : IdentityUser
    {
        public string UserType {get; set;}
    }
}