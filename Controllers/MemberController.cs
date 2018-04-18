using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CircleSNS.API.Data;
using CircleSNS.API.Dto;
using CircleSNS.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CircleSNS.API.Controllers
{
    [Route("api/[controller]")]
    public class MemberController : AccountsController
    {

        private readonly string _defaultPassword = "P@ss123!!";
        public MemberController(IAuthRepository repo, IMapper mapper) : base(repo, mapper){}

        [HttpGet("{id}" , Name = "GetMember")]
        [Authorize]
        public async Task<Member> GetMember(int id){
           return await _repo.GetMember(id);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([FromBody]CreateUserDto model)
        {
            if (await _repo.AppUserExists(model.UserName, model.Email))
                ModelState.AddModelError("UserName", "The username or the email already exist");

            if(model.Roles != null)
            {
                foreach(var role in model.Roles){
                if(! await _repo.RoleExists(role))
                    ModelState.AddModelError("Role", "The role '" + role + "' not exists");
                }
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var appUser = _mapper.Map<AppUser>(model);
            
            //Maybe there should be switch case for type of user ex.Customer, Vendor, Admin etc
            var member = _mapper.Map<Member>(model);
            member.Identity = appUser;

            member = await _repo.Register(member, this._defaultPassword);
            await _repo.AddRoles(member.Identity, model.Roles);
            
            return CreatedAtRoute("GetMember", new {id = member.Id});
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, [FromBody]UpdateMemberDto model){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var memberFromRepo = await _repo.GetMember(id);
            if(memberFromRepo == null)
                return NotFound($"Could not find member with an ID of {id}");

            var appUserFromRepo = await _repo.GetAppUser(memberFromRepo.IdentityId);

            if(appUserFromRepo == null)
                return NotFound($"Could not find user with an ID of {id}");

            if(currentUserId != appUserFromRepo.Id)
                return Unauthorized();

            _mapper.Map(model, appUserFromRepo);
            _mapper.Map(model, memberFromRepo);

            await _repo.UpdateAppUser(appUserFromRepo);

            return Ok();
        }
    }
}