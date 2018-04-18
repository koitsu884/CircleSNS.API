using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CircleSNS.API.Data;
using CircleSNS.API.Dto;
using CircleSNS.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CircleSNS.API.Controllers
{
    public class AccountsController : Controller
    {
        protected readonly IMapper _mapper;
        protected readonly IAuthRepository _repo;
        public AccountsController(IAuthRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        [HttpPost("role/{userId}/")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddRoleToAppUser(string userId, [FromBody]string[] roles){
            if(roles == null)
            {
                return BadRequest("No role parameter in the body");
            }
            else
            {
                foreach(var role in roles){
                if(! await _repo.RoleExists(role))
                    return BadRequest("The role '" + role + "' not exists");
                }
            }

            var user = await _repo.GetAppUser(userId);
            if(user == null)
                return BadRequest("Not found the user");


            var result = _repo.AddRoles(user, roles);
            if(result.Result.Succeeded)
                return Ok("The roles were successfully added to " + user.UserName);

            return BadRequest("Failed to add roles"); 
        }
    }
}