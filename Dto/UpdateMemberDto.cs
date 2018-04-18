using System.ComponentModel.DataAnnotations;

namespace CircleSNS.API.Dto
{
    public class UpdateMemberDto
    {
        [EmailAddress]
        public string Email { get; set; }
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string KnownAs {get;set;}
        public string Address { get; set; } 
    }
}