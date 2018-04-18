namespace CircleSNS.API.Models
{
    public class Member
    {
        public int Id { get; set;}
        public string IdentityId { get; set;}
        public AppUser Identity { get; set;}
        public string FirstName { get; set;}
        public string LastName { get; set;}
        public string KnownAs { get; set;}
        public string Address { get; set;}
    }
}