using System.Collections.Generic;
using System.Threading.Tasks;
using CircleSNS.API.Helper;
using CircleSNS.API.Models;
using Microsoft.AspNetCore.Identity;

namespace CircleSNS.API.Data
{
    public interface IAuthRepository
    {
        Task<T> Register<T>(T userClass, string password) where T: Member; //Will need to add class if added another user relating class
        Task<IdentityResult>  AddRoles(AppUser user, string[] roles);
        Task<AppUser> Login(string username, string password);
        Task UpdateAppUser(AppUser user);
        Task<bool> AppUserExists(string username, string email);
        Task<bool> RoleExists(string role);
        Task<IList<string>> GetRolesForAppUser(AppUser user);
        Task UpdateAppUser(AppUser user, string[] newRoles = null);
        Task<IdentityResult> DelteAppUser(AppUser user);
        Task<AppUser> GetAppUser(string id);
        Task<Member> GetMember(int id);
        Task<IEnumerable<Member>> GetMembers(UserParams userParams);
        Task<IdentityResult> DeleteMember(Member member);
        Task<bool> SaveAll();
    }
}