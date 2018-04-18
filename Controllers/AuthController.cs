using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CircleSNS.API.Data;
using CircleSNS.API.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CircleSNS.API.Controllers
{
     [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            this._mapper = mapper;
            this._config = config;
            this._repo = repo;
        }

        
         [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDto userLoginDto)
        {
            var userFromRepo = await _repo.Login(userLoginDto.UserName, userLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var roles = await this._repo.GetRolesForAppUser(userFromRepo);
            //generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value);
            var claimsIdentity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id),
                    new Claim(ClaimTypes.Name, userFromRepo.UserName),
                });
            if(roles != null){
                claimsIdentity.AddClaims(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { tokenString, userFromRepo.Id, userFromRepo.UserName});
        }
    }
}