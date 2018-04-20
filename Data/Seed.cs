using System.Linq;
using System.Threading.Tasks;
using CircleSNS.API.Models;
using Microsoft.AspNetCore.Identity;

namespace CircleSNS.API.Data
{
    public class Seed
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public Seed(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
            this._context = context;
        }



        public void SeedUsers()
        {
            //_context.Database.EnsureCreated();

            string[] roles = {"Administrator", "Accountant", "Member"};

            foreach(var role in roles){
                if(!_context.Roles.Any(r => r.Name == role)){
                    _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            string password = "P@ssw0rd!!";

            AppUser user = new AppUser { 
                UserName = "Admin",
                Email = "kazunori.hayashi.nz@gmail.com",
                EmailConfirmed = true
            };

            if(!_userManager.Users.Any(u => u.UserName == "Admin"))
            {
                var result = _userManager.CreateAsync(user, password);
                if(result.Result.Succeeded){
                    _userManager.AddToRoleAsync(user, "Administrator");
                }
            }
         }
    }
}