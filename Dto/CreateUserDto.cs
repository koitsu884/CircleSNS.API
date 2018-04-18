using System.ComponentModel.DataAnnotations;

namespace CircleSNS.API.Dto
{
    public class CreateUserDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName {get;set;}
        public string[] Roles{get; set;}
        public string LastName {get;set;}
        public string Address { get; set; }        
    }
}