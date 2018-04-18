using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using CircleSNS.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CircleSNS.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager; 
        public AuthRepository(ApplicationDbContext context,
                             UserManager<AppUser> userManager,
                             RoleManager<IdentityRole> roleManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task<AppUser> Login(string userName, string password)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                // get the user to verifty
                var userToVerify = await _userManager.FindByNameAsync(userName);

                if (userToVerify != null)
                {
                    // check the credentials  
                    if (await _userManager.CheckPasswordAsync(userToVerify, password))
                    {
                        return userToVerify;
                    }
                }
            }
            // Credentials are invalid, or account doesn't exist
            return null;
        }

        public async Task<T> Register<T>(T userClass, string password) where T : Member //Will need to add class if added another user relating class
        {
            if(userClass.Identity == null)
            {
                throw new Exception("Failed to register: Identity was not set to " + typeof(T).Name + " class");
            }

            var result = await _userManager.CreateAsync(userClass.Identity, password);

            if (!result.Succeeded) 
            {
                throw new Exception("Failed to register");
            }
            _context.Add(userClass);
            if( await _context.SaveChangesAsync() > 0)
            {
                return userClass;
            }

            throw new Exception("Failed to save register infromation");
        }


        public async Task UpdateAppUser(AppUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) 
            {
                throw new Exception("Failed to register");
            }
        }

        public async Task<bool> AppUserExists(string username, string email)
        {
            if(await _context.Users.AnyAsync(x => x.UserName == username || x.Email == email))
                return true;
            return false;
        }

        public async Task<bool> RoleExists(string role){
            return await _roleManager.RoleExistsAsync(role);
        }

        public async Task<AppUser> GetAppUser(string id){
            return await _userManager.FindByIdAsync(id);
        }

        public async Task UpdateAppUser(AppUser user, string[] newRoles = null){
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded) 
            {
                throw new Exception("Failed to update user");
            }
            if( await _context.SaveChangesAsync() > 0)
            {
                if(newRoles != null){
                    await this._userManager.RemoveFromRolesAsync(user, await this._userManager.GetRolesAsync(user));
                    await this._userManager.AddToRolesAsync(user, newRoles);
                }
            }
        }

        public async Task<IList<string>> GetRolesForAppUser(AppUser user){
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Member> GetMember(int id){
            return await _context.Members.FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<IdentityResult> AddRoles(AppUser user, string[] roles)
        {
            await this._userManager.RemoveFromRolesAsync(user, await this._userManager.GetRolesAsync(user));
            return await this._userManager.AddToRolesAsync(user, roles);
        }
    }
}