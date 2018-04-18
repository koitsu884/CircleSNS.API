using System.ComponentModel.DataAnnotations;

namespace CircleSNS.API.Dto
{
    public class RegistrationDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(8, MinimumLength=4, ErrorMessage="You must specify a password between 4 and 8")]
        public string Password { get; set; }
        [Required]
        public string FirstName {get;set;}
        [Required]
        public string LastName {get;set;}
        public string Address { get; set; }        
    }
}